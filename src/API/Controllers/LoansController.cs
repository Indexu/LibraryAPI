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
    public class LoansController : Controller
    {
        private readonly ILoanService loanService;
        private readonly IMapper mapper;

        public LoansController(ILoanService loanService, IMapper mapper)
        {
            this.loanService = loanService;
            this.mapper = mapper;
        }

        // GET api/v1/loans
        [HttpGet]
        public IActionResult GetLoans([FromQuery] int pageNumber = 1,
                                      [FromQuery] int? pageSize = null,
                                      [FromQuery] int? userID = null,
                                      [FromQuery] int? bookID = null,
                                      [FromQuery] DateTime? date = null,
                                      [FromQuery] bool? monthSpan = null)
        {
            try
            {
                var loans = loanService.GetLoans(pageNumber, pageSize, userID, bookID, date, monthSpan);

                return Ok(loans);
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

        // GET api/v1/loan/{loanID}
        [HttpGet("{loanID}", Name = "GetLoanByID")]
        public IActionResult GetLoan(int loanID)
        {
            try
            {
                var loan = loanService.GetLoanByID(loanID);

                return Ok(loan);
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

        // POST api/v1/loans
        [HttpPost]
        public IActionResult AddLoan([FromBody] LoanViewModel loan)
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
                var loanID = loanService.AddLoan(loan);
                var createdLoan = loanService.GetLoanByID(loanID);

                return CreatedAtRoute("GetLoanByID", new { loanID = loanID }, createdLoan);
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
            catch (InvalidDataException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
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

        // POST api/v1/loans/{loanID}/return
        [HttpPost("{loanID}/return")]
        public IActionResult ReturnLoanedBook(int loanID)
        {
            var error = new ErrorDTO
            {
                Code = 501,
                Message = "Returning not yet implemented"
            };

            return StatusCode(error.Code, error);
        }

        // PUT api/v1/loans/{loanID}
        [HttpPut("{loanID}")]
        public IActionResult ReplaceLoan(int loanID, [FromBody] LoanViewModel loan)
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
                loanService.ReplaceLoan(loanID, loan);

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
            catch (InvalidDataException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
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

        // PATCH api/v1/loans/{loanID}
        [HttpPatch("{loanID}")]
        public IActionResult PartiallyUpdateLoan(int loanID, [FromBody] PatchLoanViewModel loan)
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
                loanService.UpdateLoan(loanID, loan);

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
            catch (InvalidDataException ex)
            {
                var error = new ErrorDTO
                {
                    Code = 412,
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

        // DELETE api/v1/loans/{loanID}
        [HttpDelete("{loanID}")]
        public IActionResult DeleteLoan(int loanID)
        {
            try
            {
                loanService.DeleteLoanByID(loanID);

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
