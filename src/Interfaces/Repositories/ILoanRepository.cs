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
        /// Get all loans that are currently ongoing
        /// </summary>
        /// <param name="userID">Filter by user ID</param>
        /// <para />
        /// <returns>An IEnumerable of LoanDTO</returns>
        IEnumerable<LoanDTO> GetCurrentLoans(int? userID);

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