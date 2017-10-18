using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models.EntityModels
{
    /// <summary>
    /// An entity class for a rating entry in the database
    /// </summary>
    public class RatingEntity
    {
        /// <summary>
        /// The ID of the rating
        /// </summary>
        /// <value>
        /// The ID property is the integer value of the loan ID in the database
        /// <para />
        /// Example: 3
        /// </value>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// The ID of the user
        /// </summary>
        /// <value>
        /// The UserID property is the integer value of what user has a book loaned
        /// <para />
        /// Example: The Lord Of The Rings
        /// </value>
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// The ID of the book
        /// </summary>
        /// <value>
        /// The BookID property is the integer value of what book is loaned
        /// <para />
        /// Example: J.R.R. Tolkien
        /// </value>
        [Required]
        public int BookID { get; set; }

        /// <summary>
        /// The book's rating
        /// </summary>
        /// <value>
        /// The Rating property is the integer value of the book's rating. A value between 1 and 5, inclusive
        /// <para />
        /// Example: 4
        /// </value>
        [Required]
        public int Rating { get; set; }

        /// <summary>
        /// The user who rated the book
        /// </summary>
        /// <value>
        /// The User property is the UserEntity value of the user who rated the book
        /// </value>
        [ForeignKey("UserID")]
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// The rated book
        /// </summary>
        /// <value>
        /// The Book property is the BookEntity value of the rated book
        /// </value>
        [ForeignKey("BookID")]
        public virtual BookEntity Book { get; set; }
    }
}
