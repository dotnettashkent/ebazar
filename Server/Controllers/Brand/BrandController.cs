using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Stl.CommandR;

namespace Server.Controllers.Brand
{
	[Route("api/brands")]
	[ApiController]
	public class BrandController : ControllerBase
	{
		private readonly IBrandService brandService;
		private readonly ICommander commander;
		public BrandController(IBrandService brandService, ICommander commander)
		{
			this.brandService = brandService;
			this.commander = commander;
		}

		[HttpPost("")]
		public Task Create([FromBody] CreateBrandCommand command, CancellationToken cancellationToken)
		{
			return commander.Call(command, cancellationToken);
		}

	}
}
