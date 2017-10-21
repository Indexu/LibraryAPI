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
