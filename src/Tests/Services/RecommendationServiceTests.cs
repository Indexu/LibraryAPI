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
    /// The unit tests for the RecommendationService
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [TestClass]
    public class RecommendationServiceTests
    {
        /* 
         * ===================
         * GetRecommendationsByUserID
         * ===================
         */

        [TestMethod]
        public void GetRecommendationsByUserIDBasic()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfRecommendations = 10;
            var recommendations = MockDataGenerator.CreateRecommendations(numberOfRecommendations);
            var envelope = MockDataGenerator.CreateRecommendationsEnvelope(recommendations, pageNumber, pageSize);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize)).Returns(envelope);

            var service = new RecommendationService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedRecommendations = service.GetRecommendationsByUserID(userID, pageNumber, pageSize);

            // Assert
            mockReviewRepo.Verify(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedRecommendations);
            Assert.AreEqual(returnedRecommendations.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedRecommendations.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedRecommendations.Paging.PageCount, 1);
            Assert.AreEqual(returnedRecommendations.Paging.TotalNumberOfItems, numberOfRecommendations);
            Assert.AreEqual(returnedRecommendations.Items.First().Book.ID, recommendations.First().Book.ID);
            Assert.AreEqual(returnedRecommendations.Items.Last().Book.ID, recommendations.Last().Book.ID);
        }

        [TestMethod]
        public void GetRecommendationsByUserID2Pages()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = 5;
            var numberOfRecommendations = 10;
            var recommendations = MockDataGenerator.CreateRecommendations(numberOfRecommendations);
            var envelope = MockDataGenerator.CreateRecommendationsEnvelope(recommendations, pageNumber, pageSize);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize)).Returns(envelope);

            var service = new RecommendationService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedRecommendations = service.GetRecommendationsByUserID(userID, pageNumber, pageSize);

            // Assert
            mockReviewRepo.Verify(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedRecommendations);
            Assert.AreEqual(returnedRecommendations.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedRecommendations.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedRecommendations.Paging.PageCount, 2);
            Assert.AreEqual(returnedRecommendations.Paging.TotalNumberOfItems, numberOfRecommendations);
            Assert.AreEqual(returnedRecommendations.Items.First().Book.ID, recommendations.First().Book.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetRecommendationsByUserIDException()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetRecommendationsByUserID(userID, pageNumber, pageSize)).Throws(new Exception());

            var service = new RecommendationService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.GetRecommendationsByUserID(userID, pageNumber, pageSize);
        }
    }
}
