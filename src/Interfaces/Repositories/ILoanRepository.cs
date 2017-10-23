using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models;
using System;

namespace LibraryAPI.Interfaces.Repositories
{
    /// <summary>
    /// An interface detailing the loan repository
    /// </summary>
    public interface ILoanRepository
    {
        /// <summary>
        /// Get all books in loan by user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="active">Whether or not the loans must be currently active</param>
        /// <para />
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of UserLoanDTO</returns>
        Envelope<UserLoanDTO> GetLoansByUserID(int userID, bool active, int pageNumber, int? pageMaxSize);

        /// <summary>
        /// Get all loans for a book
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="active">Whether or not the loans must be currently active</param>
        /// <para />
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of BookLoanDTO</returns>
        Envelope<BookLoanDTO> GetLoansByBookID(int bookID, bool active, int pageNumber, int? pageMaxSize);

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
        /// Return a book
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
        /// Get report for users
        /// </summary>
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <param name="loanDate">Filter by loans that span over a specific date. Defaults to today</param>
        /// <para />
        /// <param name="duration">Filter by loans have been ongoing for a specific duration</param>
        /// <para />
        /// <returns>An Envelope of UserReportDTO</returns>
        Envelope<UserReportDTO> GetUsersReport(int pageNumber, int? pageMaxSize, DateTime? loanDate, int? duration);

        /// <summary>
        /// Get report for books
        /// </summary>
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <param name="loanDate">Filter by loans that span over a specific date. Defaults to today</param>
        /// <para />
        /// <param name="duration">Filter by loans have been ongoing for a specific duration</param>
        /// <para />
        /// <returns>An Envelope of BookReportDTO</returns>
        Envelope<BookReportDTO> GetBooksReport(int pageNumber, int? pageMaxSize, DateTime? loanDate, int? duration);
    }
}