using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models;
using LibraryAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryAPI.Tests.MockUtils
{
    public class MockDataGenerator
    {
        public static Random rand = new Random();

        public static List<UserDTO> CreateUsers(int n)
        {
            var users = new List<UserDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;
                var user = CreateUser(ID);

                users.Add(user);
            }

            return users;
        }

        public static Envelope<UserDTO> CreateUsersEnvelope(IEnumerable<UserDTO> users, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = users.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var selectedUsers = users.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<UserDTO>
            {
                Items = selectedUsers,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = selectedUsers.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public static UserDTO CreateUser(int userID)
        {
            return new UserDTO
            {
                ID = userID,
                Name = String.Format("User {0}", userID),
                Email = String.Format("user@user{0}.com", userID),
                Address = String.Format("{0} Main Street", userID)
            };
        }

        public static UserViewModel CreateUserViewModel(int userID)
        {
            return new UserViewModel
            {
                Name = String.Format("User {0}", userID),
                Email = String.Format("user@user{0}.com", userID),
                Address = String.Format("{0} Main Street", userID)
            };
        }

        public static PatchUserViewModel CreatePatchUserViewModel(int userID)
        {
            return new PatchUserViewModel
            {
                Name = String.Format("User {0}", userID),
                Email = String.Format("user@user{0}.com", userID),
                Address = String.Format("{0} Main Street", userID)
            };
        }

        public static UserDetailsDTO CreateUserDetailsBasic(int userID)
        {
            return new UserDetailsDTO
            {
                ID = userID,
                Name = String.Format("User {0}", userID),
                Email = String.Format("user@user{0}.com", userID),
                Address = String.Format("{0} Main Street", userID)
            };
        }

        public static UserDetailsDTO CreateUserDetails(int userID, int numberOfLoans, int pageNumber, int? pageSize)
        {
            var userDetails = new UserDetailsDTO
            {
                ID = userID,
                Name = String.Format("User {0}", userID),
                Email = String.Format("user@user{0}.com", userID),
                Address = String.Format("{0} Main Street", userID),
            };

            var loans = CreateUserLoans(numberOfLoans);
            var envelope = CreateUsersLoansEnvelope(loans, pageNumber, pageSize);

            userDetails.LoanHistory = envelope;

            return userDetails;
        }

        public static List<UserLoanDTO> CreateUserLoans(int n)
        {
            var loans = new List<UserLoanDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var loan = new UserLoanDTO
                {
                    Book = CreateBook(1),
                    LoanDate = new DateTime(2010 + ID, 1, 1),
                    ReturnDate = (i % 2 == 0 ? (DateTime?)new DateTime(2010 + ID, 3, 1) : null)
                };

                loans.Add(loan);
            }

            return loans;
        }

        public static Envelope<UserLoanDTO> CreateUsersLoansEnvelope(IEnumerable<UserLoanDTO> loans, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = loans.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var users = loans.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<UserLoanDTO>
            {
                Items = loans,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = loans.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public static BookDTO CreateBook(int bookID)
        {
            return new BookDTO
            {
                ID = 1,
                Title = String.Format("Book {0}", bookID),
                Author = String.Format("Author {0}", bookID),
                PublishDate = new DateTime(2000 + bookID, 1, 1),
                ISBN = String.Format("ISBN {0}", bookID),
            };
        }

        public static ReviewDTO CreateReview(int userID, int bookID, int rating)
        {
            return new ReviewDTO
            {
                User = CreateUser(userID),
                Book = CreateBook(bookID),
                Rating = rating
            };
        }

        public static ReviewViewModel CreateReviewViewModel(int rating)
        {
            return new ReviewViewModel
            {
                Rating = rating
            };
        }

        public static PatchReviewViewModel CreatePatchReviewViewModel(int? rating)
        {
            return new PatchReviewViewModel
            {
                Rating = rating
            };
        }

        public static BookReviewDTO CreateBookReview(int userID, int rating)
        {
            return new BookReviewDTO
            {
                User = CreateUser(userID),
                Rating = rating
            };
        }

        public static UserReviewDTO CreateUserReview(int bookID, int rating)
        {
            return new UserReviewDTO
            {
                Book = CreateBook(bookID),
                Rating = rating
            };
        }

        public static BookReviewsDTO CreateBookReviewsForBook(int bookID, int n)
        {
            var reviews = new List<BookReviewDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var review = CreateBookReview(ID, rand.Next(0, 6));

                reviews.Add(review);
            }

            var book = CreateBook(bookID);

            return new BookReviewsDTO
            {
                Book = book,
                Reviews = reviews
            };
        }

        public static IEnumerable<BookReviewDTO> CreateBookReviews(int n)
        {
            var bookReviews = new List<BookReviewDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var bookReview = CreateBookReview(ID, rand.Next(1, 10));

                bookReviews.Add(bookReview);
            }

            return bookReviews;
        }

        public static IEnumerable<BookReviewsDTO> CreateBookReviewsForBook(int n)
        {
            var bookReviews = new List<BookReviewsDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var bookReview = CreateBookReviewsForBook(ID, rand.Next(1, 10));

                bookReviews.Add(bookReview);
            }

            return bookReviews;
        }

        public static IEnumerable<UserReviewDTO> CreateUserReviews(int n)
        {
            var userReviews = new List<UserReviewDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var userReview = CreateUserReview(ID, rand.Next(1, 10));

                userReviews.Add(userReview);
            }

            return userReviews;
        }

        public static Envelope<BookReviewDTO> CreateBookReviewEnvelope(IEnumerable<BookReviewDTO> bookReviews, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = bookReviews.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var selectedBookReviews = bookReviews.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<BookReviewDTO>
            {
                Items = selectedBookReviews,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = selectedBookReviews.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public static Envelope<BookReviewsDTO> CreateBookReviewsEnvelope(IEnumerable<BookReviewsDTO> bookReviews, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = bookReviews.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var selectedBookReviews = bookReviews.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<BookReviewsDTO>
            {
                Items = selectedBookReviews,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = selectedBookReviews.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public static Envelope<UserReviewDTO> CreateUserReviewEnvelope(IEnumerable<UserReviewDTO> userReviews, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = userReviews.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var selectedUserReviews = userReviews.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<UserReviewDTO>
            {
                Items = selectedUserReviews,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = selectedUserReviews.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }
    }
}
