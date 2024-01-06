using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Stl.CommandR;

namespace Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService productService;
		private readonly ICommander commander;

		public ProductController(IProductService productService, ICommander commander)
		{
			this.productService = productService;
			this.commander = commander;
		}

		[HttpPost("")]
		public Task Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
		{
			return commander.Call(command, cancellationToken);
		}

	}
}
