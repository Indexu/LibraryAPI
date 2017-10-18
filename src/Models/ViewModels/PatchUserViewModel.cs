using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.ViewModels
{
    /// <summary>
    /// A POCO class for recieving user input for a User PATCH
    /// </summary>
    public class PatchUserViewModel
    {
        /// <summary>
        /// The full name of the user
        /// </summary>
        /// <value>
        /// The Name property is the string value of the user's name
        /// <para />
        /// Example: John Doe
        /// </value>
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
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The phone number of the user
        /// </summary>
        /// <value>
        /// The Phone property is the string value of the user's phone number
        /// <para />
        /// Example: 5812345
        /// </value>
        [RegularExpression("^[0-9]*$")]
        public string Phone { get; set; }
    }
}