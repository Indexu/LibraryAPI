using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models;
using LibraryAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;
using System.Data.Common;

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    /// <summary>
    /// An implementation of the review repository using Entity Framework Core
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class ReviewRepository : AbstractRepository, IReviewRepository
    {
        public ReviewRepository(DatabaseContext db, IMapper mapper)
            : base(db, mapper)
        {
        }

        public Envelope<BookReviewsDTO> GetBookReviews(int pageNumber, int? pageMaxSize)
        {
            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);

            // Group and order by user ID
            var group = db.Reviews.Include(r => r.User).Include(r => r.Book).GroupBy(r => r.BookID, r => r).ToList();
            group.OrderBy(g => g.Key);

            var totalNumberOfItems = group.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var report = group.Select(g => g.ToList()).Skip((pageNumber - 1) * maxSize).Take(maxSize);

            // Map entities over to DTOs
            var bookReviewsDTOs = new List<BookReviewsDTO>();
            foreach (var entry in report)
            {
                var bookReviewsDTO = new BookReviewsDTO
                {
                    Book = mapper.Map<BookEntity, BookDTO>(entry[0].Book),
                    Reviews = mapper.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(entry)
                };

                bookReviewsDTOs.Add(bookReviewsDTO);
            }

            return new Envelope<BookReviewsDTO>
            {
                Items = bookReviewsDTOs,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = bookReviewsDTOs.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public Envelope<UserReviewDTO> GetReviewsByUserID(int userID, int pageNumber, int? pageMaxSize)
        {
            // Check if user exists
            if (!db.Users.Where(u => u.ID == userID).Any())
            {
                throw new NotFoundException(userNotFoundMessage);
            }

            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);

            var query = db.Reviews.Include(r => r.Book).Where(r => r.UserID == userID);

            var totalNumberOfItems = query.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var reviewEntities = query.Skip((pageNumber - 1) * maxSize).Take(maxSize).ToList();

            var userReviewDTOs = mapper.Map<IList<ReviewEntity>, IList<UserReviewDTO>>(reviewEntities);

            return new Envelope<UserReviewDTO>
            {
                Items = userReviewDTOs,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = userReviewDTOs.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public Envelope<BookReviewDTO> GetReviewsByBookID(int bookID, int pageNumber, int? pageMaxSize)
        {
            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException(bookNotFoundMessage);
            }

            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);

            var query = db.Reviews.Include(r => r.User).Where(r => r.BookID == bookID);

            var totalNumberOfItems = query.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var reviewEntities = query.Skip((pageNumber - 1) * maxSize).Take(maxSize).ToList();

            var bookReviewDTOs = mapper.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(reviewEntities);

            return new Envelope<BookReviewDTO>
            {
                Items = bookReviewDTOs,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = bookReviewDTOs.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public ReviewDTO GetReview(int userID, int bookID)
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

            // Get review from database
            var reviewEntity = db.Reviews
                        .Include(r => r.User)
                        .Include(r => r.Book)
                        .Where(r => r.UserID == userID && r.BookID == bookID)
                        .SingleOrDefault();

            if (reviewEntity == null)
            {
                throw new NotFoundException(reviewNotFoundMessage);
            }

            var reviewDTO = mapper.Map<ReviewEntity, ReviewDTO>(reviewEntity);

            return reviewDTO;
        }

        public ReviewDTO AddReview(int userID, int bookID, ReviewViewModel review)
        {
            var user = db.Users.Where(u => u.ID == userID).SingleOrDefault();

            // Check if user exists
            if (user == null)
            {
                throw new NotFoundException(userNotFoundMessage);
            }

            var book = db.Books.Where(b => b.ID == bookID).SingleOrDefault();

            // Check if book exists
            if (book == null)
            {
                throw new NotFoundException(bookNotFoundMessage);
            }


            // Check if review already exists
            if (db.Reviews.Where(r => r.UserID == userID && r.BookID == bookID).Any())
            {
                throw new AlreadyExistsException(reviewAlreadyExistsMessage);
            }

            // Add review
            var reviewEntity = mapper.Map<ReviewViewModel, ReviewEntity>(review);
            reviewEntity.BookID = bookID;
            reviewEntity.UserID = userID;

            db.Reviews.Add(reviewEntity);
            db.SaveChanges();

            var userDTO = mapper.Map<UserEntity, UserDTO>(user);
            var bookDTO = mapper.Map<BookEntity, BookDTO>(book);

            var reviewDTO = mapper.Map<ReviewEntity, ReviewDTO>(reviewEntity);
            reviewDTO.Book = bookDTO;
            reviewDTO.User = userDTO;

            return reviewDTO;
        }

        public void UpdateReview(int userID, int bookID, PatchReviewViewModel review)
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

            // Get entity from DB
            var reviewEntity = db.Reviews.Where(r => r.UserID == userID && r.BookID == bookID).SingleOrDefault();

            // Check if review exists
            if (reviewEntity == null)
            {
                throw new NotFoundException(reviewNotFoundMessage);
            }

            // Change rating
            if (review.Rating.HasValue)
            {
                reviewEntity.Rating = review.Rating.Value;
            }

            db.Reviews.Update(reviewEntity);
            db.SaveChanges();
        }

        public void DeleteReview(int userID, int bookID)
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

            // Get entity from DB
            var reviewEntity = db.Reviews.Where(r => r.UserID == userID && r.BookID == bookID).SingleOrDefault();

            // Check if review exists
            if (reviewEntity == null)
            {
                throw new NotFoundException(reviewNotFoundMessage);
            }

            db.Reviews.Remove(reviewEntity);
            db.SaveChanges();
        }

        public Envelope<RecommendationDTO> GetRecommendationsByUserID(int userID, int pageNumber, int? pageMaxSize)
        {
            // Check if user exists
            if (!db.Users.Where(u => u.ID == userID).Any())
            {
                throw new NotFoundException(userNotFoundMessage);
            }

            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);
            var totalNumberOfItems = 0;
            var pageCount = 0;

            // Doing left outer join with grouping and aggregation in LINQ is a pain.
            // I tried for hours and decided to do it the poor mans way, raw SQL style.

            var resultList = new List<RecommendationDTO>();

            // Get the DB connection
            var conn = db.Database.GetDbConnection();
            try
            {
                // Open connection and let the fun begin
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    // The SQL query to get the total number of books being recommended
                    var countQuery = String.Format("SELECT COUNT(*) "
                                            + "FROM "
                                            + "( "
                                                + "SELECT 1 "
                                                + "FROM Books b "
                                                + "LEFT JOIN Reviews r ON b.ID = r.BookID "
                                                + "WHERE NOT EXISTS "
                                                + "( "
                                                    + "SELECT 1 "
                                                    + "FROM Loans l "
                                                    + "WHERE l.BookID = b.ID AND l.UserID = {0} "
                                                + ") "
                                                + "AND NOT EXISTS "
                                                + "( "
                                                    + "SELECT 1 "
                                                    + "FROM Reviews r2 "
                                                    + "WHERE r2.BookID = b.ID AND r2.UserID = {0} "
                                                + ") "
                                                + "GROUP BY b.ID "
                                            + ") ",
                                            userID);

                    // Set and execute the query
                    command.CommandText = countQuery;
                    var reader = command.ExecuteReader();

                    // Get the count
                    if (reader.HasRows)
                    {
                        reader.Read();
                        totalNumberOfItems = reader.GetInt32(0);
                    }

                    reader.Dispose();

                    // Calculate pagination
                    pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);
                    var skip = (pageNumber - 1) * maxSize;

                    // The SQL query for the recommendations
                    var recommendationsQuery = String.Format("SELECT b.ID, b.Title, b.Author, b.PublishDate, b.ISBN, avg(r.Rating) "
                                            + "FROM Books b "
                                            + "LEFT JOIN Reviews r ON b.ID = r.BookID "
                                            + "WHERE NOT EXISTS "
                                            + "( "
                                                + "SELECT 1 "
                                                + "FROM Loans l "
                                                + "WHERE l.BookID = b.ID AND l.UserID = {0} "
                                            + ") "
                                            + "AND NOT EXISTS "
                                            + "( "
                                                + "SELECT 1 "
                                                + "FROM Reviews r2 "
                                                + "WHERE r2.BookID = b.ID AND r2.UserID = {0} "
                                            + ") "
                                            + "GROUP BY b.ID "
                                            + "ORDER BY avg(r.Rating) DESC, b.Title ASC "
                                            + "LIMIT {1}, {2}",
                                            userID, skip, maxSize);

                    // Set and execute the query
                    command.CommandText = recommendationsQuery;
                    reader = command.ExecuteReader();

                    // Read over and add the rows
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var row = new RecommendationDTO
                            {
                                Book = new BookDTO
                                {
                                    ID = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Author = reader.GetString(2),
                                    PublishDate = reader.GetDateTime(3),
                                    ISBN = reader.GetString(4),
                                }
                            };

                            if (!reader.IsDBNull(5))
                            {
                                row.AverageRating = reader.GetDouble(5);
                            }

                            resultList.Add(row);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }

            return new Envelope<RecommendationDTO>
            {
                Items = resultList,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = resultList.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }
    }
}