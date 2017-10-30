using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.EntityModels
{
    /// <summary>
    /// An entity class for a book entry in the database
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class BookEntity
    {
        /// <summary>
        /// The ID of the book
        /// </summary>
        /// <value>
        /// The ID property is the integer value of the book ID in the database
        /// <para />
        /// Example: 3
        /// </value>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// The title of the book
        /// </summary>
        /// <value>
        /// The Title property is the string value of the book's title
        /// <para />
        /// Example: The Lord Of The Rings
        /// </value>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The name of the book's author
        /// </summary>
        /// <value>
        /// The Author property is the string value of the book's author
        /// <para />
        /// Example: J.R.R. Tolkien
        /// </value>
        [Required]
        public string Author { get; set; }

        /// <summary>
        /// When the book was published
        /// </summary>
        /// <value>
        /// The PublishDate property is the DateTime value of the book's publish date
        /// <para />
        /// Example: 2017-09-25
        /// </value>
        [Required]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// The book's ISBN
        /// </summary>
        /// <value>
        /// The ISBN property is the string value of the book's ISBN
        /// <para />
        /// Example: 428449649-2
        /// </value>
        [Required]
        public string ISBN { get; set; }
    }
}
