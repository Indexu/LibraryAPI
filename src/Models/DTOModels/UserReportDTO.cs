using System.Collections.Generic;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a user report entry
    /// </summary>
    public class UserReportDTO
    {
        /// <summary>
        /// The user
        /// </summary>
        /// <value>
        /// The User property is the UserDTO value of the report entry
        /// </value>
        public UserDTO User { get; set; }

        /// <summary>
        /// What loans the user has
        /// </summary>
        /// <value>
        /// The UserLoans property is the UserLoanDTO collection value of what books the user has
        /// </value>
        public IEnumerable<UserLoanDTO> UserLoans { get; set; }
    }
}