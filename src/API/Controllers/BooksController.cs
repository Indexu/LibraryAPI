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
    /// <summary>
    /// A controller for routes starting with /books
    /// 
    /// Author: Hilmar Tryggvason
    /// 
    /// Version: 1.0, 30 Oct 2017
    /// 
    /// License: MIT License (https://opensource.org/licenses/MIT)
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        private readonly IReportingService reportingService;
        private readonly IReviewService reviewService;
        private readonly IMapper mapper;

        public BooksController(IBookService bookService,
                               IReportingService reportingService,
                               IReviewService reviewService,
                               IMapper mapper)
        {
            this.bookService = bookService;
            this.reportingService = reportingService;
            this.reviewService = reviewService;
            this.mapper = mapper;
        }

        /* 
         * =============================
         *           Books
         * =============================
         */

        // GET api/v1/books
        [HttpGet]
        public IActionResult GetBooks([FromQuery] int pageNumber = 1,
                                      [FromQuery] int? pageSize = null,
                                      [FromQuery] DateTime? loanDate = null,
                                      [FromQuery] int? duration = null)
        {
            try
            {
                if (!loanDate.HasValue && !duration.HasValue)
                {
                    var books = bookService.GetBooks(pageNumber, pageSize);
                    return Ok(books);
                }
                else
                {
                    var report = reportingService.GetBooksReport(pageNumber, pageSize, loanDate, duration);
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

        // GET api/v1/book/{bookID}
        [HttpGet("{bookID}", Name = "GetBookByID")]
        public IActionResult GetBook(int bookID, [FromQuery] int pageNumber = 1, [FromQuery] int? pageSize = null)
        {
            try
            {
                var book = bookService.GetBookByID(bookID, pageNumber, pageSize);

                return Ok(book);
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

        // POST api/v1/books
        [HttpPost]
        public IActionResult AddBook([FromBody] BookViewModel book)
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
                var bookDTO = bookService.AddBook(book);

                return CreatedAtRoute("GetBookByID", new { bookID = bookDTO.ID }, bookDTO);
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

        // PUT api/v1/books/{bookID}
        [HttpPut("{bookID}")]
        public IActionResult ReplaceBook(int bookID, [FromBody] BookViewModel book)
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
                bookService.ReplaceBook(bookID, book);

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

        // PATCH api/v1/books/{bookID}
        [HttpPatch("{bookID}")]
        public IActionResult PartiallyUpdateBook(int bookID, [FromBody] PatchBookViewModel book)
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
                bookService.UpdateBook(bookID, book);

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

        // DELETE api/v1/books/{bookID}
        [HttpDelete("{bookID}")]
        public IActionResult DeleteBook(int bookID)
        {
            try
            {
                bookService.DeleteBookByID(bookID);

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
         *           Reviews
         * =============================
         */

        // GET api/v1/books/reviews
        [HttpGet("reviews")]
        public IActionResult GetBookReviews([FromQuery] int pageNumber = 1, [FromQuery] int? pageSize = null)
        {
            try
            {
                var reviews = reviewService.GetBookReviews(pageNumber, pageSize);

                return Ok(reviews);
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

        // GET api/v1/books/{bookID}/reviews
        [HttpGet("{bookID}/reviews")]
        public IActionResult GetBookReviewsByID(int bookID, [FromQuery] int pageNumber = 1, [FromQuery] int? pageSize = null)
        {
            try
            {
                var reviews = reviewService.GetReviewsByBookID(bookID, pageNumber, pageSize);

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

        // GET api/v1/books/{bookID}/reviews/{userID}
        [HttpGet("{bookID}/reviews/{userID}", Name = "GetBookReview")]
        public IActionResult GetBookReview(int bookID, int userID)
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

        // POST api/v1/books/{bookID}/reviews/{userID}
        [HttpPost("{bookID}/reviews/{userID}")]
        public IActionResult AddReview(int bookID, int userID, [FromBody] ReviewViewModel review)
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

                return CreatedAtRoute("GetBookReview", new { bookID = bookID, userID = userID }, addedReview);
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

        // PUT api/v1/books/{bookID}/reviews/{userID}
        [HttpPut("{bookID}/reviews/{userID}")]
        public IActionResult ReplaceReview(int bookID, int userID, [FromBody] ReviewViewModel review)
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

        // PATCH api/v1/books/{bookID}/reviews/{userID}
        [HttpPatch("{bookID}/reviews/{userID}")]
        public IActionResult UpdateReview(int bookID, int userID, [FromBody] PatchReviewViewModel review)
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

        // DELETE api/v1/books/{bookID}/reviews/{userID}
        [HttpDelete("{bookID}/reviews/{userID}")]
        public IActionResult DeleteReview(int bookID, int userID)
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
    }
}
