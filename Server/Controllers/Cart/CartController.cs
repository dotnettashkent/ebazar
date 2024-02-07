using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Stl.CommandR;

namespace Server.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly ICommander commander;
        public CartController(ICartService cartService, ICommander commander)
        {
            this.cartService = cartService;
            this.commander = commander;
        }

        [HttpPost("")]
        public Task Create([FromBody] CreateCartCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpDelete("delete")]
        public Task Delete([FromBody] DeleteCartCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        /*[HttpPut("delete/all")]
        public async Task DeleteAll(long UserId, CancellationToken cancellationToken)
        {
			await cartService.RemoveAll(UserId,cancellationToken);
        }*/

        [HttpGet("get/carts")]
        public Task<TableResponse<ProductResultView>> GetAll(long userId, CancellationToken cancellationToken)
        {
            return cartService.GetAll(userId, cancellationToken);
        }
    }
}
