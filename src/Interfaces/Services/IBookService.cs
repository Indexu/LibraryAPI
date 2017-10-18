using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models;

namespace LibraryAPI.Interfaces.Services
{
    /// <summary>
    /// An interface detailing the operations of the book service
    /// </summary>
    public interface IBookService
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
        /// Get all books in loan by user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of UserLoanDTO</returns>
        Envelope<UserLoanDTO> GetBooksInLoanByUserID(int userID, int pageNumber, int? pageMaxSize);

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
        /// Add a loan
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="bookID">The ID of the book</param>
        /// <para />
        /// <returns>An integer representing the ID of the added loan</returns>
        int AddLoan(int userID, int bookID);

        /// <summary>
        /// Return book
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="bookID">The ID of the book</param>
        /// <para />
        /// <returns>Nothing</returns>
        void ReturnBook(int userID, int bookID);

        /// <summary>
        /// Update some (or all) of a loan's information
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="bookID">The ID of the book</param>
        /// <para />
        /// <param name="loan">The PatchLoanViewModel containing new loan information</param>
        /// <para />
        /// <returns>void</returns>
        void UpdateLoan(int userID, int bookID, PatchLoanViewModel loan);

        /// <summary>
        /// Update some (or all) of a loan's information
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="bookID">The ID of the book</param>
        /// <para />
        /// <param name="loan">The LoanViewModel containing new loan information</param>
        /// <para />
        /// <returns>void</returns>
        void ReplaceLoan(int userID, int bookID, LoanViewModel loan);

        /// <summary>
        /// Replace a book's information
        /// </summary>
        /// <param name="bookID">The book ID</param>
        /// <para />
        /// <param name="book">The BookViewModel containing the new information</param>
        /// <para />
        /// <returns>void</returns>
        void ReplaceBook(int bookID, BookViewModel book);

        /// <summary>
        /// Update some (or all) of a book's information
        /// </summary>
        /// <param name="bookID">The book ID</param>
        /// <para />
        /// <param name="book">The PatchBookViewModel containing updated user information</param>
        /// <para />
        /// <returns>void</returns>
        void UpdateBook(int bookID, PatchBookViewModel book);

        /// <summary>
        /// Delete a book
        /// </summary>
        /// <param name="bookID">The book ID</param>
        /// <para />
        /// <returns>void</returns>
        void DeleteBookByID(int bookID);
    }
}
