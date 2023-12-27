using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Features;

namespace Server.Controllers.Brand
{
	[Route("api/brands")]
	[ApiController]
	public class BrandController : ControllerBase
	{
		private readonly IBrandService brandService;

		public BrandController(IBrandService brandService)
		{
			this.brandService = brandService;
		}

		[HttpPost("")]
		public async Task<IActionResult> Create(CreateBrandCommand command, CancellationToken cancellationToken)
		{
			var brand = brandService.Create(command, cancellationToken);
			return new ObjectResult(brand);
		}
	}
}
