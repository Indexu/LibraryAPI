using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for viewing a single user, more data
    /// </summary>
    public class UserDetailsDTO
    {
        /// <summary>
        /// The ID of the user
        /// </summary>
        /// <value>
        /// The ID property is the integer value of the user ID in the database
        /// <para />
        /// Example: 3
        /// </value>
        public int ID { get; set; }

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
        public string Email { get; set; }

        /// <summary>
        /// The loan history of the user
        /// </summary>
        /// <value>
        /// The LoanHistory property is the Envelope value of the books the user has loaned
        /// </value>
        public Envelope<UserLoanDTO> LoanHistory { get; set; }
    }
}
