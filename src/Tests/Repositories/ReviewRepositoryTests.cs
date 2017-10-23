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
using AutoMapper;

namespace LibraryAPI.Tests.Services
{
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
    }
}
