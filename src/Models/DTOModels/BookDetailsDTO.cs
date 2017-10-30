using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for the details of a book
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class BookDetailsDTO
    {
        /// <summary>
        /// The ID of the book
        /// </summary>
        /// <value>
        /// The ID property is the integer value of the book ID in the database
        /// <para />
        /// Example: 3
        /// </value>
        public int ID { get; set; }

        /// <summary>
        /// The title of the book
        /// </summary>
        /// <value>
        /// The Title property is the string value of the book's title
        /// <para />
        /// Example: The Lord Of The Rings
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// The name of the book's author
        /// </summary>
        /// <value>
        /// The Author property is the string value of the book's author
        /// <para />
        /// Example: J.R.R. Tolkien
        /// </value>
        public string Author { get; set; }

        /// <summary>
        /// When the book was published
        /// </summary>
        /// <value>
        /// The PublishDate property is the DateTime value of the book's publish date
        /// <para />
        /// Example: johndoe@email.com
        /// </value>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// The book's ISBN
        /// </summary>
        /// <value>
        /// The ISBN property is the string value of the book's ISBN
        /// <para />
        /// Example: 428449649-2
        /// </value>
        public string ISBN { get; set; }

        /// <summary>
        /// The loan history of the book
        /// </summary>
        /// <value>
        /// The LoanHistory property is the Envelope value of the loans the book has been a part of
        /// </value>
        public Envelope<BookLoanDTO> LoanHistory { get; set; }
    }
}
