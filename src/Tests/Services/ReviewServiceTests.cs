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
    [TestClass]
    public class ReviewServiceTests
    {
        /* 
         * ===================
         *    GetBookReviews
         * ===================
         */

        [TestMethod]
        public void GetBookReviewsBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            int numberOfBookReviews = 10;

            var reviews = MockDataGenerator.CreateBookReviewsForBook(numberOfBookReviews);
            var envelope = MockDataGenerator.CreateBookReviewsEnvelope(reviews, pageNumber, pageSize);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetBookReviews(pageNumber, pageSize)).Returns(envelope);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedReviews = service.GetBookReviews(pageNumber, pageSize);

            // Assert
            mockReviewRepo.Verify(f => f.GetBookReviews(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedReviews);
            Assert.AreEqual(returnedReviews.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReviews.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedReviews.Paging.PageCount, 1);
            Assert.AreEqual(returnedReviews.Paging.TotalNumberOfItems, numberOfBookReviews);
            Assert.AreEqual(returnedReviews.Items.First().Book.ID, reviews.First().Book.ID);
            Assert.AreEqual(returnedReviews.Items.Last().Book.ID, reviews.Last().Book.ID);
        }

        [TestMethod]
        public void GetReviews2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            int numberOfBookReviews = 10;

            var reviews = MockDataGenerator.CreateBookReviewsForBook(numberOfBookReviews);
            var envelope = MockDataGenerator.CreateBookReviewsEnvelope(reviews, pageNumber, pageSize);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetBookReviews(pageNumber, pageSize)).Returns(envelope);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedReviews = service.GetBookReviews(pageNumber, pageSize);

            // Assert
            mockReviewRepo.Verify(f => f.GetBookReviews(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedReviews);
            Assert.AreEqual(returnedReviews.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReviews.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedReviews.Paging.PageCount, 2);
            Assert.AreEqual(returnedReviews.Paging.TotalNumberOfItems, numberOfBookReviews);
            Assert.AreEqual(returnedReviews.Items.First().Book.ID, reviews.First().Book.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBookReviewsException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetBookReviews(pageNumber, pageSize)).Throws(new Exception());

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.GetBookReviews(pageNumber, pageSize);
        }

        /* 
         * ===================
         *  GetReviewsByUserID
         * ===================
         */

        [TestMethod]
        public void GetReviewsByUserIDBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            int numberOfReviews = 10;
            int userID = 1;

            var reviews = MockDataGenerator.CreateUserReviews(numberOfReviews);
            var envelope = MockDataGenerator.CreateUserReviewEnvelope(reviews, pageNumber, pageSize);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetReviewsByUserID(userID, pageNumber, pageSize)).Returns(envelope);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedReviews = service.GetReviewsByUserID(userID, pageNumber, pageSize);

            // Assert
            mockReviewRepo.Verify(f => f.GetReviewsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedReviews);
            Assert.AreEqual(returnedReviews.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReviews.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedReviews.Paging.PageCount, 1);
            Assert.AreEqual(returnedReviews.Paging.TotalNumberOfItems, numberOfReviews);
            Assert.AreEqual(returnedReviews.Items.First().Book.ID, reviews.First().Book.ID);
            Assert.AreEqual(returnedReviews.Items.Last().Book.ID, reviews.Last().Book.ID);
        }

        [TestMethod]
        public void GetReviewsByUserID2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            int numberOfReviews = 10;
            int userID = 1;

            var reviews = MockDataGenerator.CreateUserReviews(numberOfReviews);
            var envelope = MockDataGenerator.CreateUserReviewEnvelope(reviews, pageNumber, pageSize);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetReviewsByUserID(userID, pageNumber, pageSize)).Returns(envelope);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedReviews = service.GetReviewsByUserID(userID, pageNumber, pageSize);

            // Assert
            mockReviewRepo.Verify(f => f.GetReviewsByUserID(userID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedReviews);
            Assert.AreEqual(returnedReviews.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReviews.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedReviews.Paging.PageCount, 2);
            Assert.AreEqual(returnedReviews.Paging.TotalNumberOfItems, numberOfReviews);
            Assert.AreEqual(returnedReviews.Items.First().Book.ID, reviews.First().Book.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetReviewsByUserIDException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            int userID = 1;

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetReviewsByUserID(userID, pageNumber, pageSize)).Throws(new Exception());

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.GetReviewsByUserID(userID, pageNumber, pageSize);
        }

        /* 
         * ===================
         *  GetReviewsByBookID
         * ===================
         */

        [TestMethod]
        public void GetReviewsByBookIDBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            int numberOfReviews = 10;
            int bookID = 1;

            var reviews = MockDataGenerator.CreateBookReviews(numberOfReviews);
            var envelope = MockDataGenerator.CreateBookReviewEnvelope(reviews, pageNumber, pageSize);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize)).Returns(envelope);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedReviews = service.GetReviewsByBookID(bookID, pageNumber, pageSize);

            // Assert
            mockReviewRepo.Verify(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedReviews);
            Assert.AreEqual(returnedReviews.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReviews.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedReviews.Paging.PageCount, 1);
            Assert.AreEqual(returnedReviews.Paging.TotalNumberOfItems, numberOfReviews);
            Assert.AreEqual(returnedReviews.Items.First().User.ID, reviews.First().User.ID);
            Assert.AreEqual(returnedReviews.Items.Last().User.ID, reviews.Last().User.ID);
        }

        [TestMethod]
        public void GetReviewsByBookID2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            int numberOfReviews = 10;
            int bookID = 1;

            var reviews = MockDataGenerator.CreateBookReviews(numberOfReviews);
            var envelope = MockDataGenerator.CreateBookReviewEnvelope(reviews, pageNumber, pageSize);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize)).Returns(envelope);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedReviews = service.GetReviewsByBookID(bookID, pageNumber, pageSize);

            // Assert
            mockReviewRepo.Verify(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedReviews);
            Assert.AreEqual(returnedReviews.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedReviews.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedReviews.Paging.PageCount, 2);
            Assert.AreEqual(returnedReviews.Paging.TotalNumberOfItems, numberOfReviews);
            Assert.AreEqual(returnedReviews.Items.First().User.ID, reviews.First().User.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetReviewsByBookIDException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            int bookID = 1;

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize)).Throws(new Exception());

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.GetReviewsByBookID(bookID, pageNumber, pageSize);
        }

        /* 
         * ===================
         *     GetReview
         * ===================
         */

        [TestMethod]
        public void GetReview()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;
            int rating = 4;

            var review = MockDataGenerator.CreateReview(userID, bookID, rating);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetReview(userID, bookID)).Returns(review);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedReview = service.GetReview(userID, bookID);

            // Assert
            mockReviewRepo.Verify(f => f.GetReview(userID, bookID), Times.Once());
            Assert.IsNotNull(returnedReview);
            Assert.AreEqual(returnedReview.User.ID, userID);
            Assert.AreEqual(returnedReview.Book.ID, bookID);
            Assert.AreEqual(returnedReview.Rating, rating);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetReviewException()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.GetReview(userID, bookID)).Throws(new Exception());

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.GetReview(userID, bookID);
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
            int userID = 1;
            int bookID = 1;
            int rating = 4;

            var review = MockDataGenerator.CreateReviewViewModel(rating);
            var reviewDTO = MockDataGenerator.CreateReview(userID, bookID, rating);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.AddReview(userID, bookID, review)).Returns(reviewDTO);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedReview = service.AddReview(userID, bookID, review);

            // Assert
            mockReviewRepo.Verify(f => f.AddReview(userID, bookID, review), Times.Once());
            Assert.IsNotNull(returnedReview);
            Assert.AreEqual(returnedReview.User.ID, userID);
            Assert.AreEqual(returnedReview.Book.ID, bookID);
            Assert.AreEqual(returnedReview.Rating, rating);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddReviewException()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.AddReview(userID, bookID, It.IsAny<ReviewViewModel>())).Throws(new Exception());

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            var returnedReview = service.AddReview(userID, bookID, It.IsAny<ReviewViewModel>());
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
            int userID = 1;
            int bookID = 1;
            int rating = 4;

            var review = MockDataGenerator.CreateReviewViewModel(rating);
            var patchReview = MockDataGenerator.CreatePatchReviewViewModel(rating);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.UpdateReview(userID, bookID, patchReview));
            mockMapper.Setup(x => x.Map<ReviewViewModel, PatchReviewViewModel>(review)).Returns(patchReview);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.ReplaceReview(userID, bookID, review);

            // Assert
            mockReviewRepo.Verify(f => f.UpdateReview(userID, bookID, patchReview), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReplaceReviewException()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;
            int rating = 4;

            var review = MockDataGenerator.CreateReviewViewModel(rating);
            var patchReview = MockDataGenerator.CreatePatchReviewViewModel(rating);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.UpdateReview(userID, bookID, patchReview)).Throws(new Exception());
            mockMapper.Setup(x => x.Map<ReviewViewModel, PatchReviewViewModel>(review)).Returns(patchReview);

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.ReplaceReview(userID, bookID, review);
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
            int userID = 1;
            int bookID = 1;
            int rating = 4;

            var patchReview = MockDataGenerator.CreatePatchReviewViewModel(rating);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.UpdateReview(userID, bookID, patchReview));

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.UpdateReview(userID, bookID, patchReview);

            // Assert
            mockReviewRepo.Verify(f => f.UpdateReview(userID, bookID, patchReview), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateReviewException()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;
            int rating = 4;

            var patchReview = MockDataGenerator.CreatePatchReviewViewModel(rating);

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.UpdateReview(userID, bookID, patchReview)).Throws(new Exception());

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.UpdateReview(userID, bookID, patchReview);
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
            int userID = 1;
            int bookID = 1;

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.DeleteReview(userID, bookID));

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.DeleteReview(userID, bookID);

            // Assert
            mockReviewRepo.Verify(f => f.DeleteReview(userID, bookID), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteReviewException()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;

            var mockReviewRepo = new Mock<IReviewRepository>();
            var mockMapper = new Mock<IMapper>();

            mockReviewRepo.Setup(f => f.DeleteReview(userID, bookID)).Throws(new Exception());

            var service = new ReviewService(mockReviewRepo.Object, mockMapper.Object);

            // Act
            service.DeleteReview(userID, bookID);
        }
    }
}
