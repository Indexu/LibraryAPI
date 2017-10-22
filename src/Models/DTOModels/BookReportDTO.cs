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
        /// The BookLoans property is the BookLoanDTO collection value of what user have gotten the book loaned
        /// </value>
        public IEnumerable<BookLoanDTO> BookLoans { get; set; }
    }
}