using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models.EntityModels
{
    /// <summary>
    /// An entity class for a loan entry in the database
    /// </summary>
    public class LoanEntity
    {
        /// <summary>
        /// The ID of the loan
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
        /// Example: 24
        /// </value>
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// The ID of the book
        /// </summary>
        /// <value>
        /// The BookID property is the integer value of what book is loaned
        /// <para />
        /// Example: 55
        /// </value>
        [Required]
        public int BookID { get; set; }

        /// <summary>
        /// When the book was loaned
        /// </summary>
        /// <value>
        /// The LoanDate property is the DateTime value of when the book was loaned
        /// <para />
        /// Example: 2017-09-25
        /// </value>
        [Required]
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// When the book was returned, if it has been returned
        /// </summary>
        /// <value>
        /// The ReturnDate property is the DateTime value of when the return date of the loaned book
        /// <para />
        /// Example: 2017-10-11
        /// </value>
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// The user who got the book loaned
        /// </summary>
        /// <value>
        /// The User property is the UserEntity value of the user who got the book loaned
        /// </value>
        [ForeignKey("UserID")]
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// The loaned book
        /// </summary>
        /// <value>
        /// The Book property is the BookEntity value of the loaned book
        /// </value>
        [ForeignKey("BookID")]
        public virtual BookEntity Book { get; set; }
    }
}
