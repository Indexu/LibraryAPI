using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Moq;
using LibraryAPI.Repositories.EntityFrameworkCore;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Tests.MockUtils;
using LibraryAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using AutoMapper;
using JetBrains.Annotations;

namespace LibraryAPI.Tests.Services
{
    /// <summary>
    /// The unit tests for the ReviewRepository
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [TestClass]
    public class ReviewRepositoryTests
    {
        /* 
         * ===================
         *   GetBookReviews
         * ===================
         */

        [TestMethod]
        public void GetBookReviews()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfReviews = 10;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockMapper.Setup(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>())).Returns(new BookDTO());
            mockMapper.Setup(f => f.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(It.IsAny<IList<ReviewEntity>>())).Returns(new List<BookReviewDTO>());

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetBookReviews(pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>()), Times.Exactly(numberOfReviews));
            mockMapper.Verify(f => f.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(It.IsAny<IList<ReviewEntity>>()), Times.Exactly(numberOfReviews));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, 50);
            Assert.AreEqual(result.Paging.PageCount, 1);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, numberOfReviews);
        }

        [TestMethod]
        public void GetBookReviews2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            var numberOfReviews = 10;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockMapper.Setup(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>())).Returns(new BookDTO());
            mockMapper.Setup(f => f.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(It.IsAny<IList<ReviewEntity>>())).Returns(new List<BookReviewDTO>());

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetBookReviews(pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>()), Times.Exactly(numberOfReviews / 2));
            mockMapper.Verify(f => f.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(It.IsAny<IList<ReviewEntity>>()), Times.Exactly(numberOfReviews / 2));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(result.Paging.PageCount, 2);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, numberOfReviews);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBookReviewsException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfReviews = 10;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockMapper.Setup(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>())).Throws(new Exception());

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetBookReviews(pageNumber, pageSize);
        }

        /* 
         * ===================
         * GetReviewsByUserID
         * ===================
         */

        [TestMethod]
        public void GetReviewsByUserID()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfReviews = 10;
            var numberOfUsers = 10;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var userReview = MockDataGenerator.CreateUserReview(bookID, 0);
            var expectedReviews = new List<UserReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockMapper.Setup(f =>
                f.Map<IList<ReviewEntity>, IList<UserReviewDTO>>(It.Is<IList<ReviewEntity>>(data =>
                    data.Count == expectedReviews.Count)))
                .Returns(expectedReviews);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetReviewsByUserID(userID, pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f =>
                f.Map<IList<ReviewEntity>, IList<UserReviewDTO>>(It.Is<IList<ReviewEntity>>(data =>
                    data.Count == expectedReviews.Count)),
                    Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, 50);
            Assert.AreEqual(result.Paging.PageCount, 1);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, expectedReviews.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetReviewsByUserIDUserNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfReviews = 10;
            var numberOfUsers = 10;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var userReview = MockDataGenerator.CreateUserReview(bookID, 0);
            var expectedReviews = new List<UserReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockMapper.Setup(f =>
                f.Map<IList<ReviewEntity>, IList<UserReviewDTO>>(It.Is<IList<ReviewEntity>>(data =>
                    data.Count == expectedReviews.Count)))
                .Returns(expectedReviews);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetReviewsByUserID(userID, pageNumber, pageSize);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetReviewsByUserIDException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfReviews = 10;
            var numberOfUsers = 10;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var userReview = MockDataGenerator.CreateUserReview(bookID, 0);
            var expectedReviews = new List<UserReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockMapper.Setup(f =>
                f.Map<IList<ReviewEntity>, IList<UserReviewDTO>>(It.Is<IList<ReviewEntity>>(data =>
                    data.Count == expectedReviews.Count)))
                .Throws(new Exception());

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetReviewsByUserID(userID, pageNumber, pageSize);
        }

        /* 
         * ===================
         * GetReviewsByBookID
         * ===================
         */

        [TestMethod]
        public void GetReviewsByBookID()
        {
            // Arrange
            var bookID = 1;
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfReviews = 10;
            var numberOfBooks = 10;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userReview = MockDataGenerator.CreateBookReview(userID, 0);
            var expectedReviews = new List<BookReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            mockMapper.Setup(f =>
                f.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(It.Is<IList<ReviewEntity>>(data =>
                    data.Count == expectedReviews.Count)))
                .Returns(expectedReviews);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetReviewsByBookID(bookID, pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f =>
                f.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(It.Is<IList<ReviewEntity>>(data =>
                    data.Count == expectedReviews.Count)),
                    Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, 50);
            Assert.AreEqual(result.Paging.PageCount, 1);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, expectedReviews.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetReviewsByBookIDBookNotFound()
        {
            // Arrange
            var bookID = 11;
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfReviews = 10;
            var numberOfBooks = 10;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userReview = MockDataGenerator.CreateBookReview(userID, 0);
            var expectedReviews = new List<BookReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            mockMapper.Setup(f =>
                f.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(It.Is<IList<ReviewEntity>>(data =>
                    data.Count == expectedReviews.Count)))
                .Returns(expectedReviews);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetReviewsByBookID(bookID, pageNumber, pageSize);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetReviewsByBookIDException()
        {
            // Arrange
            var bookID = 1;
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfReviews = 10;
            var numberOfBooks = 10;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userReview = MockDataGenerator.CreateBookReview(userID, 0);
            var expectedReviews = new List<BookReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            mockMapper.Setup(f =>
                f.Map<IList<ReviewEntity>, IList<BookReviewDTO>>(It.Is<IList<ReviewEntity>>(data =>
                    data.Count == expectedReviews.Count)))
                .Throws(new Exception());

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

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
            var userID = 11;
            var bookID = 11;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var reviewEntity = MockDataGenerator.CreateReviewEntity(11, userID, bookID, rating);
            var reviewDTO = MockDataGenerator.CreateReview(userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            reviewEntites.Add(reviewEntity);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var userReview = MockDataGenerator.CreateBookReview(userID, 0);
            var expectedReviews = new List<BookReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockMapper.Setup(f =>
                f.Map<ReviewEntity, ReviewDTO>(It.Is<ReviewEntity>(data =>
                    data.ID == reviewEntity.ID)))
                .Returns(reviewDTO);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetReview(userID, bookID);

            // Assert
            mockMapper.Verify(f =>
                f.Map<ReviewEntity, ReviewDTO>(It.Is<ReviewEntity>(data =>
                    data.ID == reviewEntity.ID)),
                Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Rating, rating);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetReviewUserNotFound()
        {
            // Arrange
            var userID = 12;
            var bookID = 11;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var reviewEntity = MockDataGenerator.CreateReviewEntity(11, userID, bookID, rating);
            var reviewDTO = MockDataGenerator.CreateReview(userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            reviewEntites.Add(reviewEntity);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var userReview = MockDataGenerator.CreateBookReview(userID, 0);
            var expectedReviews = new List<BookReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockMapper.Setup(f =>
                f.Map<ReviewEntity, ReviewDTO>(It.Is<ReviewEntity>(data =>
                    data.ID == reviewEntity.ID)))
                .Returns(reviewDTO);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetReview(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetReviewBookNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 12;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var reviewEntity = MockDataGenerator.CreateReviewEntity(11, userID, bookID, rating);
            var reviewDTO = MockDataGenerator.CreateReview(userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            reviewEntites.Add(reviewEntity);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var userReview = MockDataGenerator.CreateBookReview(userID, 0);
            var expectedReviews = new List<BookReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockMapper.Setup(f =>
                f.Map<ReviewEntity, ReviewDTO>(It.Is<ReviewEntity>(data =>
                    data.ID == reviewEntity.ID)))
                .Returns(reviewDTO);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetReview(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetReviewReviewNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 11;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var userReview = MockDataGenerator.CreateBookReview(userID, 0);
            var expectedReviews = new List<BookReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetReview(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetReviewReviewException()
        {
            // Arrange
            var userID = 11;
            var bookID = 11;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var reviewEntity = MockDataGenerator.CreateReviewEntity(11, userID, bookID, rating);
            var reviewDTO = MockDataGenerator.CreateReview(userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            reviewEntites.Add(reviewEntity);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var userReview = MockDataGenerator.CreateBookReview(userID, 0);
            var expectedReviews = new List<BookReviewDTO> { userReview };

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockMapper.Setup(f => f.Map<ReviewEntity, ReviewDTO>(It.IsAny<ReviewEntity>())).Throws(new Exception());

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

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
            var userID = 11;
            var bookID = 11;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreateReviewViewModel(rating);
            var reviewEntity = MockDataGenerator.CreateReviewEntity(0, 0, 0, rating);
            var reviewDTO = MockDataGenerator.CreateReview(userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var userDTO = MockDataGenerator.CreateUser(userID);
            var bookDTO = MockDataGenerator.CreateBook(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockReviewSet.Setup(f =>
                f.Add(It.Is<ReviewEntity>(data =>
                    data.BookID == bookID
                    && data.UserID == userID
                    && data.Rating == rating)))
                .Callback<ReviewEntity>(data => data.ID = ++numberOfReviews);

            mockMapper.Setup(f =>
                f.Map<ReviewViewModel, ReviewEntity>(It.Is<ReviewViewModel>(data =>
                    data.Rating == rating)))
                .Returns(reviewEntity);
            mockMapper.Setup(f =>
                f.Map<UserEntity, UserDTO>(It.Is<UserEntity>(data =>
                    data.ID == userID)))
                .Returns(userDTO);
            mockMapper.Setup(f =>
                f.Map<BookEntity, BookDTO>(It.Is<BookEntity>(data =>
                    data.ID == bookID)))
                .Returns(bookDTO);
            mockMapper.Setup(f =>
                f.Map<ReviewEntity, ReviewDTO>(It.Is<ReviewEntity>(data =>
                    data.ID == numberOfReviews)))
                .Returns(reviewDTO);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.AddReview(userID, bookID, viewModel);

            // Assert
            mockReviewSet.Verify(f =>
                f.Add(It.Is<ReviewEntity>(data =>
                    data.BookID == bookID
                    && data.UserID == userID
                    && data.Rating == rating)),
                Times.Once());
            mockMapper.Verify(f =>
                f.Map<ReviewViewModel, ReviewEntity>(It.Is<ReviewViewModel>(data =>
                    data.Rating == rating)),
                Times.Once());
            mockMapper.Verify(f =>
                f.Map<UserEntity, UserDTO>(It.Is<UserEntity>(data =>
                    data.ID == userID)),
                Times.Once());
            mockMapper.Verify(f =>
                f.Map<BookEntity, BookDTO>(It.Is<BookEntity>(data =>
                    data.ID == bookID)),
                Times.Once());
            mockMapper.Verify(f =>
                f.Map<ReviewEntity, ReviewDTO>(It.Is<ReviewEntity>(data =>
                    data.ID == numberOfReviews)),
                Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.User);
            Assert.IsNotNull(result.Book);
            Assert.AreEqual(result.Rating, rating);
            Assert.AreEqual(result.User.ID, userID);
            Assert.AreEqual(result.Book.ID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void AddReviewUserNotFound()
        {
            // Arrange
            var userID = 12;
            var bookID = 11;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreateReviewViewModel(rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.AddReview(userID, bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void AddReviewBookNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 12;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreateReviewViewModel(rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.AddReview(userID, bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddReviewAlreadyExists()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreateReviewViewModel(rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.AddReview(userID, bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddReviewException()
        {
            // Arrange
            var userID = 11;
            var bookID = 11;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreateReviewViewModel(rating);
            var reviewEntity = MockDataGenerator.CreateReviewEntity(0, 0, 0, rating);
            var reviewDTO = MockDataGenerator.CreateReview(userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockReviewSet.Setup(f =>
                f.Add(It.Is<ReviewEntity>(data =>
                    data.BookID == bookID
                    && data.UserID == userID
                    && data.Rating == rating))).Throws(new Exception());

            mockMapper.Setup(f =>
                f.Map<ReviewViewModel, ReviewEntity>(It.Is<ReviewViewModel>(data =>
                    data.Rating == rating)))
                .Returns(reviewEntity);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.AddReview(userID, bookID, viewModel);
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
            var userID = 11;
            var bookID = 11;
            var rating = 3;
            var updatedRating = 2;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreatePatchReviewViewModel(updatedRating);
            var reviewEntity = MockDataGenerator.CreateReviewEntity(11, userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            reviewEntites.Add(reviewEntity);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockReviewSet.Setup(f =>
                f.Update(It.Is<ReviewEntity>(data =>
                    data.BookID == bookID
                    && data.UserID == userID
                    && data.Rating == updatedRating)));

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateReview(userID, bookID, viewModel);

            // Assert
            mockReviewSet.Verify(f =>
                f.Update(It.Is<ReviewEntity>(data =>
                    data.BookID == bookID
                    && data.UserID == userID
                    && data.Rating == updatedRating)),
                Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void UpdateReviewUserNotFound()
        {
            // Arrange
            var userID = 12;
            var bookID = 11;
            var updatedRating = 2;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreatePatchReviewViewModel(updatedRating);
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateReview(userID, bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void UpdateReviewBookNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 12;
            var updatedRating = 2;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreatePatchReviewViewModel(updatedRating);
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateReview(userID, bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void UpdateReviewReviewNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 11;
            var updatedRating = 2;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreatePatchReviewViewModel(updatedRating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateReview(userID, bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateReviewException()
        {
            // Arrange
            var userID = 11;
            var bookID = 11;
            var rating = 3;
            var updatedRating = 2;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var viewModel = MockDataGenerator.CreatePatchReviewViewModel(updatedRating);
            var reviewEntity = MockDataGenerator.CreateReviewEntity(11, userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            reviewEntites.Add(reviewEntity);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockReviewSet.Setup(f =>
                f.Update(It.Is<ReviewEntity>(data =>
                    data.BookID == bookID
                    && data.UserID == userID
                    && data.Rating == updatedRating))).Throws(new Exception());

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateReview(userID, bookID, viewModel);
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
            var userID = 11;
            var bookID = 11;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var reviewEntity = MockDataGenerator.CreateReviewEntity(11, userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            reviewEntites.Add(reviewEntity);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockReviewSet.Setup(f =>
                f.Remove(It.Is<ReviewEntity>(data =>
                    data.BookID == bookID
                    && data.UserID == userID
                    && data.Rating == rating)));

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.DeleteReview(userID, bookID);

            // Assert
            mockReviewSet.Verify(f =>
                f.Remove(It.Is<ReviewEntity>(data =>
                    data.BookID == bookID
                    && data.UserID == userID
                    && data.Rating == rating)),
                Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void DeleteReviewUserNotFound()
        {
            // Arrange
            var userID = 12;
            var bookID = 11;
            var numberOfUsers = 11;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.DeleteReview(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void DeleteReviewBookNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 12;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.DeleteReview(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void DeleteReviewReviewNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 11;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.DeleteReview(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteReviewException()
        {
            // Arrange
            var userID = 11;
            var bookID = 11;
            var rating = 3;
            var numberOfReviews = 10;
            var numberOfBooks = 11;
            var numberOfUsers = 11;

            var reviewEntity = MockDataGenerator.CreateReviewEntity(11, userID, bookID, rating);
            var reviewEntites = MockDataGenerator.CreateReviewEntities(numberOfReviews);
            reviewEntites.Add(reviewEntity);
            var reviewEntitiesQueryable = reviewEntites.AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockReviewSet = new Mock<DbSet<ReviewEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Provider).Returns(reviewEntitiesQueryable.Provider);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.Expression).Returns(reviewEntitiesQueryable.Expression);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.ElementType).Returns(reviewEntitiesQueryable.ElementType);
            mockReviewSet.As<IQueryable<ReviewEntity>>().Setup(m => m.GetEnumerator()).Returns(reviewEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Reviews).Returns(mockReviewSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            mockReviewSet.Setup(f =>
                f.Remove(It.Is<ReviewEntity>(data =>
                    data.BookID == bookID
                    && data.UserID == userID
                    && data.Rating == rating))).Throws(new Exception());

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.DeleteReview(userID, bookID);
        }

        /* 
         * ===================
         * GetRecommendationsByUserID
         * ===================
         */

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetRecommendationsByUserIDUserNotFound()
        {
            // Arrange
            var userID = 12;
            var pageNumber = 1;
            int? pageSize = null;
            var numberOfUsers = 11;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockMapper = new Mock<IMapper>();
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var service = new ReviewRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetRecommendationsByUserID(userID, pageNumber, pageSize);

        }
    }
}
