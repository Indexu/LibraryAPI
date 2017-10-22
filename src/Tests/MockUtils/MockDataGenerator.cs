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

        public static List<BookDTO> CreateBooks(int n)
        {
            var books = new List<BookDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;
                var book = CreateBook(ID);

                books.Add(book);
            }

            return books;
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

        public static UserLoanDTO CreateUserLoan(int bookID, DateTime loanDate, DateTime? returnDate)
        {
            return new UserLoanDTO
            {
                Book = CreateBook(bookID),
                LoanDate = loanDate,
                ReturnDate = returnDate
            };
        }

        public static List<UserLoanDTO> CreateUserLoans(int n)
        {
            var loans = new List<UserLoanDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var loan = CreateUserLoan(ID, new DateTime(2010 + ID, 1, 1), (i % 2 == 0 ? (DateTime?)new DateTime(2010 + ID, 3, 1) : null));

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

        public static BookLoanDTO CreateBookLoan(int userID, DateTime loanDate, DateTime? returnDate)
        {
            return new BookLoanDTO
            {
                User = CreateUser(userID),
                LoanDate = loanDate,
                ReturnDate = returnDate
            };
        }

        public static List<BookLoanDTO> CreateBookLoans(int n)
        {
            var loans = new List<BookLoanDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var loan = CreateBookLoan(ID, new DateTime(2010 + ID, 1, 1), (i % 2 == 0 ? (DateTime?)new DateTime(2010 + ID, 3, 1) : null));

                loans.Add(loan);
            }

            return loans;
        }

        public static Envelope<BookLoanDTO> CreateBookLoansEnvelope(IEnumerable<BookLoanDTO> loans, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = loans.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var users = loans.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<BookLoanDTO>
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

        public static PatchLoanViewModel CreatePatchLoanViewModel(DateTime loanDate, DateTime? returnDate)
        {
            return new PatchLoanViewModel
            {
                LoanDate = loanDate,
                ReturnDate = returnDate
            };
        }

        public static LoanViewModel CreateLoanViewModel(DateTime loanDate, DateTime? returnDate)
        {
            return new LoanViewModel
            {
                LoanDate = loanDate,
                ReturnDate = returnDate
            };
        }

        public static PatchBookViewModel CreatePatchBookViewModel(int bookID)
        {
            return new PatchBookViewModel
            {
                Title = String.Format("Book {0}", bookID),
                Author = String.Format("Author {0}", bookID),
                PublishDate = new DateTime(2000 + bookID, 1, 1),
                ISBN = String.Format("ISBN {0}", bookID),
            };
        }

        public static BookViewModel CreateBookViewModel(int bookID)
        {
            return new BookViewModel
            {
                Title = String.Format("Book {0}", bookID),
                Author = String.Format("Author {0}", bookID),
                PublishDate = new DateTime(2000 + bookID, 1, 1),
                ISBN = String.Format("ISBN {0}", bookID),
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

        public static BookDetailsDTO CreateBasicBookDetails(int bookID)
        {
            return new BookDetailsDTO
            {
                ID = 1,
                Title = String.Format("Book {0}", bookID),
                Author = String.Format("Author {0}", bookID),
                PublishDate = new DateTime(2000 + bookID, 1, 1),
                ISBN = String.Format("ISBN {0}", bookID),
                LoanHistory = null
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

        public static UserReportDTO CreateUserReport(int userID, int n)
        {
            return new UserReportDTO
            {
                User = CreateUser(userID),
                UserLoans = CreateUserLoans(n)
            };
        }

        public static BookReportDTO CreateBookReport(int bookID, int n)
        {
            return new BookReportDTO
            {
                Book = CreateBook(bookID),
                BookLoans = CreateBookLoans(n)
            };
        }

        public static RecommendationDTO CreateRecommendation(int bookID, double? averageRating)
        {
            return new RecommendationDTO
            {
                Book = CreateBook(bookID),
                AverageRating = averageRating
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

        public static IEnumerable<UserReportDTO> CreateUserReports(int n)
        {
            var reports = new List<UserReportDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var report = CreateUserReport(ID, rand.Next(1, 10));

                reports.Add(report);
            }

            return reports;
        }

        public static IEnumerable<BookReportDTO> CreateBookReports(int n)
        {
            var reports = new List<BookReportDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var report = CreateBookReport(ID, rand.Next(1, 10));

                reports.Add(report);
            }

            return reports;
        }

        public static IEnumerable<RecommendationDTO> CreateRecommendations(int n)
        {
            var recommendations = new List<RecommendationDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var recommendation = CreateRecommendation(ID, (rand.NextDouble() * 5));

                recommendations.Add(recommendation);
            }

            return recommendations;
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

        public static Envelope<BookDTO> CreateBooksEnvelope(IEnumerable<BookDTO> books, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = books.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var selectedBooks = books.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<BookDTO>
            {
                Items = selectedBooks,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = selectedBooks.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public static Envelope<UserLoanDTO> CreateUserLoansEnvelope(IEnumerable<UserLoanDTO> userLoans, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = userLoans.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var selectedLoans = userLoans.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<UserLoanDTO>
            {
                Items = selectedLoans,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = selectedLoans.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public static Envelope<UserReportDTO> CreateUserReportsEnvelope(IEnumerable<UserReportDTO> reports, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = reports.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var selectedReports = reports.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<UserReportDTO>
            {
                Items = selectedReports,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = selectedReports.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public static Envelope<BookReportDTO> CreateBookReportsEnvelope(IEnumerable<BookReportDTO> reports, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = reports.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var selectedReports = reports.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<BookReportDTO>
            {
                Items = selectedReports,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = selectedReports.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public static Envelope<RecommendationDTO> CreateRecommendationsEnvelope(IEnumerable<RecommendationDTO> recommendations, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = recommendations.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var selectedRecommendations = recommendations.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<RecommendationDTO>
            {
                Items = selectedRecommendations,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = selectedRecommendations.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }
    }
}
