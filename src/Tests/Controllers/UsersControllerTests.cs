using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LibraryAPI.API.Controllers;
using LibraryAPI.Interfaces.Services;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Tests.MockUtils;
using LibraryAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Tests.Services
{
    /// <summary>
    /// The unit tests for the UsersController
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [TestClass]
    public class UsersControllerTests
    {
        /* 
         * ===================
         *     GetUsers
         * ===================
         */

        [TestMethod]
        public void GetUsers()
        {
            // Arrange
            int pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.GetUsers(pageNumber, pageSize));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUsers(pageNumber, pageSize) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.GetUsers(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetUsersReport()
        {
            // Arrange
            int pageNumber = 1;
            int? pageSize = null;
            DateTime? loanDate = new DateTime(2000, 1, 1);
            int? duration = 30;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReportingService.Setup(f => f.GetUsersReport(pageNumber, pageSize, loanDate, duration));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUsers(pageNumber, pageSize, loanDate, duration) as ObjectResult;

            // Assert
            mockReportingService.Verify(f => f.GetUsersReport(pageNumber, pageSize, loanDate, duration), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetUsersException()
        {
            // Arrange
            int pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.GetUsers(pageNumber, pageSize)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUsers(pageNumber, pageSize) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.GetUsers(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     GetUser
         * ===================
         */

        [TestMethod]
        public void GetUser()
        {
            // Arrange
            int userID = 1;
            int pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.GetUserByID(userID, pageNumber, pageSize));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUser(userID, pageNumber, pageSize) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.GetUserByID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetUserNotFound()
        {
            // Arrange
            int userID = 1;
            int pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.GetUserByID(userID, pageNumber, pageSize)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUser(userID, pageNumber, pageSize) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.GetUserByID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void GetUserException()
        {
            // Arrange
            int userID = 1;
            int pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.GetUserByID(userID, pageNumber, pageSize)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUser(userID, pageNumber, pageSize) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.GetUserByID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     AddUser
         * ===================
         */

        [TestMethod]
        public void AddUser()
        {
            // Arrange
            var userID = 1;
            var viewModel = MockDataGenerator.CreateUserViewModel(userID);
            var DTO = MockDataGenerator.CreateUser(userID);

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.AddUser(viewModel)).Returns(DTO);

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddUser(viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.AddUser(viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 201);
        }

        [TestMethod]
        public void AddUserValidation()
        {
            // Arrange
            var userID = 1;
            var viewModel = MockDataGenerator.CreateUserViewModel(userID);

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.AddUser(viewModel));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.AddUser(viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void AddUserAlreadyExists()
        {
            // Arrange
            var userID = 1;
            var viewModel = MockDataGenerator.CreateUserViewModel(userID);

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.AddUser(viewModel)).Throws(new AlreadyExistsException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddUser(viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.AddUser(viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void AddUserException()
        {
            // Arrange
            var userID = 1;
            var viewModel = MockDataGenerator.CreateUserViewModel(userID);
            viewModel.Email = "not a valid email";

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.AddUser(viewModel)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddUser(viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.AddUser(viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *    ReplaceUser
         * ===================
         */

        [TestMethod]
        public void ReplaceUser()
        {
            // Arrange
            var userID = 1;
            var viewModel = MockDataGenerator.CreateUserViewModel(userID);

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.ReplaceUser(userID, viewModel));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceUser(userID, viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.ReplaceUser(userID, viewModel), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReplaceUserValidation()
        {
            // Arrange
            var userID = 1;
            var viewModel = new UserViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.ReplaceUser(userID, viewModel));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.ReplaceUser(userID, viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void ReplaceUserNotFound()
        {
            // Arrange
            var userID = 1;
            var viewModel = new UserViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.ReplaceUser(userID, viewModel)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceUser(userID, viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.ReplaceUser(userID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void ReplaceUserAlreadyExists()
        {
            // Arrange
            var userID = 1;
            var viewModel = new UserViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.ReplaceUser(userID, viewModel)).Throws(new AlreadyExistsException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceUser(userID, viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.ReplaceUser(userID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void ReplaceUserException()
        {
            // Arrange
            var userID = 1;
            var viewModel = new UserViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.ReplaceUser(userID, viewModel)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceUser(userID, viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.ReplaceUser(userID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         * PartiallyUpdateUser
         * ===================
         */

        [TestMethod]
        public void PartiallyUpdateUser()
        {
            // Arrange
            var userID = 1;
            var viewModel = MockDataGenerator.CreatePatchUserViewModel(userID);

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.UpdateUser(userID, viewModel));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.PartiallyUpdateUser(userID, viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.UpdateUser(userID, viewModel), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void PartiallyUpdateUserValidation()
        {
            // Arrange
            var userID = 1;
            var viewModel = new PatchUserViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.UpdateUser(userID, viewModel));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.PartiallyUpdateUser(userID, viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void PartiallyUpdateUserNotFound()
        {
            // Arrange
            var userID = 1;
            var viewModel = new PatchUserViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.UpdateUser(userID, viewModel)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.PartiallyUpdateUser(userID, viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.UpdateUser(userID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void PartiallyUpdateUserAlreadyExists()
        {
            // Arrange
            var userID = 1;
            var viewModel = new PatchUserViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.UpdateUser(userID, viewModel)).Throws(new AlreadyExistsException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.PartiallyUpdateUser(userID, viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.UpdateUser(userID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void PartiallyUpdateUserException()
        {
            // Arrange
            var userID = 1;
            var viewModel = new PatchUserViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.UpdateUser(userID, viewModel)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.PartiallyUpdateUser(userID, viewModel) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.UpdateUser(userID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     DeleteUser
         * ===================
         */

        [TestMethod]
        public void DeleteUser()
        {
            // Arrange
            var userID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.DeleteUserByID(userID));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteUser(userID) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.DeleteUserByID(userID), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteUserNotFound()
        {
            // Arrange
            var userID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.DeleteUserByID(userID)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteUser(userID) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.DeleteUserByID(userID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void DeleteUserException()
        {
            // Arrange
            var userID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockUserService.Setup(f => f.DeleteUserByID(userID)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteUser(userID) as ObjectResult;

            // Assert
            mockUserService.Verify(f => f.DeleteUserByID(userID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *    GetUserBooks
         * ===================
         */

        [TestMethod]
        public void GetUserBooks()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.GetBooksInLoanByUserID(userID, pageNumber, pageSize)).Returns(new Envelope<UserLoanDTO>());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUserBooks(userID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.GetBooksInLoanByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetUserBooksNotFound()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.GetBooksInLoanByUserID(userID, pageNumber, pageSize)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUserBooks(userID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.GetBooksInLoanByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void GetUserBooksException()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.GetBooksInLoanByUserID(userID, pageNumber, pageSize)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUserBooks(userID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.GetBooksInLoanByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     LoanBook
         * ===================
         */

        [TestMethod]
        public void LoanBook()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.AddLoan(userID, bookID));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.LoanBook(userID, bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.AddLoan(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 201);
        }

        [TestMethod]
        public void LoanBookNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.AddLoan(userID, bookID)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.LoanBook(userID, bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.AddLoan(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void LoanBookAlreadyExists()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.AddLoan(userID, bookID)).Throws(new AlreadyExistsException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.LoanBook(userID, bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.AddLoan(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void LoanBookException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.AddLoan(userID, bookID)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.LoanBook(userID, bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.AddLoan(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     ReturnBook
         * ===================
         */

        [TestMethod]
        public void ReturnBook()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReturnBook(userID, bookID));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReturnBook(userID, bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReturnBook(userID, bookID), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReturnBookNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReturnBook(userID, bookID)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReturnBook(userID, bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReturnBook(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void ReturnBookException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReturnBook(userID, bookID)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReturnBook(userID, bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReturnBook(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     ReplaceLoan
         * ===================
         */

        [TestMethod]
        public void ReplaceLoan()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new LoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReplaceLoan(userID, bookID, viewModel));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReplaceLoan(userID, bookID, viewModel), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReplaceLoanValidation()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new LoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.ReplaceLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void ReplaceLoanNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new LoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReplaceLoan(userID, bookID, viewModel)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReplaceLoan(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void ReplaceLoanInvalid()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new LoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReplaceLoan(userID, bookID, viewModel)).Throws(new InvalidDataException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReplaceLoan(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void ReplaceLoanException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new LoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReplaceLoan(userID, bookID, viewModel)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReplaceLoan(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     UpdateLoan
         * ===================
         */

        [TestMethod]
        public void UpdateLoan()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new PatchLoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.UpdateLoan(userID, bookID, viewModel));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.UpdateLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.UpdateLoan(userID, bookID, viewModel), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateLoanValidation()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new PatchLoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.UpdateLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void UpdateLoanNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new PatchLoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.UpdateLoan(userID, bookID, viewModel)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.UpdateLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.UpdateLoan(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void UpdateLoanInvalid()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new PatchLoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.UpdateLoan(userID, bookID, viewModel)).Throws(new InvalidDataException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.UpdateLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.UpdateLoan(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void UpdateLoanException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new PatchLoanViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.UpdateLoan(userID, bookID, viewModel)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.UpdateLoan(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.UpdateLoan(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         * GetUserReviewsByID
         * ===================
         */

        [TestMethod]
        public void GetUserReviewsByID()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReviewsByUserID(userID, pageNumber, pageSize)).Returns(new Envelope<UserReviewDTO>());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUserReviewsByID(userID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReviewsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetUserReviewsByIDNotFound()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReviewsByUserID(userID, pageNumber, pageSize)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUserReviewsByID(userID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReviewsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void GetUserReviewsByIDException()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReviewsByUserID(userID, pageNumber, pageSize)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUserReviewsByID(userID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReviewsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *    GetUserReview
         * ===================
         */

        [TestMethod]
        public void GetUserReview()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReview(userID, bookID)).Returns(new ReviewDTO());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUserReview(userID, bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReview(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetUserReviewNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReview(userID, bookID)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUserReview(userID, bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReview(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void GetUserReviewException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReview(userID, bookID)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetUserReview(userID, bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReview(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     AddReview
         * ===================
         */

        [TestMethod]
        public void AddReview()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new ReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.AddReview(userID, bookID, viewModel)).Returns(new ReviewDTO());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.AddReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 201);
        }

        [TestMethod]
        public void AddReviewValidation()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new ReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.AddReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void AddReviewNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new ReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.AddReview(userID, bookID, viewModel)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.AddReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void AddReviewAlreadyExists()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new ReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.AddReview(userID, bookID, viewModel)).Throws(new AlreadyExistsException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.AddReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void AddReviewException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new ReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.AddReview(userID, bookID, viewModel)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.AddReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *   ReplaceReview
         * ===================
         */

        [TestMethod]
        public void ReplaceReview()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new ReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.ReplaceReview(userID, bookID, viewModel));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.ReplaceReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReplaceReviewValidation()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new ReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.ReplaceReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void ReplaceReviewNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new ReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.ReplaceReview(userID, bookID, viewModel)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.ReplaceReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void ReplaceReviewException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new ReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.ReplaceReview(userID, bookID, viewModel)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.ReplaceReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *    UpdateReview
         * ===================
         */

        [TestMethod]
        public void UpdateReview()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new PatchReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.UpdateReview(userID, bookID, viewModel));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.UpdateReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.UpdateReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UpdateReviewValidation()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new PatchReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.UpdateReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void UpdateReviewNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new PatchReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.UpdateReview(userID, bookID, viewModel)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.UpdateReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.UpdateReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void UpdateReviewException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var viewModel = new PatchReviewViewModel();

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.UpdateReview(userID, bookID, viewModel)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.UpdateReview(userID, bookID, viewModel) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.UpdateReview(userID, bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *    DeleteReview
         * ===================
         */

        [TestMethod]
        public void DeleteReview()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.DeleteReview(userID, bookID));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteReview(userID, bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.DeleteReview(userID, bookID), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteReviewNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.DeleteReview(userID, bookID)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteReview(userID, bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.DeleteReview(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void DeleteReviewException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.DeleteReview(userID, bookID)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteReview(userID, bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.DeleteReview(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         * GetRecommendations
         * ===================
         */

        [TestMethod]
        public void GetRecommendations()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockRecommendationService.Setup(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize));

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetRecommendations(userID) as ObjectResult;

            // Assert
            mockRecommendationService.Verify(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetRecommendationsNotFound()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockRecommendationService.Setup(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize)).Throws(new NotFoundException());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetRecommendations(userID) as ObjectResult;

            // Assert
            mockRecommendationService.Verify(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void GetRecommendationsException()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockRecommendationService.Setup(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize)).Throws(new Exception());

            var controller = new UsersController(mockUserService.Object,
                                              mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockRecommendationService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetRecommendations(userID) as ObjectResult;

            // Assert
            mockRecommendationService.Verify(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }
    }
}
