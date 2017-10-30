using System;
using System.Collections.Generic;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for multiple reviews of a book
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class BookReviewsDTO
    {
        /// <summary>
        /// The reviewed book
        /// </summary>
        /// <value>
        /// The Book property is the BookDTO value of the reviewed book
        /// </value>
        public BookDTO Book { get; set; }

        /// <summary>
        /// The reviews
        /// </summary>
        /// <value>
        /// The Reviews property is the IEnumerable value of the book reviews
        /// </value>
        public IEnumerable<BookReviewDTO> Reviews { get; set; }
    }
}