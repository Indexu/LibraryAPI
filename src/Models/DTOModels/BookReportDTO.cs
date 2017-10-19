using System.Collections.Generic;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a book report entry
    /// </summary>
    public class BookReportDTO
    {
        /// <summary>
        /// The book
        /// </summary>
        /// <value>
        /// The Book property is the BookDTO value of the report entry
        /// </value>
        public BookDTO Book { get; set; }

        /// <summary>
        /// What users have the book loaned
        /// </summary>
        /// <value>
        /// The UserLoans property is the UserLoanDTO collection value of what books the user has
        /// </value>
        public IEnumerable<BookLoanDTO> BookLoans { get; set; }
    }
}