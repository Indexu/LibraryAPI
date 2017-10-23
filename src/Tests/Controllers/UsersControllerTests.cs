using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LibraryAPI.API.Controllers;
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
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Tests.Services
{
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
    }
}
