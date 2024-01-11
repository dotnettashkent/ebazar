using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Stl.CommandR;

namespace Server.Controllers.User
{
    [Route("api/[controller]")]
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
        [HttpPost("")]
        public Task Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpGet("")]
        public Task<UserView> Login( string email, string password)
        {
            return userService.Login(email, password);
        }

    }
}
