﻿using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models;

namespace LibraryAPI.Interfaces.Services
{
    /// <summary>
    /// An interface detailing the user service
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
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
        /// <param name="pageNumber">The page number of the paging of loan history</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page of loan history</param>
        /// <para />
        /// <returns>An UserDetailsDTO</returns>
        UserDetailsDTO GetUserByID(int userID, int pageNumber, int? pageMaxSize);

        /// <summary>
        /// Add a user
        /// </summary>
        /// <param name="user">The UserViewModel containing the user information</param>
        /// <para />
        /// <returns>The newly created user as a UserDTO</returns>
        UserDTO AddUser(UserViewModel user);

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
