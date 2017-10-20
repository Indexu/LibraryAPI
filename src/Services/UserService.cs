using System;
using System.Collections.Generic;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Interfaces.Services;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using AutoMapper;

namespace LibraryAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ILoanRepository loanRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, ILoanRepository loanRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.loanRepository = loanRepository;
            this.mapper = mapper;
        }

        public Envelope<UserDTO> GetUsers(int pageNumber, int? pageMaxSize)
        {
            try
            {
                return userRepository.GetUsers(pageNumber, pageMaxSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserDetailsDTO GetUserByID(int userID, int pageNumber, int? pageMaxSize)
        {
            try
            {
                var user = userRepository.GetUserByID(userID);

                var details = mapper.Map<UserDTO, UserDetailsDTO>(user);

                details.LoanHistory = loanRepository.GetLoansByUserID(userID, false, pageNumber, pageMaxSize);

                return details;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddUser(UserViewModel user)
        {
            try
            {
                return userRepository.AddUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReplaceUser(int userID, UserViewModel user)
        {
            try
            {
                userRepository.UpdateUser(userID, user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUser(int userID, PatchUserViewModel user)
        {
            try
            {
                var oldUser = userRepository.GetUserByID(userID);

                MergeUsersForPatch(user, oldUser);

                var newUser = mapper.Map<PatchUserViewModel, UserViewModel>(user);

                userRepository.UpdateUser(userID, newUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUserByID(int userID)
        {
            try
            {
                userRepository.DeleteUserByID(userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MergeUsersForPatch(PatchUserViewModel user, UserDTO oldUser)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                user.Name = oldUser.Name;
            }

            if (string.IsNullOrEmpty(user.Address))
            {
                user.Address = oldUser.Address;
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                user.Email = oldUser.Email;
            }
        }
    }
}
