using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models;

namespace LibraryAPI.Interfaces.Repositories
{
    /// <summary>
    /// An interface detailing the user repository
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of UserDTO</returns>
        Envelope<UserDTO> GetUsers(int pageNumber, int? pageMaxSize);

        /// <summary>
        /// Get a user by user ID
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="loanHistory">Whether or not the loan history should be populated</param>
        /// <para />
        /// <returns>An UserDTO</returns>
        UserDTO GetUserByID(int userID);

        /// <summary>
        /// Add a user
        /// </summary>
        /// <param name="user">The UserViewModel containing the user information</param>
        /// <para />
        /// <returns>The newly created user as a UserDTO</returns>
        UserDTO AddUser(UserViewModel user);

        /// <summary>
        /// Update some (or all) of a user's information
        /// </summary>
        /// <param name="userID">The user ID</param>
        /// <para />
        /// <param name="user">The UserViewModel containing updated user information</param>
        /// <para />
        /// <returns>void</returns>
        void UpdateUser(int userID, UserViewModel user);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userID">The user ID</param>
        /// <para />
        /// <returns>void</returns>
        void DeleteUserByID(int userID);
    }
}