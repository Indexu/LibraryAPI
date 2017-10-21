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
        /// Get all loans
        /// </summary>
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <param name="userID">Filter by user ID</param>
        /// <para />
        /// <param name="bookID">Filter by book ID</param>
        /// <para />
        /// <param name="date">Filter by loans that span over a specific date</param>
        /// <para />
        /// <param name="monthSpan">Filter by loans have been ongoing for more than a month 
        /// (can use the "date" parameter above)
        /// Defaults to the current date</param>
        /// <para />
        /// <returns>An Envelope of LoanDTO</returns>
        Envelope<LoanDTO> GetLoans(int pageNumber, int? pageMaxSize, int? userID, int? bookID, DateTime? date, bool? monthSpan);

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
        /// Get a loan by loan ID
        /// </summary>
        /// <param name="loanID">The ID of the loan</param>
        /// <para />
        /// <returns>A LoanDTO</returns>
        LoanDTO GetLoanByID(int loanID);

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

        // ===================================================================

        /// <summary>
        /// Add a loan
        /// </summary>
        /// <param name="loan">The LoanViewModel containing the loan information</param>
        /// <para />
        /// <returns>An integer representing the ID of the added loan</returns>
        int AddLoan(LoanViewModel loan);

        /// <summary>
        /// Update some (or all) of a loan's information
        /// </summary>
        /// <param name="loanID">The loan ID</param>
        /// <para />
        /// <param name="loan">The LoanViewModel containing new loan information</param>
        /// <para />
        /// <returns>void</returns>
        void UpdateLoan(int loanID, LoanViewModel loan);

        /// <summary>
        /// Delete a loan
        /// </summary>
        /// <param name="loanID">The loan ID</param>
        /// <para />
        /// <returns>void</returns>
        void DeleteLoanByID(int loanID);
    }
}