using System;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a loan
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class LoanDTO
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
        /// The user who got the book loaned
        /// </summary>
        /// <value>
        /// The User property is the UserDTO value of the user who got the book loaned
        /// </value>
        public UserDTO User { get; set; }

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
