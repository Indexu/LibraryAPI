using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a review for a book
    /// </summary>
    public class BookReviewDTO
    {
        /// <summary>
        /// The user who reviewed
        /// </summary>
        /// <value>
        /// The User property is the UserDTO value of the user who reviewed the book
        /// </value>
        public UserDTO User { get; set; }

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