using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.CommandR;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers.User
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        private readonly ICommander commander;
        public UserController(IUserService userService, ICommander commander)
        {
            this.userService = userService;
            this.commander = commander;
        }
        [HttpPost("create")]
        public async Task<ActionResult<bool>> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Entity.PhoneNumber) || string.IsNullOrEmpty(command.Entity.Password))
            {
                var errorMessages = new List<dynamic>();

                if (string.IsNullOrEmpty(command.Entity.PhoneNumber))
                {
                    errorMessages.Add(new { message = "phone number required", param = "phone_number" });
                }

                if (string.IsNullOrEmpty(command.Entity.Password))
                {
                    errorMessages.Add(new { message = "password required", param = "password" });
                }

                return StatusCode(400, new { success = false, messages = errorMessages });
            }

            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true }); // Return 200 OK with true
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { error = ex.Message, success = false }); // Return 500 Internal Server Error with the error message and false
            }
        }





        [HttpDelete("delete")]
        public Task Delete(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpPut("update")]
        public Task Update(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(string email, string password)
        {
            try
            {
                string token = await userService.Login(email, password);
                return Ok(token); // Return 200 OK with the token
            }
            catch (CustomException ex)
            {
                return NotFound(ex.Message); // Return 404 Not Found with the error message
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error with the error message
            }
        }


        [HttpGet("return/user")]
        public async Task<ActionResult<UserView>> Return(string token)
        {
            try
            {
                return await userService.GetByToken(token);
            }
            catch (CustomException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<UserView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await userService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ActionResult<UserResultView>> Get(long Id)
        {
            try
            {
                var user = await userService.Get(Id);
                if (user == null)
                {
                    return NotFound(); // Returns HTTP 404 Not Found
                }
                return user;
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message); // Returns HTTP 400 Bad Request with the validation exception message
            }
        }


    }
}
