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
    /// The unit tests for the LoanRepository
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [TestClass]
    public class LoanRepositoryTests
    {
        /* 
         * ===================
         *  GetLoansByUserID
         * ===================
         */

        [TestMethod]
        public void GetLoansByUserIDBasic()
        {
            // Arrange
            var userID = 1;
            var numberOfLoans = 10;
            var pageNumber = 1;
            int? pageSize = null;

            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();
            var expectedEntites = entities.Where(x => x.UserID == userID).ToList();
            var DTOs = MockDataGenerator.CreateUserLoans(expectedEntites.Count);
            var evelope = MockDataGenerator.CreateUsersLoansEnvelope(DTOs, pageNumber, pageSize);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockMapper.Setup(f =>
                f.Map<IList<LoanEntity>, IList<UserLoanDTO>>(It.Is<IList<LoanEntity>>(data =>
                    data.Count == expectedEntites.Count
                    && data.First().ID == expectedEntites.First().ID
                    && data.Last().ID == expectedEntites.Last().ID)))
                .Returns(DTOs);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetLoansByUserID(userID, false, pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f =>
                f.Map<IList<LoanEntity>, IList<UserLoanDTO>>(It.Is<IList<LoanEntity>>(data =>
                    data.Count == expectedEntites.Count
                    && data.First().ID == expectedEntites.First().ID
                    && data.Last().ID == expectedEntites.Last().ID)),
                Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, 50);
            Assert.AreEqual(result.Paging.PageCount, 1);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, DTOs.Count);
            Assert.AreEqual(result.Items.First().Book.ID, DTOs.First().Book.ID);
            Assert.AreEqual(result.Items.Last().Book.ID, DTOs.Last().Book.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetLoansByUserIDException()
        {
            // Arrange
            var userID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockDbContext.Setup(f => f.Loans).Throws(new Exception());

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetLoansByUserID(userID, false, pageNumber, pageSize);
        }

        /* 
         * ===================
         *  GetLoansByBookID
         * ===================
         */

        [TestMethod]
        public void GetLoansByBookIDBasic()
        {
            // Arrange
            var bookID = 1;
            var numberOfLoans = 10;
            var pageNumber = 1;
            int? pageSize = null;

            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();
            var expectedEntites = entities.Where(x => x.BookID == bookID).ToList();
            var DTOs = MockDataGenerator.CreateBookLoans(expectedEntites.Count);
            var evelope = MockDataGenerator.CreateBookLoansEnvelope(DTOs, pageNumber, pageSize);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockMapper.Setup(f =>
                f.Map<IList<LoanEntity>, IList<BookLoanDTO>>(It.Is<IList<LoanEntity>>(data =>
                    data.Count == expectedEntites.Count
                    && data.First().ID == expectedEntites.First().ID
                    && data.Last().ID == expectedEntites.Last().ID)))
                .Returns(DTOs);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetLoansByBookID(bookID, false, pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f =>
                f.Map<IList<LoanEntity>, IList<BookLoanDTO>>(It.Is<IList<LoanEntity>>(data =>
                    data.Count == expectedEntites.Count
                    && data.First().ID == expectedEntites.First().ID
                    && data.Last().ID == expectedEntites.Last().ID)),
                Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, 50);
            Assert.AreEqual(result.Paging.PageCount, 1);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, DTOs.Count);
            Assert.AreEqual(result.Items.First().User.ID, DTOs.First().User.ID);
            Assert.AreEqual(result.Items.Last().User.ID, DTOs.Last().User.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetLoansByBookIDException()
        {
            // Arrange
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockDbContext.Setup(f => f.Loans).Throws(new Exception());

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetLoansByBookID(bookID, false, pageNumber, pageSize);
        }

        /* 
         * ===================
         *      AddLoan
         * ===================
         */

        [TestMethod]
        public void AddLoan()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();
            var expectedEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, null);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockLoanSet.Setup(f =>
                f.Add(It.Is<LoanEntity>(data =>
                    data.UserID == expectedEntity.UserID
                    && data.BookID == expectedEntity.BookID)))
                .Callback<LoanEntity>(u => u.ID = expectedEntity.ID);

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.AddLoan(userID, bookID);

            // Assert
            mockLoanSet.Verify(f =>
                f.Add(It.Is<LoanEntity>(data =>
                    data.UserID == expectedEntity.UserID
                    && data.BookID == expectedEntity.BookID)),
                Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result, expectedEntity.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void AddLoanUserNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 1;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();
            var expectedEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, null);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockLoanSet.Setup(f =>
                f.Add(It.Is<LoanEntity>(data =>
                    data.UserID == expectedEntity.UserID
                    && data.BookID == expectedEntity.BookID)))
                .Callback<LoanEntity>(u => u.ID = expectedEntity.ID);

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.AddLoan(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void AddLoanBookNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 11;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();
            var expectedEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, null);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockLoanSet.Setup(f =>
                f.Add(It.Is<LoanEntity>(data =>
                    data.UserID == expectedEntity.UserID
                    && data.BookID == expectedEntity.BookID)))
                .Callback<LoanEntity>(u => u.ID = expectedEntity.ID);

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.AddLoan(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddLoanAlreadyExists()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var loanEntity = MockDataGenerator.CreateLoanEntity(1, userID, bookID, DateTime.Now, null);
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = new List<LoanEntity> { loanEntity };
            var entitiesQueryable = entities.AsQueryable();
            var expectedEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, null);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockLoanSet.Setup(f =>
                f.Add(It.Is<LoanEntity>(data =>
                    data.UserID == expectedEntity.UserID
                    && data.BookID == expectedEntity.BookID)))
                .Callback<LoanEntity>(u => u.ID = expectedEntity.ID);

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.AddLoan(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddLoanException()
        {
            // Arrange
            var userID = 11;
            var bookID = 1;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();
            var expectedEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, null);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockLoanSet.Setup(f =>
                f.Add(It.Is<LoanEntity>(data =>
                    data.UserID == expectedEntity.UserID
                    && data.BookID == expectedEntity.BookID)))
                .Callback<LoanEntity>(u => u.ID = expectedEntity.ID);

            mockDbContext.Setup(f => f.Loans).Throws(new Exception());
            mockDbContext.Setup(f => f.Users).Throws(new Exception());
            mockDbContext.Setup(f => f.Books).Throws(new Exception());

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.AddLoan(userID, bookID);
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
            var userID = 11;
            var bookID = 11;
            var numberOfLoans = 10;
            var numberOfUsers = 11;
            var numberOfBooks = 11;

            var loanEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, null);
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            entities.Add(loanEntity);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockLoanSet.Setup(f =>
                f.Update(It.Is<LoanEntity>(data =>
                    data.UserID == loanEntity.UserID
                    && data.BookID == loanEntity.BookID
                    && data.ReturnDate.HasValue)));

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.ReturnBook(userID, bookID);

            // Assert
            mockLoanSet.Verify(f =>
                f.Update(It.Is<LoanEntity>(data =>
                    data.UserID == loanEntity.UserID
                    && data.BookID == loanEntity.BookID
                    && data.ReturnDate.HasValue)),
                Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ReturnBookUserNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 1;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.ReturnBook(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ReturnBookBookNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 11;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.ReturnBook(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ReturnBookLoanNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 11;
            var numberOfLoans = 10;
            var numberOfUsers = 11;
            var numberOfBooks = 11;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.ReturnBook(userID, bookID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReturnBookException()
        {
            // Arrange
            var userID = 11;
            var bookID = 11;
            var numberOfLoans = 10;
            var numberOfUsers = 11;
            var numberOfBooks = 11;

            var loanEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, null);
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            entities.Add(loanEntity);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockLoanSet.Setup(f => f.Update(It.IsAny<LoanEntity>())).Throws(new Exception());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.ReturnBook(userID, bookID);
        }

        /* 
         * ===================
         *   GetUsersReport
         * ===================
         */

        [TestMethod]
        public void GetUsersReportBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            DateTime? loanDate = new DateTime(2017, 1, 1);
            int? duration = 30;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var expectedSize = 3;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockMapper.Setup(f => f.Map<IList<LoanEntity>, IList<UserLoanDTO>>(It.IsAny<IList<LoanEntity>>())).Returns(new List<UserLoanDTO>());
            mockMapper.Setup(f => f.Map<UserEntity, UserDTO>(It.IsAny<UserEntity>())).Returns(new UserDTO());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetUsersReport(pageNumber, pageSize, loanDate, duration);

            // Assert
            mockMapper.Verify(f => f.Map<IList<LoanEntity>, IList<UserLoanDTO>>(It.IsAny<IList<LoanEntity>>()), Times.Exactly(expectedSize));
            mockMapper.Verify(f => f.Map<UserEntity, UserDTO>(It.IsAny<UserEntity>()), Times.Exactly(expectedSize));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, 50);
            Assert.AreEqual(result.Paging.PageCount, 1);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, expectedSize);
        }

        [TestMethod]
        public void GetUsersReport2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 2;
            DateTime? loanDate = new DateTime(2017, 1, 1);
            int? duration = 30;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var expectedSize = 3;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockMapper.Setup(f => f.Map<IList<LoanEntity>, IList<UserLoanDTO>>(It.IsAny<IList<LoanEntity>>())).Returns(new List<UserLoanDTO>());
            mockMapper.Setup(f => f.Map<UserEntity, UserDTO>(It.IsAny<UserEntity>())).Returns(new UserDTO());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetUsersReport(pageNumber, pageSize, loanDate, duration);

            // Assert
            mockMapper.Verify(f => f.Map<IList<LoanEntity>, IList<UserLoanDTO>>(It.IsAny<IList<LoanEntity>>()), Times.Exactly(expectedSize - 1));
            mockMapper.Verify(f => f.Map<UserEntity, UserDTO>(It.IsAny<UserEntity>()), Times.Exactly(expectedSize - 1));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(result.Paging.PageCount, 2);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, expectedSize);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetUsersReportException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            DateTime? loanDate = new DateTime(2017, 1, 1);
            int? duration = 30;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockMapper.Setup(f => f.Map<IList<LoanEntity>, IList<UserLoanDTO>>(It.IsAny<IList<LoanEntity>>())).Returns(new List<UserLoanDTO>());
            mockMapper.Setup(f => f.Map<UserEntity, UserDTO>(It.IsAny<UserEntity>())).Throws(new Exception());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetUsersReport(pageNumber, pageSize, loanDate, duration);
        }

        /* 
         * ===================
         *   GetBooksReport
         * ===================
         */

        [TestMethod]
        public void GetBooksReportBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            DateTime? loanDate = new DateTime(2017, 1, 1);
            int? duration = 30;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var expectedSize = 3;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockMapper.Setup(f => f.Map<IList<LoanEntity>, IList<BookLoanDTO>>(It.IsAny<IList<LoanEntity>>())).Returns(new List<BookLoanDTO>());
            mockMapper.Setup(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>())).Returns(new BookDTO());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetBooksReport(pageNumber, pageSize, loanDate, duration);

            // Assert
            mockMapper.Verify(f => f.Map<IList<LoanEntity>, IList<BookLoanDTO>>(It.IsAny<IList<LoanEntity>>()), Times.Exactly(expectedSize));
            mockMapper.Verify(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>()), Times.Exactly(expectedSize));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, 50);
            Assert.AreEqual(result.Paging.PageCount, 1);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, expectedSize);
        }

        [TestMethod]
        public void GetBooksReport2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 2;
            DateTime? loanDate = new DateTime(2017, 1, 1);
            int? duration = 30;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var expectedSize = 3;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockMapper.Setup(f => f.Map<IList<LoanEntity>, IList<BookLoanDTO>>(It.IsAny<IList<LoanEntity>>())).Returns(new List<BookLoanDTO>());
            mockMapper.Setup(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>())).Returns(new BookDTO());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            var result = service.GetBooksReport(pageNumber, pageSize, loanDate, duration);

            // Assert
            mockMapper.Verify(f => f.Map<IList<LoanEntity>, IList<BookLoanDTO>>(It.IsAny<IList<LoanEntity>>()), Times.Exactly(expectedSize - 1));
            mockMapper.Verify(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>()), Times.Exactly(expectedSize - 1));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(result.Paging.PageCount, 2);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, expectedSize);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBooksReportException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 2;
            DateTime? loanDate = new DateTime(2017, 1, 1);
            int? duration = 30;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockMapper.Setup(f => f.Map<IList<LoanEntity>, IList<BookLoanDTO>>(It.IsAny<IList<LoanEntity>>())).Returns(new List<BookLoanDTO>());
            mockMapper.Setup(f => f.Map<BookEntity, BookDTO>(It.IsAny<BookEntity>())).Throws(new Exception());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.GetBooksReport(pageNumber, pageSize, loanDate, duration);
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
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var loanEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, null);
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            entities.Add(loanEntity);
            var entitiesQueryable = entities.AsQueryable();
            var viewModel = MockDataGenerator.CreatePatchLoanViewModel(new DateTime(1999, 1, 1), new DateTime(2010, 1, 1));

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockLoanSet.Setup(f =>
                f.Update(It.Is<LoanEntity>(data =>
                    data.UserID == userID
                    && data.BookID == bookID
                    && data.ReturnDate == viewModel.ReturnDate
                    && data.LoanDate == viewModel.LoanDate)));

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateLoan(userID, bookID, viewModel);

            // Assert
            mockLoanSet.Verify(f =>
                f.Update(It.Is<LoanEntity>(data =>
                    data.UserID == userID
                    && data.BookID == bookID
                    && data.ReturnDate == viewModel.ReturnDate
                    && data.LoanDate == viewModel.LoanDate)),
                Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void UpdateBookUserNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 1;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();
            var viewModel = MockDataGenerator.CreatePatchLoanViewModel(new DateTime(1999, 1, 1), new DateTime(2010, 1, 1));

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateLoan(userID, bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void UpdateLoanBookBookNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 11;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            var entitiesQueryable = entities.AsQueryable();
            var viewModel = MockDataGenerator.CreatePatchLoanViewModel(new DateTime(1999, 1, 1), new DateTime(2010, 1, 1));

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateLoan(userID, bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void UpdateLoanLoanNotFound()
        {
            // Arrange
            var userID = 11;
            var bookID = 1;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var loanEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, DateTime.Now);
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            entities.Add(loanEntity);
            var entitiesQueryable = entities.AsQueryable();
            var viewModel = MockDataGenerator.CreatePatchLoanViewModel(new DateTime(1999, 1, 1), new DateTime(2010, 1, 1));

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateLoan(userID, bookID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateLoanException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;
            var numberOfLoans = 10;
            var numberOfUsers = 10;
            var numberOfBooks = 10;

            var loanEntity = MockDataGenerator.CreateLoanEntity(11, userID, bookID, DateTime.Now, null);
            var userEntitiesQueryable = MockDataGenerator.CreateUserEntities(numberOfUsers).AsQueryable();
            var bookEntitiesQueryable = MockDataGenerator.CreateBookEntities(numberOfBooks).AsQueryable();
            var entities = MockDataGenerator.CreateLoanEntities(numberOfLoans);
            entities.Add(loanEntity);
            var entitiesQueryable = entities.AsQueryable();
            var viewModel = MockDataGenerator.CreatePatchLoanViewModel(new DateTime(1999, 1, 1), new DateTime(2010, 1, 1));

            var mockDbContext = new Mock<DatabaseContext>();
            var mockLoanSet = new Mock<DbSet<LoanEntity>>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockBookSet = new Mock<DbSet<BookEntity>>();
            var mockMapper = new Mock<IMapper>();

            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Provider).Returns(entitiesQueryable.Provider);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.Expression).Returns(entitiesQueryable.Expression);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.ElementType).Returns(entitiesQueryable.ElementType);
            mockLoanSet.As<IQueryable<LoanEntity>>().Setup(m => m.GetEnumerator()).Returns(entitiesQueryable.GetEnumerator());

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());

            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Provider).Returns(bookEntitiesQueryable.Provider);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.Expression).Returns(bookEntitiesQueryable.Expression);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.ElementType).Returns(bookEntitiesQueryable.ElementType);
            mockBookSet.As<IQueryable<BookEntity>>().Setup(m => m.GetEnumerator()).Returns(bookEntitiesQueryable.GetEnumerator());

            mockLoanSet.Setup(f => f.Update(It.IsAny<LoanEntity>())).Throws(new Exception());

            mockDbContext.Setup(f => f.Loans).Returns(mockLoanSet.Object);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockDbContext.Setup(f => f.Books).Returns(mockBookSet.Object);

            var service = new LoanRepository(mockDbContext.Object, mockMapper.Object);

            // Act
            service.UpdateLoan(userID, bookID, viewModel);
        }
    }
}
