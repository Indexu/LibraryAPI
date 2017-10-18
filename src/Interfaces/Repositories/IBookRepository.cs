using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models;

namespace LibraryAPI.Interfaces.Repositories
{
    /// <summary>
    /// An interface detailing the book repository
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Get all books
        /// </summary>
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of BookDTO</returns>
        Envelope<BookDTO> GetBooks(int pageNumber, int? pageMaxSize);

        /// <summary>
        /// Get a book by book ID
        /// </summary>
        /// <param name="bookID">The ID of the book</param>
        /// <para />
        /// <returns>A BookDTO</returns>
        BookDTO GetBookByID(int bookID);

        /// <summary>
        /// Add a book
        /// </summary>
        /// <param name="book">The BookViewModel containing the book information</param>
        /// <para />
        /// <returns>An integer representing the ID of the added book</returns>
        int AddBook(BookViewModel book);

        /// <summary>
        /// Update some (or all) of a book's information
        /// </summary>
        /// <param name="bookID">The book ID</param>
        /// <para />
        /// <param name="book">The BookViewModel containing new book information</param>
        /// <para />
        /// <returns>void</returns>
        void UpdateBook(int bookID, BookViewModel book);

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="bookID">The book ID</param>
        /// <para />
        /// <returns>void</returns>
        void DeleteBookByID(int bookID);
    }
}