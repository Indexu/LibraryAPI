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
    /// <summary>
    /// An implementation of the recommendation service
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    public class RecommendationService : AbstractService, IRecommendationService
    {
        private readonly IReviewRepository reviewRepository;

        public RecommendationService(IReviewRepository reviewRepository, IMapper mapper)
            : base(mapper)
        {
            this.reviewRepository = reviewRepository;
        }

        public Envelope<RecommendationDTO> GetRecommendationsByUserID(int userID, int pageNumber, int? pageMaxSize)
        {
            try
            {
                return reviewRepository.GetRecommendationsByUserID(userID, pageNumber, pageMaxSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
