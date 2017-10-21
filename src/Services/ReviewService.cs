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
    public class ReviewService : AbstractService, IReviewService
    {
        private readonly IReviewRepository reviewRepository;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
            : base(mapper)
        {
            this.reviewRepository = reviewRepository;
        }

        public Envelope<BookReviewsDTO> GetBookReviews(int pageNumber, int? pageMaxSize)
        {
            try
            {
                return reviewRepository.GetBookReviews(pageNumber, pageMaxSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Envelope<UserReviewDTO> GetReviewsByUserID(int userID, int pageNumber, int? pageMaxSize)
        {
            try
            {
                return reviewRepository.GetReviewsByUserID(userID, pageNumber, pageMaxSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Envelope<BookReviewDTO> GetReviewsByBookID(int bookID, int pageNumber, int? pageMaxSize)
        {
            try
            {
                return reviewRepository.GetReviewsByBookID(bookID, pageNumber, pageMaxSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReviewDTO GetReview(int userID, int bookID)
        {
            try
            {
                return reviewRepository.GetReview(userID, bookID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReviewDTO AddReview(int userID, int bookID, ReviewViewModel review)
        {
            try
            {
                return reviewRepository.AddReview(userID, bookID, review);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReplaceReview(int userID, int bookID, ReviewViewModel review)
        {
            try
            {
                var newReview = mapper.Map<ReviewViewModel, PatchReviewViewModel>(review);
                reviewRepository.UpdateReview(userID, bookID, newReview);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateReview(int userID, int bookID, PatchReviewViewModel review)
        {
            try
            {
                reviewRepository.UpdateReview(userID, bookID, review);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteReview(int userID, int bookID)
        {
            try
            {
                reviewRepository.DeleteReview(userID, bookID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
