using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for an ongoing loan belonging to a specific user
    /// </summary>
    public class UserLoanDTO
    {
        /// <summary>
        /// The ID of the book
        /// </summary>
        /// <value>
        /// The ID property is the integer value of the loan ID in the database
        /// <para />
        /// Example: 3
        /// </value>
        public int ID { get; set; }

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
        /// Example: johndoe@email.com
        /// </value>
        public DateTime LoanDate { get; set; }
    }
}
