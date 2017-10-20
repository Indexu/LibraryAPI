using System;
using System.Collections.Generic;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Interfaces.Services;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using AutoMapper;

namespace LibraryAPI.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository bookRepository;
        private readonly ILoanRepository loanRepository;
        private readonly IMapper mapper;

        public BookService(IBookRepository bookRepository, ILoanRepository loanRepository, IMapper mapper)
        {
            this.bookRepository = bookRepository;
            this.loanRepository = loanRepository;
            this.mapper = mapper;
        }

        public Envelope<BookDTO> GetBooks(int pageNumber, int? pageMaxSize)
        {
            try
            {
                return bookRepository.GetBooks(pageNumber, pageMaxSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Envelope<UserLoanDTO> GetBooksInLoanByUserID(int userID, int pageNumber, int? pageMaxSize)
        {
            try
            {
                return loanRepository.GetLoansByUserID(userID, true, pageNumber, pageMaxSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BookDetailsDTO GetBookByID(int bookID, int pageNumber, int? pageMaxSize)
        {
            try
            {
                var book = bookRepository.GetBookByID(bookID);

                var details = mapper.Map<BookDTO, BookDetailsDTO>(book);

                details.LoanHistory = loanRepository.GetLoansByBookID(bookID, false, pageNumber, pageMaxSize);

                return details;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BookDTO AddBook(BookViewModel book)
        {
            try
            {
                return bookRepository.AddBook(book);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddLoan(int userID, int bookID)
        {
            try
            {
                return loanRepository.AddLoan(userID, bookID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLoan(int userID, int bookID, PatchLoanViewModel loan)
        {
            try
            {
                loanRepository.UpdateLoan(userID, bookID, loan);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReplaceLoan(int userID, int bookID, LoanViewModel loan)
        {
            try
            {
                var mappedLoan = mapper.Map<LoanViewModel, PatchLoanViewModel>(loan);

                loanRepository.UpdateLoan(userID, bookID, mappedLoan);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReturnBook(int userID, int bookID)
        {
            try
            {
                loanRepository.ReturnBook(userID, bookID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReplaceBook(int bookID, BookViewModel book)
        {
            try
            {
                bookRepository.UpdateBook(bookID, book);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBook(int bookID, PatchBookViewModel book)
        {
            try
            {
                var oldBook = bookRepository.GetBookByID(bookID);

                MergeBooksForPatch(book, oldBook);

                var newBook = mapper.Map<PatchBookViewModel, BookViewModel>(book);

                bookRepository.UpdateBook(bookID, newBook);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteBookByID(int bookID)
        {
            try
            {
                bookRepository.DeleteBookByID(bookID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MergeBooksForPatch(PatchBookViewModel book, BookDTO oldBook)
        {
            if (string.IsNullOrEmpty(book.Title))
            {
                book.Title = oldBook.Title;
            }

            if (string.IsNullOrEmpty(book.Author))
            {
                book.Author = oldBook.Author;
            }

            if (string.IsNullOrEmpty(book.ISBN))
            {
                book.ISBN = oldBook.ISBN;
            }

            if (!book.PublishDate.HasValue)
            {
                book.PublishDate = oldBook.PublishDate;
            }
        }
    }
}
