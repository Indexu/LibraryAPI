using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models;
using LibraryAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    /// <summary>
    /// An implementation of the book repository using Entity Framework Core
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class BookRepository : AbstractRepository, IBookRepository
    {
        public BookRepository(DatabaseContext db, IMapper mapper)
            : base(db, mapper)
        {
        }

        public Envelope<BookDTO> GetBooks(int pageNumber, int? pageMaxSize)
        {
            var maxSize = (pageMaxSize.HasValue ? pageMaxSize.Value : defaultPageSize);
            var totalNumberOfItems = db.Books.Count();
            var pageCount = (int)Math.Ceiling(totalNumberOfItems / (double)maxSize);

            var bookEntities = db.Books.Skip((pageNumber - 1) * maxSize).Take(maxSize).ToList();

            var bookDTOs = mapper.Map<IList<BookEntity>, IList<BookDTO>>(bookEntities);

            return new Envelope<BookDTO>
            {
                Items = bookDTOs,
                Paging = new Paging
                {
                    PageCount = pageCount,
                    PageSize = bookDTOs.Count,
                    PageMaxSize = maxSize,
                    PageNumber = pageNumber,
                    TotalNumberOfItems = totalNumberOfItems,
                }
            };
        }

        public BookDTO GetBookByID(int bookID)
        {
            var bookEntity = db.Books.Where(b => b.ID == bookID).SingleOrDefault();

            if (bookEntity == null)
            {
                throw new NotFoundException(bookNotFoundMessage);
            }

            var bookDTO = mapper.Map<BookEntity, BookDTO>(bookEntity);

            return bookDTO;
        }

        public BookDTO AddBook(BookViewModel book)
        {
            // Check if exists by ISBN
            if (db.Books.Where(b => b.ISBN == book.ISBN).Any())
            {
                throw new AlreadyExistsException(bookAlreadyExistsMessage);
            }

            // Add book
            var bookEntity = mapper.Map<BookViewModel, BookEntity>(book);

            db.Books.Add(bookEntity);
            db.SaveChanges();

            var bookDTO = mapper.Map<BookEntity, BookDTO>(bookEntity);

            return bookDTO;
        }

        public void UpdateBook(int bookID, BookViewModel book)
        {
            // Get entity from DB
            var bookEntity = db.Books.Where(b => b.ID == bookID).SingleOrDefault();

            if (bookEntity == null)
            {
                throw new NotFoundException(bookNotFoundMessage);
            }

            // Check if ISBN change and make sure ISBN is still unique
            if (bookEntity.ISBN != book.ISBN && db.Books.Where(b => b.ISBN == book.ISBN).Any())
            {
                throw new AlreadyExistsException(bookAlreadyExistsMessage);
            }

            bookEntity.Title = book.Title;
            bookEntity.Author = book.Author;
            bookEntity.PublishDate = book.PublishDate.Value;
            bookEntity.ISBN = book.ISBN;

            db.Books.Update(bookEntity);
            db.SaveChanges();
        }

        public void DeleteBookByID(int bookID)
        {
            var bookEntity = db.Books.Where(b => b.ID == bookID).SingleOrDefault();

            if (bookEntity == null)
            {
                throw new NotFoundException(bookNotFoundMessage);
            }

            db.Books.Remove(bookEntity);
            db.SaveChanges();
        }
    }
}