using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Interfaces.Services;
using LibraryAPI.Models.ViewModels;
using LibraryAPI.Models.DTOModels;
using LibraryAPI.Exceptions;

using AutoMapper;

namespace LibraryAPI.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IBookService bookService;
        private readonly IReportingService reportingService;
        private readonly IReviewService reviewService;
        private readonly IRecommendationService recommendationService;
        private readonly IMapper mapper;

        public UsersController(IUserService userService,
                               IBookService bookService,
                               IReportingService reportingService,
                               IReviewService reviewService,
                               IRecommendationService recommendationService,
                               IMapper mapper)
        {
            this.userService = userService;
            this.bookService = bookService;
            this.reportingService = reportingService;
            this.reviewService = reviewService;
            this.recommendationService = recommendationService;
            this.mapper = mapper;
        }

        /* 
         * =============================
         *             User
         * =============================
         */

        // GET api/v1/users
        [HttpGet]
        public IActionResult GetUsers([FromQuery] int pageNumber = 1,
                                      [FromQuery] int? pageSize = null,
                                      [FromQuery] DateTime? loanDate = null,
                                      [FromQuery] int? duration = null)
        {
            try
            {
                // Get all users
                if (!loanDate.HasValue && !duration.HasValue)
                {
                    var users = userService.GetUsers(pageNumber, pageSize);
                    return Ok(users);
                }
                // Get report for users
                else
                {
                    var report = reportingService.GetUsersReport(pageNumber, pageSize, loanDate, duration);
                    return Ok(report);
                }
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // GET api/v1/users/{userID}
        [HttpGet("{userID}", Name = "GetUserByID")]
        public IActionResult GetUser(int userID, [FromQuery] int pageNumber = 1, [FromQuery] int? pageSize = null)
        {
            try
            {
                var user = userService.GetUserByID(userID, pageNumber, pageSize);

                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // POST api/v1/users
        [HttpPost]
        public IActionResult AddUser([FromBody] UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
                    Message = "Validation error. Invalid input"
                };

                return StatusCode(error.Code, error);
            }

            try
            {
                var createdUser = userService.AddUser(user);

                return CreatedAtRoute("GetUserByID", new { userID = createdUser.ID }, createdUser);
            }
            catch (AlreadyExistsException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 409,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // PUT api/v1/users/{userID}
        [HttpPut("{userID}")]
        public IActionResult ReplaceUser(int userID, [FromBody] UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
                    Message = "Validation error. Invalid input"
                };

                return StatusCode(error.Code, error);
            }

            try
            {
                userService.ReplaceUser(userID, user);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (AlreadyExistsException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 409,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // PATCH api/v1/users/{userID}
        [HttpPatch("{userID}")]
        public IActionResult PartiallyUpdateUser(int userID, [FromBody] PatchUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
                    Message = "Validation error. Invalid input"
                };

                return StatusCode(error.Code, error);
            }

            try
            {
                userService.UpdateUser(userID, user);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (AlreadyExistsException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 409,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // DELETE api/v1/users/{userID}
        [HttpDelete("{userID}")]
        public IActionResult DeleteUser(int userID)
        {
            try
            {
                userService.DeleteUserByID(userID);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        /* 
         * =============================
         *             Loans
         * =============================
         */

        // GET api/v1/users/{userID}/books
        [HttpGet("{userID}/books", Name = "GetUserBooks")]
        public IActionResult GetUserBooks(int userID, [FromQuery] int pageNumber = 1, [FromQuery] int? pageSize = null)
        {
            try
            {
                var books = bookService.GetBooksInLoanByUserID(userID, pageNumber, pageSize);

                return Ok(books);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // POST api/v1/users/{userID}/books/{bookID}
        [HttpPost("{userID}/books/{bookID}")]
        public IActionResult LoanBook(int userID, int bookID)
        {
            try
            {
                bookService.AddLoan(userID, bookID);

                return CreatedAtRoute("GetUserBooks", new { userID = userID }, null);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (AlreadyExistsException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 409,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // DELETE api/v1/users/{userID}/books/{bookID}
        [HttpDelete("{userID}/books/{bookID}")]
        public IActionResult ReturnBook(int userID, int bookID)
        {
            try
            {
                bookService.ReturnBook(userID, bookID);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // PUT api/v1/users/{userID}/books/{bookID}
        [HttpPut("{userID}/books/{bookID}")]
        public IActionResult ReplaceLoan(int userID, int bookID, [FromBody] LoanViewModel loan)
        {
            if (!ModelState.IsValid)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
                    Message = "Validation error. Invalid input"
                };

                return StatusCode(error.Code, error);
            }

            try
            {
                bookService.ReplaceLoan(userID, bookID, loan);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (InvalidDataException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 409,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // PATCH api/v1/users/{userID}/books/{bookID}
        [HttpPatch("{userID}/books/{bookID}")]
        public IActionResult UpdateLoan(int userID, int bookID, [FromBody] PatchLoanViewModel loan)
        {
            if (!ModelState.IsValid)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
                    Message = "Validation error. Invalid input"
                };

                return StatusCode(error.Code, error);
            }

            try
            {
                bookService.UpdateLoan(userID, bookID, loan);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (InvalidDataException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 409,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        /* 
         * =============================
         *           Reviews
         * =============================
         */

        // GET api/v1/users/{userID}/reviews
        [HttpGet("{userID}/reviews")]
        public IActionResult GetUserReviewsByID(int userID, [FromQuery] int pageNumber = 1, [FromQuery] int? pageSize = null)
        {
            try
            {
                var reviews = reviewService.GetReviewsByUserID(userID, pageNumber, pageSize);

                return Ok(reviews);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // GET api/v1/users/{userID}/reviews/{bookID}
        [HttpGet("{userID}/reviews/{bookID}", Name = "GetUserReview")]
        public IActionResult GetUserReview(int userID, int bookID)
        {
            try
            {
                var review = reviewService.GetReview(userID, bookID);

                return Ok(review);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // POST api/v1/users/{userID}/reviews/{bookID}
        [HttpPost("{userID}/reviews/{bookID}")]
        public IActionResult AddReview(int userID, int bookID, [FromBody] ReviewViewModel review)
        {
            if (!ModelState.IsValid)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
                    Message = "Validation error. Invalid input"
                };

                return StatusCode(error.Code, error);
            }

            try
            {
                var addedReview = reviewService.AddReview(userID, bookID, review);

                return CreatedAtRoute("GetUserReview", new { userID = userID, bookID = bookID }, addedReview);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (AlreadyExistsException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 409,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // PUT api/v1/users/{userID}/reviews/{bookID}
        [HttpPut("{userID}/reviews/{bookID}")]
        public IActionResult ReplaceReview(int userID, int bookID, [FromBody] ReviewViewModel review)
        {
            if (!ModelState.IsValid)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
                    Message = "Validation error. Invalid input"
                };

                return StatusCode(error.Code, error);
            }

            try
            {
                reviewService.ReplaceReview(userID, bookID, review);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // PATCH api/v1/users/{userID}/reviews/{bookID}
        [HttpPatch("{userID}/reviews/{bookID}")]
        public IActionResult UpdateReview(int userID, int bookID, [FromBody] PatchReviewViewModel review)
        {
            if (!ModelState.IsValid)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
                    Message = "Validation error. Invalid input"
                };

                return StatusCode(error.Code, error);
            }

            try
            {
                reviewService.UpdateReview(userID, bookID, review);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        // DELETE api/v1/users/{userID}/reviews/{bookID}
        [HttpDelete("{userID}/reviews/{bookID}")]
        public IActionResult DeleteReview(int userID, int bookID)
        {
            try
            {
                reviewService.DeleteReview(userID, bookID);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }

        /* 
         * =============================
         *        Recommendations
         * =============================
         */

        // GET api/v1/users/{userID}/recommendations
        [HttpGet("{userID}/recommendations")]
        public IActionResult GetRecommendations(int userID, [FromQuery] int pageNumber = 1, [FromQuery] int? pageSize = null)
        {
            try
            {
                var recommendations = recommendationService.GetRecommendationsByUserID(userID, pageNumber, pageSize);

                return Ok(recommendations);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 404,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO
                {
                    Code = 500,
                    Message = ex.Message
                };

                return StatusCode(error.Code, error);
            }
        }
    }
}
