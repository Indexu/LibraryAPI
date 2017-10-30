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
    /// The unit tests for the BooksController
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [TestClass]
    public class BooksControllerTests
    {
        /* 
         * ===================
         *     GetBooks
         * ===================
         */

        [TestMethod]
        public void GetBooks()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.GetBooks(pageNumber, pageSize));

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBooks(pageNumber, pageSize) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.GetBooks(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetBooksReport()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            DateTime? loanDate = new DateTime(2000, 1, 1);
            int? duration = 30;

            var mockUserService = new Mock<IUserService>();
            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockRecommendationService = new Mock<IRecommendationService>();
            var mockMapper = new Mock<IMapper>();

            mockReportingService.Setup(f => f.GetBooksReport(pageNumber, pageSize, loanDate, duration));

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBooks(pageNumber, pageSize, loanDate, duration) as ObjectResult;

            // Assert
            mockReportingService.Verify(f => f.GetBooksReport(pageNumber, pageSize, loanDate, duration), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetBooksException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.GetBooks(pageNumber, pageSize)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBooks(pageNumber, pageSize) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.GetBooks(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     GetBook
         * ===================
         */

        [TestMethod]
        public void GetBook()
        {
            // Arrange
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.GetBookByID(bookID, pageNumber, pageSize)).Returns(new BookDetailsDTO());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBook(bookID, pageNumber, pageSize) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.GetBookByID(bookID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetBookNotFound()
        {
            // Arrange
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.GetBookByID(bookID, pageNumber, pageSize)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBook(bookID, pageNumber, pageSize) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.GetBookByID(bookID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void GetBookException()
        {
            // Arrange
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.GetBookByID(bookID, pageNumber, pageSize)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBook(bookID, pageNumber, pageSize) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.GetBookByID(bookID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *     AddBook
         * ===================
         */

        [TestMethod]
        public void AddBook()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);
            var DTO = MockDataGenerator.CreateBook(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.AddBook(viewModel)).Returns(DTO);

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddBook(viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.AddBook(viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 201);
        }

        [TestMethod]
        public void AddBookMissingValidation()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.AddBook(viewModel));

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.AddBook(viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void AddBookAlreadyExists()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.AddBook(viewModel)).Throws(new AlreadyExistsException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddBook(viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.AddBook(viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void AddBookException()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.AddBook(viewModel)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.AddBook(viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.AddBook(viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *    ReplaceBook
         * ===================
         */

        [TestMethod]
        public void ReplaceBook()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReplaceBook(bookID, viewModel));

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceBook(bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReplaceBook(bookID, viewModel), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReplaceBookInvalid()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.ReplaceBook(bookID, viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void ReplaceBookNotFound()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReplaceBook(bookID, viewModel)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceBook(bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReplaceBook(bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void ReplaceBookAlreadyExists()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReplaceBook(bookID, viewModel)).Throws(new AlreadyExistsException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceBook(bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReplaceBook(bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void ReplaceBookException()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.ReplaceBook(bookID, viewModel)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.ReplaceBook(bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.ReplaceBook(bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         * PartiallyUpdateBook
         * ===================
         */

        [TestMethod]
        public void PartiallyUpdateBook()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreatePatchBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.UpdateBook(bookID, viewModel));

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.PartiallyUpdateBook(bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.UpdateBook(bookID, viewModel), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void PartiallyUpdateBookInvalid()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreatePatchBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            controller.ModelState.AddModelError("fake error", "fake error");

            // Act
            var result = controller.PartiallyUpdateBook(bookID, viewModel) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 412);
        }

        [TestMethod]
        public void PartiallyUpdateBookNotFound()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreatePatchBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.UpdateBook(bookID, viewModel)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.PartiallyUpdateBook(bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.UpdateBook(bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void PartiallyUpdateBookAlreadyExists()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreatePatchBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.UpdateBook(bookID, viewModel)).Throws(new AlreadyExistsException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.PartiallyUpdateBook(bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.UpdateBook(bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 409);
        }

        [TestMethod]
        public void PartiallyUpdateBookException()
        {
            // Arrange
            var bookID = 1;
            var viewModel = MockDataGenerator.CreatePatchBookViewModel(bookID);

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.UpdateBook(bookID, viewModel)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.PartiallyUpdateBook(bookID, viewModel) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.UpdateBook(bookID, viewModel), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         * DeleteBook
         * ===================
         */

        [TestMethod]
        public void DeleteBook()
        {
            // Arrange
            var bookID = 1;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.DeleteBookByID(bookID));

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteBook(bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.DeleteBookByID(bookID), Times.Once());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteBookNotFound()
        {
            // Arrange
            var bookID = 1;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.DeleteBookByID(bookID)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteBook(bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.DeleteBookByID(bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void DeleteBookException()
        {
            // Arrange
            var bookID = 1;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockBookService.Setup(f => f.DeleteBookByID(bookID)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteBook(bookID) as ObjectResult;

            // Assert
            mockBookService.Verify(f => f.DeleteBookByID(bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetBookReviews(pageNumber, pageSize)).Returns(new Envelope<BookReviewsDTO>());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBookReviews(pageNumber, pageSize) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetBookReviews(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetBookReviewsException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetBookReviews(pageNumber, pageSize)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBookReviews(pageNumber, pageSize) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetBookReviews(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         * GetBookReviewsByID
         * ===================
         */

        [TestMethod]
        public void GetBookReviewsByID()
        {
            // Arrange
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize)).Returns(new Envelope<BookReviewDTO>());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBookReviewsByID(bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetBookReviewsByIDNotFound()
        {
            // Arrange
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBookReviewsByID(bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void GetBookReviewsByIDException()
        {
            // Arrange
            var bookID = 1;
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBookReviewsByID(bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReviewsByBookID(bookID, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }

        /* 
         * ===================
         *    GetBookReview
         * ===================
         */

        [TestMethod]
        public void GetBookReview()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReview(userID, bookID)).Returns(new ReviewDTO());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBookReview(bookID, userID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReview(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [TestMethod]
        public void GetBookReviewNotFound()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReview(userID, bookID)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBookReview(bookID, userID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.GetReview(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void GetBookReviewException()
        {
            // Arrange
            var userID = 1;
            var bookID = 1;

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.GetReview(userID, bookID)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.GetBookReview(bookID, userID) as ObjectResult;

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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.AddReview(userID, bookID, viewModel)).Returns(new ReviewDTO());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.AddReview(userID, bookID, viewModel)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.AddReview(userID, bookID, viewModel)).Throws(new AlreadyExistsException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.AddReview(userID, bookID, viewModel)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.ReplaceReview(userID, bookID, viewModel));

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.ReplaceReview(userID, bookID, viewModel)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.ReplaceReview(userID, bookID, viewModel)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.UpdateReview(userID, bookID, viewModel));

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.UpdateReview(userID, bookID, viewModel)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.UpdateReview(userID, bookID, viewModel)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.DeleteReview(userID, bookID));

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.DeleteReview(userID, bookID)).Throws(new NotFoundException());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
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

            var mockBookService = new Mock<IBookService>();
            var mockReportingService = new Mock<IReportingService>();
            var mockReviewService = new Mock<IReviewService>();
            var mockMapper = new Mock<IMapper>();

            mockReviewService.Setup(f => f.DeleteReview(userID, bookID)).Throws(new Exception());

            var controller = new BooksController(mockBookService.Object,
                                              mockReportingService.Object,
                                              mockReviewService.Object,
                                              mockMapper.Object);

            // Act
            var result = controller.DeleteReview(userID, bookID) as ObjectResult;

            // Assert
            mockReviewService.Verify(f => f.DeleteReview(userID, bookID), Times.Once());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }
    }
}
