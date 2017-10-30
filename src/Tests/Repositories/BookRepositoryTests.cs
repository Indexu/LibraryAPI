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
    /// <summary>
    /// The unit tests for the BookRepository
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [TestClass]
    public class BookRepositoryTests
    {
        /* 
         * ===================
         *      GetBooks
         * ===================
         */

        [TestMethod]
        public void GetBooksBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;

            var entities = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entities.AsQueryable();
            var DTOs = MockDataGenerator.CreateBooks(10);
            var envelope = MockDataGenerator.CreateBooksEnvelope(DTOs, pageNumber, pageSize);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockMapper.Setup(f => f.Map<IList<BookEntity>, IList<BookDTO>>(entities)).Returns(DTOs);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = repository.GetBooks(pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f => f.Map<IList<BookEntity>, IList<BookDTO>>(entities), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, 50);
            Assert.AreEqual(result.Paging.PageCount, 1);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, 10);
            Assert.AreEqual(result.Items.First().ID, DTOs.First().ID);
            Assert.AreEqual(result.Items.Last().ID, DTOs.Last().ID);
        }

        [TestMethod]
        public void GetBooks2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;

            var entities = MockDataGenerator.CreateBookEntities(10);
            var expectedEntities = MockDataGenerator.CreateBookEntities(5);
            var entitiesQueryable = entities.AsQueryable();
            var DTOs = MockDataGenerator.CreateBooks(10);
            var envelope = MockDataGenerator.CreateBooksEnvelope(DTOs, pageNumber, pageSize);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockMapper.Setup(f =>
                f.Map<IList<BookEntity>, IList<BookDTO>>(It.Is<IList<BookEntity>>(data =>
                    data.Count == expectedEntities.Count
                    && data.First().ID == expectedEntities.First().ID
                    && data.Last().ID == expectedEntities.Last().ID)))
                .Returns(DTOs);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = repository.GetBooks(pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f => f.Map<IList<BookEntity>, IList<BookDTO>>(It.Is<IList<BookEntity>>(data =>
                    data.Count == expectedEntities.Count
                    && data.First().ID == expectedEntities.First().ID
                    && data.Last().ID == expectedEntities.Last().ID)),
                    Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(result.Paging.PageCount, 2);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, 10);
            Assert.AreEqual(result.Items.First().ID, DTOs.First().ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBooksException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockDbContext.Setup(f => f.Books).Throws(new Exception());

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.GetBooks(pageNumber, pageSize);
        }

        /* 
         * ===================
         *     GetBookByID
         * ===================
         */

        [TestMethod]
        public void GetBookByID()
        {
            // Arrange
            var bookID = 1;
            var entities = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entities.AsQueryable();
            var entity = MockDataGenerator.CreateBookEntity(bookID);
            var DTO = MockDataGenerator.CreateBook(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockMapper.Setup(f => f.Map<BookEntity, BookDTO>(It.Is<BookEntity>(data => data.ID == bookID))).Returns(DTO);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = repository.GetBookByID(bookID);

            // Assert
            mockMapper.Verify(f => f.Map<BookEntity, BookDTO>(It.Is<BookEntity>(data => data.ID == bookID)), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetBookByIDNotFound()
        {
            // Arrange
            var bookID = 20;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.GetBookByID(bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBookByIDException()
        {
            // Arrange
            var bookID = 20;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Books).Throws(new Exception());

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.GetBookByID(bookID);
        }

        /* 
         * ===================
         *      AddBook
         * ===================
         */

        [TestMethod]
        public void AddBook()
        {
            // Arrange
            var bookID = 11;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);
            var BookEntity = MockDataGenerator.CreateBookEntity(bookID);
            var BookDTO = MockDataGenerator.CreateBook(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockBookSet.Setup(f => f.Add(It.Is<BookEntity>(data => data.Title == BookEntity.Title))).Callback<BookEntity>(u => u.ID = BookEntity.ID);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockMapper.Setup(f => f.Map<BookViewModel, BookEntity>(It.Is<BookViewModel>(data => data.Title == viewModel.Title))).Returns(BookEntity);
            mockMapper.Setup(f => f.Map<BookEntity, BookDTO>(It.Is<BookEntity>(data => data.ID == BookDTO.ID))).Returns(BookDTO);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = repository.AddBook(viewModel);

            // Assert
            mockMapper.Verify(f => f.Map<BookViewModel, BookEntity>(It.Is<BookViewModel>(data => data.Title == viewModel.Title)), Times.Once());
            mockMapper.Verify(f => f.Map<BookEntity, BookDTO>(It.Is<BookEntity>(data => data.ID == BookDTO.ID)), Times.Once());
            mockBookSet.Verify(f => f.Add(It.Is<BookEntity>(data => data.Title == BookEntity.Title)), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddBookAlreadyExists()
        {
            // Arrange
            var bookID = 5;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.AddBook(viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddBookException()
        {
            // Arrange
            var bookID = 11;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);
            var BookEntity = MockDataGenerator.CreateBookEntity(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockBookSet.Setup(f => f.Add(It.Is<BookEntity>(data => data.Title == BookEntity.Title))).Throws(new Exception());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);
            mockMapper.Setup(f => f.Map<BookViewModel, BookEntity>(It.Is<BookViewModel>(data => data.Title == viewModel.Title))).Returns(BookEntity);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.AddBook(viewModel);
        }

        /* 
         * ===================
         *     UpdateBook
         * ===================
         */

        [TestMethod]
        public void UpdateBook()
        {
            // Arrange
            var bookID = 5;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var viewModel = MockDataGenerator.CreateBookViewModel(20);
            var BookEntity = MockDataGenerator.CreateBookEntity(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockBookSet.Setup(f => f.Update(It.Is<BookEntity>(data => data.ID == BookEntity.ID)));
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.UpdateBook(bookID, viewModel);

            // Assert
            mockBookSet.Verify(f => f.Update(It.Is<BookEntity>(data => data.ID == BookEntity.ID)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void UpdateBookNotFound()
        {
            // Arrange
            var bookID = 20;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var viewModel = MockDataGenerator.CreateBookViewModel(30);
            var BookEntity = MockDataGenerator.CreateBookEntity(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.UpdateBook(bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void UpdateBookAlreadyExists()
        {
            // Arrange
            var bookID = 5;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var viewModel = MockDataGenerator.CreateBookViewModel(2);
            var BookEntity = MockDataGenerator.CreateBookEntity(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.UpdateBook(bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateBookException()
        {
            // Arrange
            var bookID = 5;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var viewModel = MockDataGenerator.CreateBookViewModel(20);
            var BookEntity = MockDataGenerator.CreateBookEntity(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockBookSet.Setup(f => f.Update(It.Is<BookEntity>(data => data.ID == BookEntity.ID))).Throws(new Exception());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.UpdateBook(bookID, viewModel);
        }

        /* 
         * ===================
         *     DeleteBook
         * ===================
         */

        [TestMethod]
        public void DeleteBook()
        {
            // Arrange
            var bookID = 5;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var BookEntity = MockDataGenerator.CreateBookEntity(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockBookSet.Setup(f => f.Remove(It.Is<BookEntity>(data => data.ID == BookEntity.ID)));
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.DeleteBookByID(bookID);

            // Assert
            mockBookSet.Verify(f => f.Remove(It.Is<BookEntity>(data => data.ID == BookEntity.ID)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void DeleteBookNotFound()
        {
            // Arrange
            var bookID = 20;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var BookEntity = MockDataGenerator.CreateBookEntity(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.DeleteBookByID(bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteBookException()
        {
            // Arrange
            var bookID = 5;
            var entites = MockDataGenerator.CreateBookEntities(10);
            var entitiesQueryable = entites.AsQueryable();
            var BookEntity = MockDataGenerator.CreateBookEntity(bookID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator()); mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockBookSet.Setup(f => f.Remove(It.Is<BookEntity>(data => data.ID == BookEntity.ID))).Throws(new Exception());
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var repository = new BookRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            repository.DeleteBookByID(bookID);
        }
    }
}
