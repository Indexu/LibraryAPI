using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a review by a user
    /// </summary>
    public class UserReviewDTO
    {
        /// <summary>
        /// The reviewed book
        /// </summary>
        /// <value>
        /// The Book property is the BookDTO value of the reviewed book
        /// </value>
        public BookDTO Book { get; set; }

        /// <summary>
        /// The rating of the book
        /// </summary>
        /// <value>
        /// The Rating property is the integer value of the book's rating in the range 0 to 5, inclusive
        /// <para />
        /// Example: 3
        /// </value>
        public int Rating { get; set; }
    }
}