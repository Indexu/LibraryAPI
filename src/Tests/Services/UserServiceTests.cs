using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LibraryAPI.Services;
using LibraryAPI.Interfaces.Services;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models;
using LibraryAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace LibraryAPI.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private List<UserDTO> allUsers;
        private Envelope<UserDTO> usersEnvelope;
        private UserDetailsDTO userDetails;

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

            CreateUsers(10);
            CreateUsersEnvelope(pageNumber, pageSize);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.GetUsers(pageNumber, pageSize)).Returns(usersEnvelope);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var users = service.GetUsers(pageNumber, pageSize);

            // Assert
            mockUserRepo.Verify(f => f.GetUsers(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(users);
            Assert.AreEqual(users.Paging.PageNumber, pageNumber);
            Assert.AreEqual(users.Paging.PageMaxSize, 50);
            Assert.AreEqual(users.Paging.PageCount, 1);
            Assert.AreEqual(users.Paging.TotalNumberOfItems, 10);
            Assert.AreEqual(users.Items.First().ID, allUsers.First().ID);
            Assert.AreEqual(users.Items.Last().ID, allUsers.Last().ID);
        }

        [TestMethod]
        public void GetUsers2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;

            CreateUsers(10);
            CreateUsersEnvelope(pageNumber, pageSize);

            var mockUserRepo = new Mock<IUserRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserRepo.Setup(f => f.GetUsers(pageNumber, pageSize)).Returns(usersEnvelope);

            var service = new UserService(mockUserRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var users = service.GetUsers(pageNumber, pageSize);

            // Assert
            mockUserRepo.Verify(f => f.GetUsers(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(users);
            Assert.AreEqual(users.Paging.PageNumber, pageNumber);
            Assert.AreEqual(users.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(users.Paging.PageCount, 2);
            Assert.AreEqual(users.Paging.TotalNumberOfItems, 10);
            Assert.AreEqual(users.Items.First().ID, allUsers.First().ID);
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

            var user = CreateUser(userID);
            var userDetails = CreateUserDetailsBasic(userID);
            var loans = CreateUserLoans(numberOfLoans);
            var loansEnvelope = CreateUsersLoansEnvelope(loans, pageNumber, pageSize);

            CreateUserDetails(userID, numberOfLoans, pageNumber, pageSize);

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

            var user = CreateUser(userID);
            var userDetails = CreateUserDetailsBasic(userID);
            var loans = CreateUserLoans(numberOfLoans);
            var loansEnvelope = CreateUsersLoansEnvelope(loans, pageNumber, pageSize);

            CreateUserDetails(userID, numberOfLoans, pageNumber, pageSize);

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
         *    GetUserByID
         * ===================
         */

        [TestMethod]
        public void AddUser()
        {
            // Arrange
            var newUser = new UserViewModel
            {
                Name = "User 1",
                Address = "1 Main Street",
                Email = "user@user1.com"
            };

            var expectedUser = CreateUser(1);

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
            Assert.AreEqual(returnedUser.ID, 1);
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

            var newUser = new UserViewModel
            {
                Name = "User 1",
                Address = "1 Main Street",
                Email = "user@user1.com"
            };

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

            var newUser = new PatchUserViewModel
            {
                Name = "User 2",
                Address = "2 Main Street",
                Email = "user@user2.com"
            };

            var existingUser = new UserViewModel
            {
                Name = "User 1",
                Address = "1 Main Street",
                Email = "user@user1.com"
            };

            var user = CreateUser(userID);

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

            var existingUser = new UserViewModel
            {
                Name = "User 1",
                Address = "1 Main Street",
                Email = "user@user1.com"
            };

            var user = CreateUser(userID);

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

            var newUser = new PatchUserViewModel
            {
                Name = "User 2",
                Address = "2 Main Street",
                Email = "user@user2.com"
            };

            var existingUser = new UserViewModel
            {
                Name = "User 1",
                Address = "1 Main Street",
                Email = "user@user1.com"
            };

            var user = CreateUser(userID);

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

        /* 
        * ============================
        *       Helper functions
        * ============================
        */

        private void CreateUsers(int n)
        {
            allUsers = new List<UserDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;
                var user = CreateUser(ID);

                allUsers.Add(user);
            }
        }

        private void CreateUsersEnvelope(int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = allUsers.Count;
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var users = allUsers.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            usersEnvelope = new Envelope<UserDTO>
            {
                Items = users,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = users.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        private UserDTO CreateUser(int userID)
        {
            return new UserDTO
            {
                ID = userID,
                Name = String.Format("User {0}", userID),
                Email = String.Format("user@user{0}.com", userID),
                Address = String.Format("{0} Main Street", userID)
            };
        }

        private UserDetailsDTO CreateUserDetailsBasic(int userID)
        {
            return new UserDetailsDTO
            {
                ID = userID,
                Name = String.Format("User {0}", userID),
                Email = String.Format("user@user{0}.com", userID),
                Address = String.Format("{0} Main Street", userID)
            };
        }

        private void CreateUserDetails(int userID, int numberOfLoans, int pageNumber, int? pageSize)
        {
            userDetails = new UserDetailsDTO
            {
                ID = userID,
                Name = String.Format("User {0}", userID),
                Email = String.Format("user@user{0}.com", userID),
                Address = String.Format("{0} Main Street", userID),
            };

            var loans = CreateUserLoans(numberOfLoans);
            var envelope = CreateUsersLoansEnvelope(loans, pageNumber, pageSize);

            userDetails.LoanHistory = envelope;
        }

        private List<UserLoanDTO> CreateUserLoans(int n)
        {
            var loans = new List<UserLoanDTO>();

            for (int i = 0; i < n; i++)
            {
                var ID = i + 1;

                var loan = new UserLoanDTO
                {
                    Book = new BookDTO
                    {
                        ID = 1,
                        Title = String.Format("Book {0}", ID),
                        Author = String.Format("Author {0}", ID),
                        PublishDate = new DateTime(2000 + ID, 1, 1),
                        ISBN = String.Format("ISBN {0}", ID),
                    },
                    LoanDate = new DateTime(2010 + ID, 1, 1),
                    ReturnDate = (i % 2 == 0 ? (DateTime?)new DateTime(2010 + ID, 3, 1) : null)
                };

                loans.Add(loan);
            }

            return loans;
        }

        private Envelope<UserLoanDTO> CreateUsersLoansEnvelope(List<UserLoanDTO> loans, int pageNumber, int? pageSize)
        {
            var maxSize = (pageSize.HasValue ? pageSize.Value : 50);
            var totalNumberOfItems = loans.Count;
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var users = loans.Skip((pageNumber - 1) * maxSize).Take(maxSize);

            return new Envelope<UserLoanDTO>
            {
                Items = loans,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = loans.Count(),
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }
    }
}
