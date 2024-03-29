using System.Collections.Generic;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a book report entity
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
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