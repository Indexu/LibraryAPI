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
    public interface IRecommendationService
    {
        /// <summary>
        /// Get recommendations for a user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <para />
        /// <param name="pageNumber">The page number of the paging</param>
        /// <para />
        /// <param name="pageMaxSize">The maximum items on a single page</param>
        /// <para />
        /// <returns>An Envelope of RecommendationDTO</returns>
        Envelope<RecommendationDTO> GetRecommendationsByUserID(int userID, int pageNumber, int? pageMaxSize);
    }
}
