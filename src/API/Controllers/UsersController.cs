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
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        // GET api/v1/users
        [HttpGet]
        public IActionResult GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int? pageSize = null)
        {
            try
            {
                var users = userService.GetUsers(pageNumber, pageSize);

                return Ok(users);
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
        public IActionResult GetUser(int userID)
        {
            try
            {
                var user = userService.GetUserByID(userID);

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

        // GET api/v1/users/{userID}/recommendations
        [HttpGet("{userID}/recommendations")]
        public IActionResult GetRecommendations(int userID)
        {
            var error = new ErrorDTO
            {
                Code = 501,
                Message = "Recommendations not yet implemented"
            };

            return StatusCode(error.Code, error);
        }

        // POST api/v1/users
        [HttpPost]
        public IActionResult AddUser([FromBody] UserViewModel user)
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
                var userID = userService.AddUser(user);
                var createdUser = userService.GetUserByID(userID);

                return CreatedAtRoute("GetUserByID", new { userID = userID }, createdUser);
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
                    Code = 400,
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
                    Code = 400,
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
    }
}
