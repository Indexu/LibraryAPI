using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models;

namespace LibraryAPI.Interfaces.Repositories
{
    /// <summary>
    /// An interface detailing the review repository
    /// </summary>
    public interface IReviewRepository
    {
        /// <summary>
        /// Get all book reviews
        /// </summary>
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of BooksReviewDTO</returns>
        Envelope<BookReviewsDTO> GetBookReviews(int pageNumber, int? pageMaxSize);

        /// <summary>
        /// Get all reviews by user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of UserReviewDTO</returns>
        Envelope<UserReviewDTO> GetReviewsByUserID(int userID, int pageNumber, int? pageMaxSize);

        /// <summary>
        /// Get all reviews for a book
        /// </summary>
        /// <param name="bookID">The ID of the book</param>
        /// <para />
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of BookReviewDTO</returns>
        Envelope<BookReviewDTO> GetReviewsByBookID(int bookID, int pageNumber, int? pageMaxSize);

        /// <summary>
        /// Get a specific review
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="bookID">The ID of the book</param>
        /// <para />
        /// <returns>A ReviewDTO</returns>
        ReviewDTO GetReview(int userID, int bookID);

        /// <summary>
        /// Add a review
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="review">The ReviewViewModel containing the review information</param>
        /// <para />
        /// <returns>A ReviewDTO for the created review</returns>
        ReviewDTO AddReview(int userID, int bookID, ReviewViewModel review);

        /// <summary>
        /// Update some (or all) of a review's information
        /// </summary>
        /// <param name="userID">The user ID</param>
        /// <para />
        /// <param name="bookID">The book ID</param>
        /// <para />
        /// <param name="review">The PatchReviewViewModel containing the updated review information</param>
        /// <para />
        /// <returns>void</returns>
        void UpdateReview(int userID, int bookID, PatchReviewViewModel review);

        /// <summary>
        /// Delete a review
        /// </summary>
        /// <param name="userID">The user ID</param>
        /// <para />
        /// <param name="bookID">The book ID</param>
        /// <para />
        /// <returns>void</returns>
        void DeleteReview(int userID, int bookID);

        /// <summary>
        /// Get recommendations for a user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of RecommendationDTO</returns>
        Envelope<RecommendationDTO> GetRecommendationsByUserID(int userID, int pageNumber, int? pageMaxSize);
    }
}