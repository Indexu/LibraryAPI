using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models;
using System;

namespace LibraryAPI.Interfaces.Services
{
    /// <summary>
    /// An interface detailing the operations of the recommendation service
    /// </summary>
    public interface IReportingService
    {
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
