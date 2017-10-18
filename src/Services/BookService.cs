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
        private readonly IMapper mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            this.bookRepository = bookRepository;
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

        public BookDTO GetBookByID(int bookID)
        {
            try
            {
                return bookRepository.GetBookByID(bookID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddBook(BookViewModel book)
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
