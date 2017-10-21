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

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    public class ReviewRepository : AbstractRepository, IReviewRepository
    {
        private const string notFoundMessage = "Review not found";
        private const string alreadyExistsMessage = "Review already exists";

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
                throw new NotFoundException("User not found");
            }

            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);

            var query = db.Reviews.Include(r => r.Book).Where(r => r.UserID == userID);

            var totalNumberOfItems = query.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var reviewEntities = query.Skip((pageNumber - 1) * maxSize).Take(maxSize).ToList();

            var userReviewDTOs = Mapper.Map<IList<ReviewEntity>, IList<UserReviewDTO>>(reviewEntities);

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
                throw new NotFoundException("Book not found");
            }

            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);

            var query = db.Reviews.Include(r => r.User).Where(r => r.BookID == bookID);

            var totalNumberOfItems = query.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var reviewEntities = query.Skip((pageNumber - 1) * maxSize).Take(maxSize).ToList();

            var bookReviewDTOs = Mapper.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(reviewEntities);

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
                throw new NotFoundException("User not found");
            }

            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException("Book not found");
            }

            // Get review from database
            var reviewEntity = db.Reviews
                        .Include(r => r.User)
                        .Include(r => r.Book)
                        .Where(r => r.UserID == userID && r.BookID == bookID)
                        .SingleOrDefault();

            if (reviewEntity == null)
            {
                throw new NotFoundException("Review not found");
            }

            var reviewDTO = Mapper.Map<ReviewEntity, ReviewDTO>(reviewEntity);

            return reviewDTO;
        }

        public ReviewDTO AddReview(int userID, int bookID, ReviewViewModel review)
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

            // Check if review already exists
            if (db.Reviews.Where(r => r.UserID == userID && r.BookID == bookID).Any())
            {
                throw new AlreadyExistsException(alreadyExistsMessage);
            }

            // Add review
            var reviewEntity = mapper.Map<ReviewViewModel, ReviewEntity>(review);
            reviewEntity.BookID = bookID;
            reviewEntity.UserID = userID;

            db.Reviews.Add(reviewEntity);
            db.SaveChanges();

            var user = db.Users.Where(u => u.ID == userID).SingleOrDefault();
            var userDTO = mapper.Map<UserEntity, UserDTO>(user);

            var book = db.Books.Where(b => b.ID == bookID).SingleOrDefault();
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
                throw new NotFoundException("User not found");
            }

            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException("Book not found");
            }

            // Get entity from DB
            var reviewEntity = db.Reviews.Where(r => r.UserID == userID && r.BookID == bookID).SingleOrDefault();

            // Check if review exists
            if (reviewEntity == null)
            {
                throw new NotFoundException("Review not found");
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
                throw new NotFoundException("User not found");
            }

            // Check if book exists
            if (!db.Books.Where(b => b.ID == bookID).Any())
            {
                throw new NotFoundException("Book not found");
            }

            // Get entity from DB
            var reviewEntity = db.Reviews.Where(r => r.UserID == userID && r.BookID == bookID).SingleOrDefault();

            // Check if review exists
            if (reviewEntity == null)
            {
                throw new NotFoundException("Review not found");
            }

            db.Reviews.Remove(reviewEntity);
            db.SaveChanges();
        }

        public Envelope<RecommendationDTO> GetRecommendationsByUserID(int userID, int pageNumber, int? pageMaxSize)
        {
            // Check if user exists
            if (!db.Users.Where(u => u.ID == userID).Any())
            {
                throw new NotFoundException("User not found");
            }

            var query =
            (
                from b in db.Books
                join r in db.Reviews on b.ID equals r.BookID into rs
                from r in rs.DefaultIfEmpty()
                    //where !db.Loans.Any(l => l.UserID == userID)
                group r by b.ID into g
                from p in g
                let AverageRating = g.Average(ga => ga.Rating)
                orderby AverageRating descending
                select new { Book = p.Book, AverageRating = AverageRating }
            );

            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);

            var totalNumberOfItems = query.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var recommendations = query.Skip((pageNumber - 1) * maxSize).Take(maxSize).ToList();

            var recommendationDTOs = new List<RecommendationDTO>();
            foreach (var item in recommendations)
            {
                var recommendationDTO = new RecommendationDTO
                {
                    Book = mapper.Map<BookEntity, BookDTO>(item.Book),
                    AverageRating = item.AverageRating
                };

                recommendationDTOs.Add(recommendationDTO);
            }

            return new Envelope<RecommendationDTO>
            {
                Items = recommendationDTOs,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = recommendationDTOs.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }
    }
}