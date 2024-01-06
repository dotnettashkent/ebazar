using Microsoft.AspNetCore.Mvc;
using Shared.Features;
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
	}
}
