using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.ViewModels
{
    /// <summary>
    /// A POCO class for recieving user input for a Loan PATCH
    /// </summary>
    public class PatchLoanViewModel
    {
        /// <summary>
        /// When the book was loaned
        /// </summary>
        /// <value>
        /// The LoanDate property is the DateTime value of when the book was loaned
        /// <para />
        /// Example: 2017-09-25
        /// </value>
        public DateTime? LoanDate { get; set; }

        /// <summary>
        /// When the book was returned, if it has been returned
        /// </summary>
        /// <value>
        /// The ReturnDate property is the DateTime value of when the return date of the loaned book
        /// <para />
        /// Example: 2017-10-11
        /// </value>
        public DateTime? ReturnDate { get; set; }
    }
}
