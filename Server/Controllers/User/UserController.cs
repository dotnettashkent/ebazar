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
        public Task Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpDelete("delete")]
        public Task Delete(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpPut("update")]
        public Task Update(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpGet("login")]
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
