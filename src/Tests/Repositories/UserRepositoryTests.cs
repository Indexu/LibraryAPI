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
    public class UserRepositoryTests
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

            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var userDTOs = MockDataGenerator.CreateUsers(10);
            var envelope = MockDataGenerator.CreateUsersEnvelope(userDTOs, pageNumber, pageSize);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockMapper.Setup(f => f.Map<IList<UserEntity>, IList<UserDTO>>(userEntities)).Returns(userDTOs);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            var result = repository.GetUsers(pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f => f.Map<IList<UserEntity>, IList<UserDTO>>(userEntities), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, 50);
            Assert.AreEqual(result.Paging.PageCount, 1);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, 10);
            Assert.AreEqual(result.Items.First().ID, userDTOs.First().ID);
            Assert.AreEqual(result.Items.Last().ID, userDTOs.Last().ID);
        }

        [TestMethod]
        public void GetUsers2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;

            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var expectedUserEntities = MockDataGenerator.CreateUserEntities(5);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var userDTOs = MockDataGenerator.CreateUsers(10);
            var envelope = MockDataGenerator.CreateUsersEnvelope(userDTOs, pageNumber, pageSize);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockMapper.Setup(f =>
                f.Map<IList<UserEntity>, IList<UserDTO>>(It.Is<IList<UserEntity>>(data =>
                    data.Count == expectedUserEntities.Count
                    && data.First().ID == expectedUserEntities.First().ID
                    && data.Last().ID == expectedUserEntities.Last().ID)))
                .Returns(userDTOs);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            var result = repository.GetUsers(pageNumber, pageSize);

            // Assert
            mockMapper.Verify(f => f.Map<IList<UserEntity>, IList<UserDTO>>(It.Is<IList<UserEntity>>(data =>
                    data.Count == expectedUserEntities.Count
                    && data.First().ID == expectedUserEntities.First().ID
                    && data.Last().ID == expectedUserEntities.Last().ID)),
                    Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Paging.PageNumber, pageNumber);
            Assert.AreEqual(result.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(result.Paging.PageCount, 2);
            Assert.AreEqual(result.Paging.TotalNumberOfItems, 10);
            Assert.AreEqual(result.Items.First().ID, userDTOs.First().ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetUsersException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockDbContext.Setup(f => f.Users).Throws(new Exception());

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.GetUsers(pageNumber, pageSize);
        }

        /* 
         * ===================
         *     GetUserByID
         * ===================
         */

        [TestMethod]
        public void GetUserByID()
        {
            // Arrange
            var userID = 1;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var userEntity = MockDataGenerator.CreateUserEntity(userID);
            var userDTO = MockDataGenerator.CreateUser(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockMapper.Setup(f => f.Map<UserEntity, UserDTO>(It.Is<UserEntity>(data => data.ID == userID))).Returns(userDTO);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            var result = repository.GetUserByID(userID);

            // Assert
            mockMapper.Verify(f => f.Map<UserEntity, UserDTO>(It.Is<UserEntity>(data => data.ID == userID)), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ID, userID);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetUserByIDNotFound()
        {
            // Arrange
            var userID = 20;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.GetUserByID(userID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetUserByIDException()
        {
            // Arrange
            var userID = 20;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Users).Throws(new Exception());

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.GetUserByID(userID);
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
            var userID = 11;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var viewModel = MockDataGenerator.CreateUserViewModel(userID);
            var userEntity = MockDataGenerator.CreateUserEntity(userID);
            var userDTO = MockDataGenerator.CreateUser(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockUserSet.Setup(f => f.Add(It.Is<UserEntity>(data => data.Email == userEntity.Email))).Callback<UserEntity>(u => u.ID = userEntity.ID);
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockMapper.Setup(f => f.Map<UserViewModel, UserEntity>(It.Is<UserViewModel>(data => data.Email == viewModel.Email))).Returns(userEntity);
            mockMapper.Setup(f => f.Map<UserEntity, UserDTO>(It.Is<UserEntity>(data => data.ID == userDTO.ID))).Returns(userDTO);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            var result = repository.AddUser(viewModel);

            // Assert
            mockMapper.Verify(f => f.Map<UserViewModel, UserEntity>(It.Is<UserViewModel>(data => data.Email == viewModel.Email)), Times.Once());
            mockMapper.Verify(f => f.Map<UserEntity, UserDTO>(It.Is<UserEntity>(data => data.ID == userDTO.ID)), Times.Once());
            mockUserSet.Verify(f => f.Add(It.Is<UserEntity>(data => data.Email == userEntity.Email)), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ID, userID);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void AddUserAlreadyExists()
        {
            // Arrange
            var userID = 5;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var viewModel = MockDataGenerator.CreateUserViewModel(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.AddUser(viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddUserException()
        {
            // Arrange
            var userID = 11;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var viewModel = MockDataGenerator.CreateUserViewModel(userID);
            var userEntity = MockDataGenerator.CreateUserEntity(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockUserSet.Setup(f => f.Add(It.Is<UserEntity>(data => data.Email == userEntity.Email))).Throws(new Exception());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);
            mockMapper.Setup(f => f.Map<UserViewModel, UserEntity>(It.Is<UserViewModel>(data => data.Email == viewModel.Email))).Returns(userEntity);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.AddUser(viewModel);
        }

        /* 
         * ===================
         *     UpdateUser
         * ===================
         */

        [TestMethod]
        public void UpdateUser()
        {
            // Arrange
            var userID = 5;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var viewModel = MockDataGenerator.CreateUserViewModel(20);
            var userEntity = MockDataGenerator.CreateUserEntity(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockUserSet.Setup(f => f.Update(It.Is<UserEntity>(data => data.ID == userEntity.ID)));
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.UpdateUser(userID, viewModel);

            // Assert
            mockUserSet.Verify(f => f.Update(It.Is<UserEntity>(data => data.ID == userEntity.ID)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void UpdateUserNotFound()
        {
            // Arrange
            var userID = 20;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var viewModel = MockDataGenerator.CreateUserViewModel(30);
            var userEntity = MockDataGenerator.CreateUserEntity(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.UpdateUser(userID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsException))]
        public void UpdateUserAlreadyExists()
        {
            // Arrange
            var userID = 5;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var viewModel = MockDataGenerator.CreateUserViewModel(2);
            var userEntity = MockDataGenerator.CreateUserEntity(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.UpdateUser(userID, viewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateUserException()
        {
            // Arrange
            var userID = 5;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var viewModel = MockDataGenerator.CreateUserViewModel(20);
            var userEntity = MockDataGenerator.CreateUserEntity(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockUserSet.Setup(f => f.Update(It.Is<UserEntity>(data => data.ID == userEntity.ID))).Throws(new Exception());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.UpdateUser(userID, viewModel);
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
            var userID = 5;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var userEntity = MockDataGenerator.CreateUserEntity(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockUserSet.Setup(f => f.Remove(It.Is<UserEntity>(data => data.ID == userEntity.ID)));
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.DeleteUserByID(userID);

            // Assert
            mockUserSet.Verify(f => f.Remove(It.Is<UserEntity>(data => data.ID == userEntity.ID)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void DeleteUserNotFound()
        {
            // Arrange
            var userID = 20;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var userEntity = MockDataGenerator.CreateUserEntity(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.DeleteUserByID(userID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteUserException()
        {
            // Arrange
            var userID = 5;
            var userEntities = MockDataGenerator.CreateUserEntities(10);
            var userEntitiesQueryable = userEntities.AsQueryable();
            var userEntity = MockDataGenerator.CreateUserEntity(userID);

            var mockDbContext = new Mock<DatabaseContext>();
            var mockUserSet = new Mock<DbSet<UserEntity>>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Provider).Returns(userEntitiesQueryable.Provider);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.Expression).Returns(userEntitiesQueryable.Expression);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.ElementType).Returns(userEntitiesQueryable.ElementType);
            mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator()); mockUserSet.As<IQueryable<UserEntity>>().Setup(m => m.GetEnumerator()).Returns(userEntitiesQueryable.GetEnumerator());
            mockUserSet.Setup(f => f.Remove(It.Is<UserEntity>(data => data.ID == userEntity.ID))).Throws(new Exception());
            mockDbContext.Setup(f => f.Users).Returns(mockUserSet.Object);

            var repository = new UserRepository(mockDbContext.Object, mockMapper.Object, mockLoanRepo.Object);

            // Act
            repository.DeleteUserByID(userID);
        }
    }
}
