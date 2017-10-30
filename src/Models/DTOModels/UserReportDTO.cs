using System.Collections.Generic;

namespace LibraryAPI.Models.DTOModels
{
    /// <summary>
    /// A DTO (data transfer object) for a user report entity
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
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