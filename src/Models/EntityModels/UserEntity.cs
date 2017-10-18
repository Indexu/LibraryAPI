using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.EntityModels
{
    /// <summary>
    /// An entity class for a user entry in the database
    /// </summary>
    public class UserEntity
    {
        /// <summary>
        /// The ID of the user
        /// </summary>
        /// <value>
        /// The ID property is the integer value of the user ID in the database
        /// <para />
        /// Example: 3
        /// </value>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// The full name of the user
        /// </summary>
        /// <value>
        /// The Name property is the string value of the user's name
        /// <para />
        /// Example: John Doe
        /// </value>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The home address of the user
        /// </summary>
        /// <value>
        /// The Address property is the string value of the user's home address
        /// <para />
        /// Example: 420 Evergreen Terrace
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        /// <value>
        /// The Email property is the string value of the user's email address
        /// <para />
        /// Example: johndoe@email.com
        /// </value>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
