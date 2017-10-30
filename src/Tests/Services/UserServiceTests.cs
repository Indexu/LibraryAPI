using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LibraryAPI.Services;
using LibraryAPI.Interfaces.Services;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Tests.MockUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace LibraryAPI.Tests.Services
{
    /// <summary>
    /// The unit tests for the UserService
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [TestClass]
    public class UserServiceTests
    {
        /* 
         * ===================
         *      GetUsers
         * ===================
         */

        [TestMethod]
        public void GetUsersBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;

            var users = MockDataGenerator.CreateUsers(10);
            var envelope = MockDataGenerator.CreateUsersEnvelope(users, pageNumber, pageSize);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.GetUsers(pageNumber, pageSize)).Returns(envelope);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedUsers = service.GetUsers(pageNumber, pageSize);

            // Assert
            mockUserRepo.Verify(f => f.GetUsers(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedUsers);
            Assert.AreEqual(returnedUsers.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedUsers.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedUsers.Paging.PageCount, 1);
            Assert.AreEqual(returnedUsers.Paging.TotalNumberOfItems, 10);
            Assert.AreEqual(returnedUsers.Items.First().ID, users.First().ID);
            Assert.AreEqual(returnedUsers.Items.Last().ID, users.Last().ID);
        }

        [TestMethod]
        public void GetUsers2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;

            var users = MockDataGenerator.CreateUsers(10);
            var envelope = MockDataGenerator.CreateUsersEnvelope(users, pageNumber, pageSize);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.GetUsers(pageNumber, pageSize)).Returns(envelope);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedUsers = service.GetUsers(pageNumber, pageSize);

            // Assert
            mockUserRepo.Verify(f => f.GetUsers(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedUsers);
            Assert.AreEqual(returnedUsers.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedUsers.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedUsers.Paging.PageCount, 2);
            Assert.AreEqual(returnedUsers.Paging.TotalNumberOfItems, 10);
            Assert.AreEqual(returnedUsers.Items.First().ID, users.First().ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetUsersException()
        {
            // Arrange:
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.GetUsers(It.IsAny<int>(), It.IsAny<int?>())).Throws(new Exception());

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.GetUsers(pageNumber, pageSize);
        }

        /* 
         * ===================
         *    GetUserByID
         * ===================
         */

        [TestMethod]
        public void GetUserByIDBasic()
        {
            // Arrange
            var userID = 1;
            var numberOfLoans = 10;
            var pageNumber = 1;
            int? pageSize = null;

            var user = MockDataGenerator.CreateUser(userID);
            var userDetails = MockDataGenerator.CreateUserDetailsBasic(userID);
            var loans = MockDataGenerator.CreateUserLoans(numberOfLoans);
            var loansEnvelope = MockDataGenerator.CreateUsersLoansEnvelope(loans, pageNumber, pageSize);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.GetUserByID(userID)).Returns(user);
            mockLoanRepo.Setup(f => f.GetLoansByUserID(userID, false, pageNumber, pageSize)).Returns(loansEnvelope);
            mockMapper.Setup(x => x.Map<UserDTO, UserDetailsDTO>(It.IsAny<UserDTO>())).Returns(userDetails);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedUser = service.GetUserByID(userID, pageNumber, pageSize);

            // Assert
            mockUserRepo.Verify(f => f.GetUserByID(userID), Times.Once());
            mockMapper.Verify(f => f.Map<UserDTO, UserDetailsDTO>(It.IsAny<UserDTO>()), Times.Once());
            mockLoanRepo.Verify(f => f.GetLoansByUserID(userID, false, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual(returnedUser.LoanHistory.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedUser.LoanHistory.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedUser.LoanHistory.Paging.PageCount, 1);
            Assert.AreEqual(returnedUser.LoanHistory.Paging.TotalNumberOfItems, numberOfLoans);
            Assert.AreEqual(user.ID, userID);
        }

        [TestMethod]
        public void GetUserByID2Pages()
        {
            // Arrange
            var userID = 1;
            var numberOfLoans = 10;
            var pageNumber = 1;
            int? pageSize = 5;

            var user = MockDataGenerator.CreateUser(userID);
            var userDetails = MockDataGenerator.CreateUserDetailsBasic(userID);
            var loans = MockDataGenerator.CreateUserLoans(numberOfLoans);
            var loansEnvelope = MockDataGenerator.CreateUsersLoansEnvelope(loans, pageNumber, pageSize);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.GetUserByID(userID)).Returns(user);
            mockLoanRepo.Setup(f => f.GetLoansByUserID(userID, false, pageNumber, pageSize)).Returns(loansEnvelope);
            mockMapper.Setup(x => x.Map<UserDTO, UserDetailsDTO>(It.IsAny<UserDTO>())).Returns(userDetails);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedUser = service.GetUserByID(userID, pageNumber, pageSize);

            // Assert
            mockUserRepo.Verify(f => f.GetUserByID(userID), Times.Once());
            mockMapper.Verify(f => f.Map<UserDTO, UserDetailsDTO>(It.IsAny<UserDTO>()), Times.Once());
            mockLoanRepo.Verify(f => f.GetLoansByUserID(userID, false, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual(returnedUser.LoanHistory.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedUser.LoanHistory.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedUser.LoanHistory.Paging.PageCount, 2);
            Assert.AreEqual(returnedUser.LoanHistory.Paging.TotalNumberOfItems, numberOfLoans);
            Assert.AreEqual(user.ID, userID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetUserByIDException()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.GetUserByID(userID)).Throws(new Exception());

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.GetUserByID(userID, pageNumber, pageSize);
        }

        /* 
         * ===================
         *      AddUser
         * ===================
         */

        [TestMethod]
        public void AddUser()
        {
            // Arrange
            var userID = 1;
            var newUser = MockDataGenerator.CreateUserViewModel(userID);

            var expectedUser = MockDataGenerator.CreateUser(1);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.AddUser(newUser)).Returns(expectedUser);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedUser = service.AddUser(newUser);

            // Assert
            mockUserRepo.Verify(f => f.AddUser(newUser), Times.Once());
            Assert.IsNotNull(returnedUser);
            Assert.AreEqual(returnedUser.ID, userID);
            Assert.AreEqual(returnedUser.Name, newUser.Name);
            Assert.AreEqual(returnedUser.Email, newUser.Email);
            Assert.AreEqual(returnedUser.Address, newUser.Address);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddUserException()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.AddUser(It.IsAny<UserViewModel>())).Throws(new Exception());

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedUser = service.AddUser(new UserViewModel());
        }

        /* 
         * ===================
         *    Replace user
         * ===================
         */

        [TestMethod]
        public void ReplaceUser()
        {
            // Arrange
            var userID = 1;

            var newUser = MockDataGenerator.CreateUserViewModel(userID);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.UpdateUser(userID, newUser));

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.ReplaceUser(userID, newUser);

            // Assert
            mockUserRepo.Verify(f => f.UpdateUser(userID, newUser), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReplaceUserException()
        {
            // Arrange
            var userID = 1;

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.UpdateUser(It.IsAny<int>(), It.IsAny<UserViewModel>())).Throws(new Exception());

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.ReplaceUser(userID, new UserViewModel());
        }

        /* 
         * ===================
         *    Update user
         * ===================
         */

        [TestMethod]
        public void UpdateUser()
        {
            // Arrange
            var userID = 1;

            var newUser = MockDataGenerator.CreatePatchUserViewModel(2);

            var existingUser = MockDataGenerator.CreateUserViewModel(userID);

            var user = MockDataGenerator.CreateUser(userID);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.UpdateUser(userID, It.IsAny<UserViewModel>()));
            mockUserRepo.Setup(f => f.GetUserByID(userID)).Returns(user);
            mockMapper.Setup(x => x.Map<PatchUserViewModel, UserViewModel>(It.IsAny<PatchUserViewModel>())).Returns(existingUser);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.UpdateUser(userID, newUser);

            // Assert
            mockUserRepo.Verify(f => f.UpdateUser(userID, It.IsAny<UserViewModel>()), Times.Once());
            mockUserRepo.Verify(f => f.GetUserByID(userID), Times.Once());
            mockMapper.Verify(f => f.Map<PatchUserViewModel, UserViewModel>(It.IsAny<PatchUserViewModel>()), Times.Once());
        }

        [TestMethod]
        public void UpdateUserEmpty()
        {
            // Arrange
            var userID = 1;

            var newUser = new PatchUserViewModel();

            var existingUser = MockDataGenerator.CreateUserViewModel(userID);

            var user = MockDataGenerator.CreateUser(userID);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.UpdateUser(userID, It.IsAny<UserViewModel>()));
            mockUserRepo.Setup(f => f.GetUserByID(userID)).Returns(user);
            mockMapper.Setup(x => x.Map<PatchUserViewModel, UserViewModel>(It.IsAny<PatchUserViewModel>())).Returns(existingUser);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.UpdateUser(userID, newUser);

            // Assert
            mockUserRepo.Verify(f => f.UpdateUser(userID, It.IsAny<UserViewModel>()), Times.Once());
            mockUserRepo.Verify(f => f.GetUserByID(userID), Times.Once());
            mockMapper.Verify(f => f.Map<PatchUserViewModel, UserViewModel>(It.IsAny<PatchUserViewModel>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateUserException()
        {
            // Arrange
            var userID = 1;

            var newUser = new PatchUserViewModel();

            var existingUser = MockDataGenerator.CreateUserViewModel(userID);

            var user = MockDataGenerator.CreateUser(userID);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.UpdateUser(userID, It.IsAny<UserViewModel>())).Throws(new Exception());
            mockUserRepo.Setup(f => f.GetUserByID(userID)).Returns(user);
            mockMapper.Setup(x => x.Map<PatchUserViewModel, UserViewModel>(It.IsAny<PatchUserViewModel>())).Returns(existingUser);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.UpdateUser(userID, newUser);
        }

        /* 
         * ===================
         *    Delete user
         * ===================
         */

        [TestMethod]
        public void DeleteUser()
        {
            // Arrange
            var userID = 1;

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.DeleteUserByID(userID));

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.DeleteUserByID(userID);

            // Assert
            mockUserRepo.Verify(f => f.DeleteUserByID(userID), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteUserException()
        {
            // Arrange
            var userID = 1;

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.DeleteUserByID(It.IsAny<int>())).Throws(new Exception());

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.DeleteUserByID(userID);
        }
    }
}
