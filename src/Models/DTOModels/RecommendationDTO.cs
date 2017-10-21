using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a book recommendation
    /// </summary>
    public class RecommendationDTO
    {
        /// <summary>
        /// The recommended book
        /// </summary>
        /// <value>
        /// The Book property is the BookDTO value of the recommended book
        /// </value>
        public BookDTO Book { get; set; }

        /// <summary>
        /// The rating of the book
        /// </summary>
        /// <value>
        /// The AverageRating property is the double value of the book's average rating in the range 0.0 to 5.0, inclusive
        /// <para />
        /// Example: 4.3
        /// </value>
        public double AverageRating { get; set; }
    }
}