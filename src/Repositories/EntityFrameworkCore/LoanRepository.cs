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
    public class LoanRepository : AbstractRepository, ILoanRepository
    {
        private const string notFoundMessage = "Loan not found";
        private const string alreadyExistsMessage = "Loan already exists";
        private const string dateNonsenseMessage = "Loan date must be before the return date";
        private const int monthDays = 30;

        public LoanRepository(DatabaseContext db, IMapper mapper)
            : base(db, mapper)
        {
        }

        public Envelope<LoanDTO> GetLoans(int pageNumber, int? pageMaxSize, int? userID, int? bookID, DateTime? date, bool? monthSpan)
        {
            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);

            var query = db.Loans
                        .Include(l => l.Book)
                        .Include(l => l.User)
                        .OrderBy(l => l.ID)
                        .Select(l => l);

            // Filter by user
            if (userID.HasValue)
            {
                query = query.Where(l => l.UserID == userID.Value);
            }

            // Filter by book
            if (bookID.HasValue)
            {
                query = query.Where(l => l.BookID == bookID.Value);
            }

            // Loan spans over the date
            if (date.HasValue)
            {
                query = query.Where(l =>
                    l.LoanDate <= date.Value
                    && (!l.ReturnDate.HasValue
                        || (l.ReturnDate.HasValue && date.Value <= l.ReturnDate.Value)
                        )
                );
            }

            // Loans that have been ongoing for more than a month
            if (monthSpan.HasValue && monthSpan.Value == true)
            {
                // Default to the current date if no date specified
                var dateToUse = DateTime.Now;

                if (date.HasValue)
                {
                    dateToUse = date.Value;
                }

                query = query.Where(l =>
                    monthDays <= (dateToUse - l.LoanDate).TotalDays
                    && (!l.ReturnDate.HasValue
                        || (l.ReturnDate.HasValue && dateToUse <= l.ReturnDate.Value)
                        )
                );
            }

            var totalNumberOfItems = query.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var loanEntities = query.Skip((pageNumber - 1) * maxSize).Take(maxSize).ToList();

            var loanDTOs = Mapper.Map<IList<LoanEntity>, IList<LoanDTO>>(loanEntities);

            return new Envelope<LoanDTO>
            {
                Items = loanDTOs,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = loanDTOs.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            }; ;
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

            var userLoansDTO = Mapper.Map<IList<LoanEntity>, IList<UserLoanDTO>>(loanEntities);

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

        public LoanDTO GetLoanByID(int loanID)
        {
            var loanEntity = db.Loans
                            .Include(l => l.Book)
                            .Include(l => l.User)
                            .Where(b => b.ID == loanID)
                            .SingleOrDefault();

            if (loanEntity == null)
            {
                throw new NotFoundException(notFoundMessage);
            }

            var loanDTO = Mapper.Map<LoanEntity, LoanDTO>(loanEntity);

            return loanDTO;
        }

        public int AddLoan(int userID, int bookID)
        {
            // Check if user exists
            if (!db.Users.Where(u => u.ID == userID).Any())
            {
                throw new NotFoundException("User not found");
            }

            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException("Book not found");
            }

            // Check if user already has book loaned
            if (db.Loans.Where(l => l.BookID == bookID && l.UserID == userID && l.ReturnDate == null).Any())
            {
                throw new AlreadyExistsException("User already has book loaned");
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
                throw new NotFoundException("User not found");
            }

            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException("Book not found");
            }

            var loanEntity = db.Loans
                                .Where(l => l.BookID == bookID && l.UserID == userID && l.ReturnDate == null)
                                .SingleOrDefault();

            // Check if user already has book loaned
            if (loanEntity == null)
            {
                throw new NotFoundException("User does not have the book loaned");
            }

            loanEntity.ReturnDate = DateTime.Now;

            db.Loans.Update(loanEntity);
            db.SaveChanges();
        }

        public int AddLoan(LoanViewModel loan)
        {
            var loanEntity = mapper.Map<LoanViewModel, LoanEntity>(loan);

            // Check if loan date is before return date
            if (loan.ReturnDate.HasValue && loan.ReturnDate.Value < loan.LoanDate)
            {
                throw new InvalidDataException(dateNonsenseMessage);
            }

            // // Check if user exists
            // if (!db.Users.Where(u => u.ID == loan.UserID).Any())
            // {
            //     throw new NotFoundException("User not found");
            // }

            // // Check if book exists
            // if (!db.Books.Where(b => b.ID == loan.BookID).Any())
            // {
            //     throw new NotFoundException("Book not found");
            // }

            // Add loan
            db.Loans.Add(loanEntity);
            db.SaveChanges();

            return loanEntity.ID;
        }

        public void UpdateLoan(int loanID, LoanViewModel loan)
        {
            throw new NotImplementedException();
        }

        public void UpdateLoan(int userID, int bookID, PatchLoanViewModel loan)
        {
            // Check if user exists
            if (!db.Users.Where(u => u.ID == userID).Any())
            {
                throw new NotFoundException("User not found");
            }

            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException("Book not found");
            }

            // Get entity from database
            var loanEntity = db.Loans
                                .Where(l => l.UserID == userID && l.BookID == bookID && l.ReturnDate == null)
                                .SingleOrDefault();

            if (loanEntity == null)
            {
                throw new NotFoundException("User does not have the book loaned");
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

        public void DeleteLoanByID(int loanID)
        {
            var loanEntity = db.Loans.Where(l => l.ID == loanID).SingleOrDefault();

            if (loanEntity == null)
            {
                throw new NotFoundException(notFoundMessage);
            }

            db.Loans.Remove(loanEntity);
            db.SaveChanges();
        }
    }
}