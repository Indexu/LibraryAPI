using System.Collections.Generic;
using System.Linq;
using LibraryAPI.Interfaces.Repositories;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.EntityModels;
using LibraryAPI.Models;
using LibraryAPI.Exceptions;
using AutoMapper;
using System;

namespace LibraryAPI.Repositories.EntityFrameworkCore
{
    public class BookRepository : AbstractRepository, IBookRepository
    {
        private const string notFoundMessage = "Book not found";
        private const string alreadyExistsMessage = "Book with that ISBN already exists";

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

            var bookDTOs = Mapper.Map<IList<BookEntity>, IList<BookDTO>>(bookEntities);

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
                throw new NotFoundException(notFoundMessage);
            }

            var bookDTO = Mapper.Map<BookEntity, BookDTO>(bookEntity);

            return bookDTO;
        }

        public int AddBook(BookViewModel book)
        {
            // Check if exists by ISBN
            if (db.Books.Where(b => b.ISBN == book.ISBN).Any())
            {
                throw new AlreadyExistsException(alreadyExistsMessage);
            }

            // Add book
            var bookEntity = mapper.Map<BookViewModel, BookEntity>(book);

            db.Books.Add(bookEntity);
            db.SaveChanges();

            return bookEntity.ID;
        }

        public void UpdateBook(int bookID, BookViewModel book)
        {
            // Get entity from DB
            var bookEntity = db.Books.Where(b => b.ID == bookID).SingleOrDefault();

            if (bookEntity == null)
            {
                throw new NotFoundException(notFoundMessage);
            }

            // Check if ISBN change and make sure ISBN is still unique
            if (bookEntity.ISBN != book.ISBN && db.Books.Where(b => b.ISBN == book.ISBN).Any())
            {
                throw new AlreadyExistsException(alreadyExistsMessage);
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
                throw new NotFoundException(notFoundMessage);
            }

            db.Books.Remove(bookEntity);
            db.SaveChanges();
        }
    }
}