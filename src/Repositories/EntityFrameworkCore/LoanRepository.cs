using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    /// <summary>
    /// An implementation of the loan repository using Entity Framework Core
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class LoanRepository : AbstractRepository, ILoanRepository
    {
        public LoanRepository(DatabaseContext db, IMapper mapper)
            : base(db, mapper)
        {
        }

        public Envelope<UserLoanDTO> GetLoansByUserID(int userID, bool active, int pageNumber, int? pageMaxSize)
        {
            var query = db.Loans
                        .Include(l => l.Book)
                        .Where(l => l.UserID == userID && (active ? l.ReturnDate == null : true))
                        .OrderByDescending(l => l.LoanDate);

            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);
            var totalNumberOfItems = query.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var loanEntities = query
                                .Select(l => l)
                                .Skip((pageNumber - 1) * maxSize)
                                .Take(maxSize)
                                .ToList();

            var userLoansDTO = mapper.Map<IList<LoanEntity>, IList<UserLoanDTO>>(loanEntities);

            return new Envelope<UserLoanDTO>
            {
                Items = userLoansDTO,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = userLoansDTO.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public Envelope<BookLoanDTO> GetLoansByBookID(int bookID, bool active, int pageNumber, int? pageMaxSize)
        {
            var query = db.Loans
                        .Include(l => l.User)
                        .Where(l => l.BookID == bookID && (active ? l.ReturnDate == null : true))
                        .OrderByDescending(l => l.LoanDate);

            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);
            var totalNumberOfItems = query.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var loanEntities = query
                                .Select(l => l)
                                .Skip((pageNumber - 1) * maxSize)
                                .Take(maxSize)
                                .ToList();

            var bookLoansDTO = mapper.Map<IList<LoanEntity>, IList<BookLoanDTO>>(loanEntities);

            return new Envelope<BookLoanDTO>
            {
                Items = bookLoansDTO,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = bookLoansDTO.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public int AddLoan(int userID, int bookID)
        {
            // Check if user exists
            if (!db.Users.Where(u => u.ID == userID).Any())
            {
                throw new NotFoundException(userNotFoundMessage);
            }

            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException(bookNotFoundMessage);
            }

            // Check if user already has book loaned
            if (db.Loans.Where(l => l.BookID == bookID && l.UserID == userID && l.ReturnDate == null).Any())
            {
                throw new AlreadyExistsException(loanAlreadyExistsMessage);
            }

            var loanEntity = new LoanEntity
            {
                UserID = userID,
                BookID = bookID,
                LoanDate = DateTime.Now,
                ReturnDate = null
            };

            // Add loan
            db.Loans.Add(loanEntity);
            db.SaveChanges();

            return loanEntity.ID;
        }

        public void ReturnBook(int userID, int bookID)
        {
            // Check if user exists
            if (!db.Users.Where(u => u.ID == userID).Any())
            {
                throw new NotFoundException(userNotFoundMessage);
            }

            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException(bookNotFoundMessage);
            }

            var loanEntity = db.Loans
                                .Where(l => l.BookID == bookID && l.UserID == userID && l.ReturnDate == null)
                                .SingleOrDefault();

            // Check if user doesn't have the book loaned
            if (loanEntity == null)
            {
                throw new NotFoundException(loanNotFoundMessage);
            }

            loanEntity.ReturnDate = DateTime.Now;

            db.Loans.Update(loanEntity);
            db.SaveChanges();
        }

        public Envelope<UserReportDTO> GetUsersReport(int pageNumber, int? pageMaxSize, DateTime? loanDate, int? duration)
        {
            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);

            var query = db.Loans.AsQueryable();

            // Filter by date
            if (loanDate.HasValue)
            {
                query = query.Where(l => l.LoanDate <= loanDate.Value && (!l.ReturnDate.HasValue || (loanDate <= l.ReturnDate)));
            }

            // Filter by duration
            if (duration.HasValue)
            {
                DateTime dateToUse = loanDate.HasValue ? loanDate.Value : DateTime.Now;
                query = query.Where(l =>
                    (duration.Value <= (dateToUse - l.LoanDate).TotalDays)
                    && (!l.ReturnDate.HasValue
                        || (l.ReturnDate.HasValue && dateToUse <= l.ReturnDate.Value)));
            }

            // Group and order by user ID
            var group = query.Include(l => l.User).Include(l => l.Book).GroupBy(l => l.UserID, l => l).ToList();
            group.OrderBy(g => g.Key);

            var totalNumberOfItems = group.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var report = group.Select(g => g.ToList()).Skip((pageNumber - 1) * maxSize).Take(maxSize);

            // Map entities over to DTOs
            var userReportDTOs = new List<UserReportDTO>();
            foreach (var entry in report)
            {
                var userReportDTO = new UserReportDTO
                {
                    User = mapper.Map<UserEntity, UserDTO>(entry[0].User),
                    UserLoans = mapper.Map<IList<LoanEntity>, IList<UserLoanDTO>>(entry)
                };

                userReportDTOs.Add(userReportDTO);
            }

            return new Envelope<UserReportDTO>
            {
                Items = userReportDTOs,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = userReportDTOs.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public Envelope<BookReportDTO> GetBooksReport(int pageNumber, int? pageMaxSize, DateTime? loanDate, int? duration)
        {
            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);

            var query = db.Loans.AsQueryable();

            // Filter by date
            if (loanDate.HasValue)
            {
                query = query.Where(l => l.LoanDate <= loanDate.Value && (!l.ReturnDate.HasValue || (loanDate <= l.ReturnDate)));
            }

            // Filter by duration
            if (duration.HasValue)
            {
                DateTime dateToUse = loanDate.HasValue ? loanDate.Value : DateTime.Now;
                query = query.Where(l =>
                    (duration.Value <= (dateToUse - l.LoanDate).TotalDays)
                    && (!l.ReturnDate.HasValue
                        || (l.ReturnDate.HasValue && dateToUse <= l.ReturnDate.Value)));
            }

            // Group and order by book ID
            var group = query.Include(l => l.User).Include(l => l.Book).GroupBy(l => l.BookID, l => l).ToList();
            group.OrderBy(g => g.Key);

            var totalNumberOfItems = group.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var report = group.Select(g => g.ToList()).Skip((pageNumber - 1) * maxSize).Take(maxSize);

            // Map entities over to DTOs
            var bookReportDTOs = new List<BookReportDTO>();
            foreach (var entry in report)
            {
                var bookReportDTO = new BookReportDTO
                {
                    Book = mapper.Map<BookEntity, BookDTO>(entry[0].Book),
                    BookLoans = mapper.Map<IList<LoanEntity>, IList<BookLoanDTO>>(entry)
                };

                bookReportDTOs.Add(bookReportDTO);
            }

            return new Envelope<BookReportDTO>
            {
                Items = bookReportDTOs,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = bookReportDTOs.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public void UpdateLoan(int userID, int bookID, PatchLoanViewModel loan)
        {
            // Check if user exists
            if (!db.Users.Where(u => u.ID == userID).Any())
            {
                throw new NotFoundException(userNotFoundMessage);
            }

            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException(bookNotFoundMessage);
            }

            // Get entity from database
            var loanEntity = db.Loans
                                .Where(l => l.UserID == userID && l.BookID == bookID && l.ReturnDate == null)
                                .SingleOrDefault();

            if (loanEntity == null)
            {
                throw new NotFoundException(loanNotFoundMessage);
            }

            // Check if loan date is before return date
            if (loan.ReturnDate.HasValue)
            {
                if ((loan.LoanDate.HasValue && loan.ReturnDate.Value < loan.LoanDate.Value) ||
                    (!loan.LoanDate.HasValue && loan.ReturnDate.Value < loanEntity.LoanDate))
                {
                    throw new InvalidDataException(dateNonsenseMessage);
                }
            }

            // Loan date
            if (loan.LoanDate.HasValue)
            {
                loanEntity.LoanDate = loan.LoanDate.Value;
            }

            // Return date
            if (loan.ReturnDate.HasValue)
            {
                loanEntity.ReturnDate = loan.ReturnDate.Value;
            }

            db.Loans.Update(loanEntity);
            db.SaveChanges();
        }
    }
}