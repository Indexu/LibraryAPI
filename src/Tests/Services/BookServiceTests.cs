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
    public class BookServiceTests
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
            int numberOfBooks = 10;

            var books = MockDataGenerator.CreateBooks(numberOfBooks);
            var envelope = MockDataGenerator.CreateBooksEnvelope(books, pageNumber, pageSize);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBooks(pageNumber, pageSize)).Returns(envelope);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedUsers = service.GetBooks(pageNumber, pageSize);

            // Assert
            mockBookRepo.Verify(f => f.GetBooks(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedUsers);
            Assert.AreEqual(returnedUsers.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedUsers.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedUsers.Paging.PageCount, 1);
            Assert.AreEqual(returnedUsers.Paging.TotalNumberOfItems, numberOfBooks);
            Assert.AreEqual(returnedUsers.Items.First().ID, books.First().ID);
            Assert.AreEqual(returnedUsers.Items.Last().ID, books.Last().ID);
        }

        [TestMethod]
        public void GetBooks2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            int numberOfBooks = 10;

            var books = MockDataGenerator.CreateBooks(numberOfBooks);
            var envelope = MockDataGenerator.CreateBooksEnvelope(books, pageNumber, pageSize);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBooks(pageNumber, pageSize)).Returns(envelope);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedUsers = service.GetBooks(pageNumber, pageSize);

            // Assert
            mockBookRepo.Verify(f => f.GetBooks(pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedUsers);
            Assert.AreEqual(returnedUsers.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedUsers.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedUsers.Paging.PageCount, 2);
            Assert.AreEqual(returnedUsers.Paging.TotalNumberOfItems, numberOfBooks);
            Assert.AreEqual(returnedUsers.Items.First().ID, books.First().ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBooksException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBooks(pageNumber, pageSize)).Throws(new Exception());

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.GetBooks(pageNumber, pageSize);
        }

        /* 
         * ===================
         * GetBooksInLoanByUserID
         * ===================
         */

        [TestMethod]
        public void GetBooksInLoanByUserIDBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            int numberOfLoans = 10;
            int userID = 1;

            var userLoans = MockDataGenerator.CreateUserLoans(numberOfLoans);
            var envelope = MockDataGenerator.CreateUsersLoansEnvelope(userLoans, pageNumber, pageSize);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.GetLoansByUserID(userID, true, pageNumber, pageSize)).Returns(envelope);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedLoans = service.GetBooksInLoanByUserID(userID, pageNumber, pageSize);

            // Assert
            mockLoanRepo.Verify(f => f.GetLoansByUserID(userID, true, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedLoans);
            Assert.AreEqual(returnedLoans.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedLoans.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedLoans.Paging.PageCount, 1);
            Assert.AreEqual(returnedLoans.Paging.TotalNumberOfItems, numberOfLoans);
            Assert.AreEqual(returnedLoans.Items.First().Book.ID, userLoans.First().Book.ID);
            Assert.AreEqual(returnedLoans.Items.Last().Book.ID, userLoans.Last().Book.ID);
        }

        [TestMethod]
        public void GetBooksInLoanByUserID2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            int numberOfLoans = 10;
            int userID = 1;

            var userLoans = MockDataGenerator.CreateUserLoans(numberOfLoans);
            var envelope = MockDataGenerator.CreateUsersLoansEnvelope(userLoans, pageNumber, pageSize);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.GetLoansByUserID(userID, true, pageNumber, pageSize)).Returns(envelope);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedLoans = service.GetBooksInLoanByUserID(userID, pageNumber, pageSize);

            // Assert
            mockLoanRepo.Verify(f => f.GetLoansByUserID(userID, true, pageNumber, pageSize), Times.Once());
            Assert.IsNotNull(returnedLoans);
            Assert.AreEqual(returnedLoans.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedLoans.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedLoans.Paging.PageCount, 2);
            Assert.AreEqual(returnedLoans.Paging.TotalNumberOfItems, numberOfLoans);
            Assert.AreEqual(returnedLoans.Items.First().Book.ID, userLoans.First().Book.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBooksInLoanByUserIDException()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            int userID = 1;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.GetLoansByUserID(userID, true, pageNumber, pageSize)).Throws(new Exception());

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.GetBooksInLoanByUserID(userID, pageNumber, pageSize);
        }

        /* 
         * ===================
         *    GetBookByID
         * ===================
         */

        [TestMethod]
        public void GetBookByIDBasic()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            int numberOfLoans = 10;
            int bookID = 1;

            var bookDTO = MockDataGenerator.CreateBook(bookID);
            var bookDetailsBasicDTO = MockDataGenerator.CreateBasicBookDetails(bookID);
            var bookLoans = MockDataGenerator.CreateBookLoans(numberOfLoans);
            var envelope = MockDataGenerator.CreateBookLoansEnvelope(bookLoans, pageNumber, pageSize);
            var bookDetailsDTO = MockDataGenerator.CreateBasicBookDetails(bookID);
            bookDetailsDTO.LoanHistory = envelope;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBookByID(bookID)).Returns(bookDTO);
            mockLoanRepo.Setup(f => f.GetLoansByBookID(bookID, false, pageNumber, pageSize)).Returns(envelope);
            mockMapper.Setup(x => x.Map<BookDTO, BookDetailsDTO>(bookDTO)).Returns(bookDetailsBasicDTO);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedBook = service.GetBookByID(bookID, pageNumber, pageSize);

            // Assert
            mockBookRepo.Verify(f => f.GetBookByID(bookID), Times.Once());
            mockLoanRepo.Verify(f => f.GetLoansByBookID(bookID, false, pageNumber, pageSize), Times.Once());
            mockMapper.Verify(f => f.Map<BookDTO, BookDetailsDTO>(bookDTO), Times.Once());
            Assert.IsNotNull(returnedBook);
            Assert.AreEqual(returnedBook.ID, bookID);
            Assert.AreEqual(returnedBook.LoanHistory.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedBook.LoanHistory.Paging.PageMaxSize, 50);
            Assert.AreEqual(returnedBook.LoanHistory.Paging.PageCount, 1);
            Assert.AreEqual(returnedBook.LoanHistory.Paging.TotalNumberOfItems, numberOfLoans);
            Assert.AreEqual(returnedBook.LoanHistory.Items.First().User.ID, bookLoans.First().User.ID);
            Assert.AreEqual(returnedBook.LoanHistory.Items.Last().User.ID, bookLoans.Last().User.ID);
        }

        [TestMethod]
        public void GetBookByIDID2Pages()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = 5;
            int numberOfLoans = 10;
            int bookID = 1;

            var bookDTO = MockDataGenerator.CreateBook(bookID);
            var bookDetailsBasicDTO = MockDataGenerator.CreateBasicBookDetails(bookID);
            var bookLoans = MockDataGenerator.CreateBookLoans(numberOfLoans);
            var envelope = MockDataGenerator.CreateBookLoansEnvelope(bookLoans, pageNumber, pageSize);
            var bookDetailsDTO = MockDataGenerator.CreateBasicBookDetails(bookID);
            bookDetailsDTO.LoanHistory = envelope;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBookByID(bookID)).Returns(bookDTO);
            mockLoanRepo.Setup(f => f.GetLoansByBookID(bookID, false, pageNumber, pageSize)).Returns(envelope);
            mockMapper.Setup(x => x.Map<BookDTO, BookDetailsDTO>(bookDTO)).Returns(bookDetailsBasicDTO);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedBook = service.GetBookByID(bookID, pageNumber, pageSize);

            // Assert
            mockBookRepo.Verify(f => f.GetBookByID(bookID), Times.Once());
            mockLoanRepo.Verify(f => f.GetLoansByBookID(bookID, false, pageNumber, pageSize), Times.Once());
            mockMapper.Verify(f => f.Map<BookDTO, BookDetailsDTO>(bookDTO), Times.Once());
            Assert.IsNotNull(returnedBook);
            Assert.AreEqual(returnedBook.ID, bookID);
            Assert.AreEqual(returnedBook.LoanHistory.Paging.PageNumber, pageNumber);
            Assert.AreEqual(returnedBook.LoanHistory.Paging.PageMaxSize, pageSize);
            Assert.AreEqual(returnedBook.LoanHistory.Paging.PageCount, 2);
            Assert.AreEqual(returnedBook.LoanHistory.Paging.TotalNumberOfItems, numberOfLoans);
            Assert.AreEqual(returnedBook.LoanHistory.Items.First().User.ID, bookLoans.First().User.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBookByIDExceptionBookRepo()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            int numberOfLoans = 10;
            int bookID = 1;

            var bookDTO = MockDataGenerator.CreateBook(bookID);
            var bookDetailsBasicDTO = MockDataGenerator.CreateBasicBookDetails(bookID);
            var bookLoans = MockDataGenerator.CreateBookLoans(numberOfLoans);
            var envelope = MockDataGenerator.CreateBookLoansEnvelope(bookLoans, pageNumber, pageSize);
            var bookDetailsDTO = MockDataGenerator.CreateBasicBookDetails(bookID);
            bookDetailsDTO.LoanHistory = envelope;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBookByID(bookID)).Throws(new Exception());
            mockLoanRepo.Setup(f => f.GetLoansByBookID(bookID, false, pageNumber, pageSize)).Returns(envelope);
            mockMapper.Setup(x => x.Map<BookDTO, BookDetailsDTO>(bookDTO)).Returns(bookDetailsBasicDTO);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.GetBookByID(bookID, pageNumber, pageSize);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetBookByIDExceptionLoanRepo()
        {
            // Arrange
            var pageNumber = 1;
            int? pageSize = null;
            int bookID = 1;

            var bookDTO = MockDataGenerator.CreateBook(bookID);
            var bookDetailsBasicDTO = MockDataGenerator.CreateBasicBookDetails(bookID);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBookByID(bookID)).Returns(bookDTO);
            mockLoanRepo.Setup(f => f.GetLoansByBookID(bookID, false, pageNumber, pageSize)).Throws(new Exception());
            mockMapper.Setup(x => x.Map<BookDTO, BookDetailsDTO>(bookDTO)).Returns(bookDetailsBasicDTO);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.GetBookByID(bookID, pageNumber, pageSize);
        }

        /* 
         * ===================
         *       AddBook
         * ===================
         */

        [TestMethod]
        public void AddBook()
        {
            // Arrange
            int bookID = 1;

            var bookViewModel = MockDataGenerator.CreateBookViewModel(bookID);
            var bookDTO = MockDataGenerator.CreateBook(bookID);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.AddBook(bookViewModel)).Returns(bookDTO);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedBook = service.AddBook(bookViewModel);

            // Assert
            mockBookRepo.Verify(f => f.AddBook(bookViewModel), Times.Once());
            Assert.IsNotNull(returnedBook);
            Assert.AreEqual(returnedBook.Title, bookDTO.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddBookException()
        {
            // Arrange
            int bookID = 1;

            var bookViewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.AddBook(bookViewModel)).Throws(new Exception());

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.AddBook(bookViewModel);
        }

        /* 
         * ===================
         *       AddLoan
         * ===================
         */

        [TestMethod]
        public void AddLoan()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.AddLoan(userID, bookID)).Returns(1);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            var returnedID = service.AddLoan(userID, bookID);

            // Assert
            mockLoanRepo.Verify(f => f.AddLoan(userID, bookID), Times.Once());
            Assert.IsNotNull(returnedID);
            Assert.AreEqual(returnedID, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddLoanException()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.AddLoan(userID, bookID)).Throws(new Exception());

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.AddLoan(userID, bookID);
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
            int userID = 1;
            int bookID = 1;
            var loanDate = new DateTime(2000, 1, 1);
            DateTime? returnDate = null;
            var patchViewModel = MockDataGenerator.CreatePatchLoanViewModel(loanDate, returnDate);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.UpdateLoan(userID, bookID, patchViewModel));

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.UpdateLoan(userID, bookID, patchViewModel);

            // Assert
            mockLoanRepo.Verify(f => f.UpdateLoan(userID, bookID, patchViewModel), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateLoanException()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;
            var loanDate = new DateTime(2000, 1, 1);
            DateTime? returnDate = null;
            var patchViewModel = MockDataGenerator.CreatePatchLoanViewModel(loanDate, returnDate);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.UpdateLoan(userID, bookID, patchViewModel)).Throws(new Exception());

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.UpdateLoan(userID, bookID, patchViewModel);
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
            int userID = 1;
            int bookID = 1;
            var loanDate = new DateTime(2000, 1, 1);
            DateTime? returnDate = null;
            var patchViewModel = MockDataGenerator.CreatePatchLoanViewModel(loanDate, returnDate);
            var viewModel = MockDataGenerator.CreateLoanViewModel(loanDate, returnDate);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.UpdateLoan(userID, bookID, patchViewModel));
            mockMapper.Setup(x => x.Map<LoanViewModel, PatchLoanViewModel>(viewModel)).Returns(patchViewModel);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.ReplaceLoan(userID, bookID, viewModel);

            // Assert
            mockLoanRepo.Verify(f => f.UpdateLoan(userID, bookID, patchViewModel), Times.Once());
            mockMapper.Verify(f => f.Map<LoanViewModel, PatchLoanViewModel>(viewModel), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReplaceLoanException()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;
            var loanDate = new DateTime(2000, 1, 1);
            DateTime? returnDate = null;
            var patchViewModel = MockDataGenerator.CreatePatchLoanViewModel(loanDate, returnDate);
            var viewModel = MockDataGenerator.CreateLoanViewModel(loanDate, returnDate);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.UpdateLoan(userID, bookID, patchViewModel)).Throws(new Exception());
            mockMapper.Setup(x => x.Map<LoanViewModel, PatchLoanViewModel>(viewModel)).Returns(patchViewModel);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.ReplaceLoan(userID, bookID, viewModel);
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
            int userID = 1;
            int bookID = 1;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.ReturnBook(userID, bookID));

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.ReturnBook(userID, bookID);

            // Assert
            mockLoanRepo.Verify(f => f.ReturnBook(userID, bookID), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReturnBookException()
        {
            // Arrange
            int userID = 1;
            int bookID = 1;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockLoanRepo.Setup(f => f.ReturnBook(userID, bookID)).Throws(new Exception());

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.ReturnBook(userID, bookID);
        }

        /* 
         * ===================
         *     ReplaceBook
         * ===================
         */

        [TestMethod]
        public void ReplaceBook()
        {
            // Arrange
            int bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.UpdateBook(bookID, viewModel));

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.ReplaceBook(bookID, viewModel);

            // Assert
            mockBookRepo.Verify(f => f.UpdateBook(bookID, viewModel), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReplaceBookException()
        {
            // Arrange
            int bookID = 1;
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.UpdateBook(bookID, viewModel)).Throws(new Exception());

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.ReplaceBook(bookID, viewModel);
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
            int bookID = 1;
            var patchViewModel = MockDataGenerator.CreatePatchBookViewModel(bookID);
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);
            var oldDTO = MockDataGenerator.CreateBook(bookID);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBookByID(bookID)).Returns(oldDTO);
            mockBookRepo.Setup(f => f.UpdateBook(bookID, viewModel));
            mockMapper.Setup(x => x.Map<PatchBookViewModel, BookViewModel>(patchViewModel)).Returns(viewModel);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.UpdateBook(bookID, patchViewModel);

            // Assert
            mockBookRepo.Verify(f => f.GetBookByID(bookID), Times.Once());
            mockBookRepo.Verify(f => f.UpdateBook(bookID, viewModel), Times.Once());
            mockMapper.Verify(f => f.Map<PatchBookViewModel, BookViewModel>(patchViewModel), Times.Once());
        }

        [TestMethod]
        public void UpdateBookEmpty()
        {
            // Arrange
            int bookID = 1;
            var patchViewModel = new PatchBookViewModel();
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);
            var oldDTO = MockDataGenerator.CreateBook(bookID);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBookByID(bookID)).Returns(oldDTO);
            mockBookRepo.Setup(f => f.UpdateBook(bookID, viewModel));
            mockMapper.Setup(x => x.Map<PatchBookViewModel, BookViewModel>(patchViewModel)).Returns(viewModel);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.UpdateBook(bookID, patchViewModel);

            // Assert
            mockBookRepo.Verify(f => f.GetBookByID(bookID), Times.Once());
            mockBookRepo.Verify(f => f.UpdateBook(bookID, viewModel), Times.Once());
            mockMapper.Verify(f => f.Map<PatchBookViewModel, BookViewModel>(patchViewModel), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateBookExceptionGet()
        {
            // Arrange
            int bookID = 1;
            var patchViewModel = MockDataGenerator.CreatePatchBookViewModel(bookID);
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBookByID(bookID)).Throws(new Exception());
            mockBookRepo.Setup(f => f.UpdateBook(bookID, viewModel));
            mockMapper.Setup(x => x.Map<PatchBookViewModel, BookViewModel>(patchViewModel)).Returns(viewModel);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.UpdateBook(bookID, patchViewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateBookExceptionUpdate()
        {
            // Arrange
            int bookID = 1;
            var patchViewModel = MockDataGenerator.CreatePatchBookViewModel(bookID);
            var viewModel = MockDataGenerator.CreateBookViewModel(bookID);
            var oldDTO = MockDataGenerator.CreateBook(bookID);

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.GetBookByID(bookID)).Returns(oldDTO);
            mockBookRepo.Setup(f => f.UpdateBook(bookID, viewModel)).Throws(new Exception());
            mockMapper.Setup(x => x.Map<PatchBookViewModel, BookViewModel>(patchViewModel)).Returns(viewModel);

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.UpdateBook(bookID, patchViewModel);
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
            int bookID = 1;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.DeleteBookByID(bookID));

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.DeleteBookByID(bookID);

            // Assert
            mockBookRepo.Verify(f => f.DeleteBookByID(bookID), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteBookException()
        {
            // Arrange
            int bookID = 1;

            var mockBookRepo = new Mock<IBookRepository>();
            var mockLoanRepo = new Mock<ILoanRepository>();
            var mockMapper = new Mock<IMapper>();

            mockBookRepo.Setup(f => f.DeleteBookByID(bookID)).Throws(new Exception());

            var service = new BookService(mockBookRepo.Object, mockLoanRepo.Object, mockMapper.Object);

            // Act
            service.DeleteBookByID(bookID);
        }
    }
}