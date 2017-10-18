using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for an ongoing loan belonging to a specific user
    /// </summary>
    public class UserLoanDTO
    {
        /// <summary>
        /// The loaned book
        /// </summary>
        /// <value>
        /// The Book property is the BookDTO value of the loaned book
        /// </value>
        public BookDTO Book { get; set; }

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