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
        /// The phone number of the user
        /// </summary>
        /// <value>
        /// The Phone property is the string value of the user's phone number
        /// <para />
        /// Example: 5812345
        /// </value>
        public string Phone { get; set; }

        /// <summary>
        /// The books the user has right now
        /// </summary>
        /// <value>
        /// The CurrentLoans property is the IEnumerable value of the books the user has right now
        /// </value>
        public IEnumerable<UserLoanDTO> CurrentLoans { get; set; }
    }
}
