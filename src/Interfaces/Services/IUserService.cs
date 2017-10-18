using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models;

namespace LibraryAPI.Interfaces.Services
{
    /// <summary>
    /// An interface detailing the operations of the user service
    /// </summary>
    public interface IUserService
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
        /// <returns>An UserDetailsDTO</returns>
        UserDetailsDTO GetUserByID(int userID);

        /// <summary>
        /// Add a user
        /// </summary>
        /// <param name="user">The UserViewModel containing the user information</param>
        /// <para />
        /// <returns>An integer representing the ID of the added user</returns>
        int AddUser(UserViewModel user);

        /// <summary>
        /// Replace a user's information
        /// </summary>
        /// <param name="userID">The user ID</param>
        /// <para />
        /// <param name="user">The UserViewModel containing the new information</param>
        /// <para />
        /// <returns>void</returns>
        void ReplaceUser(int userID, UserViewModel user);

        /// <summary>
        /// Update some (or all) of a user's information
        /// </summary>
        /// <param name="userID">The user ID</param>
        /// <para />
        /// <param name="user">The PatchUserViewModel containing updated user information</param>
        /// <para />
        /// <returns>void</returns>
        void UpdateUser(int userID, PatchUserViewModel user);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userID">The user ID</param>
        /// <para />
        /// <returns>void</returns>
        void DeleteUserByID(int userID);
    }
}
