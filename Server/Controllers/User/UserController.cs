using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Features;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.CommandR;

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
            return commander.Call(command,cancellationToken); 
        }

        [HttpGet("login")]
        public Task<string> Login( string email, string password)
        {
            return userService.Login(email, password);
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<UserView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await userService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<UserResultView> Get(long Id)
        {
            return await userService.Get(Id);
        }


    }
}
