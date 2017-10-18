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
    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        private readonly IMapper mapper;

        public BooksController(IBookService bookService, IMapper mapper)
        {
            this.bookService = bookService;
            this.mapper = mapper;
        }

        // GET api/v1/books
        [HttpGet]
        public IActionResult GetBooks([FromQuery] int pageNumber = 1, [FromQuery] int? pageSize = null)
        {
            try
            {
                var books = bookService.GetBooks(pageNumber, pageSize);

                return Ok(books);
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
        public IActionResult GetBook(int bookID)
        {
            try
            {
                var book = bookService.GetBookByID(bookID);

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
                    Code = 400,
                    Message = "Validation error. Invalid input"
                };

                return StatusCode(error.Code, error);
            }

            try
            {
                var bookID = bookService.AddBook(book);
                var createdBook = bookService.GetBookByID(bookID);

                return CreatedAtRoute("GetBookByID", new { bookID = bookID }, createdBook);
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
                    Code = 400,
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
                    Code = 400,
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
    }
}
