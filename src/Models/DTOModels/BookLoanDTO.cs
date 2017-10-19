using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a loan of a specific book
    /// </summary>
    public class BookLoanDTO
    {
        /// <summary>
        /// The user
        /// </summary>
        /// <value>
        /// The User property is the UserDTO value of the loan
        /// </value>
        public UserDTO User { get; set; }

        /// <summary>
        /// When the book was loaned
        /// </summary>
        /// <value>
        /// The LoanDate property is the DateTime value of when the book was loaned
        /// <para />
        /// Example: 2017-09-22
        /// </value>
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// When the book was returned, if it has been returned
        /// </summary>
        /// <value>
        /// The ReturnDate property is the DateTime value of when the return date of the loaned book
        /// <para />
        /// Example: 2017-09-25
        /// </value>
        public DateTime? ReturnDate { get; set; }
    }
}