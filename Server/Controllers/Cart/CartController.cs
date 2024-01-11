using Microsoft.AspNetCore.Mvc;
using Service.Features;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Stl.CommandR;

namespace Server.Controllers
{
	[Route("api/[controller]")]
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

		[HttpDelete()]
		public Task Delete([FromBody] DeleteCartCommand command, CancellationToken cancellationToken)
		{
            return commander.Call(command, cancellationToken);
        }

        [HttpGet("get/favourites")]
        public Task<TableResponse<ProductView>> GetAll(long userId)
        {
            return cartService.GetAll(userId);
        }
    }
}
