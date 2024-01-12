using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Features;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
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

		[HttpPost("create")]
		public Task Create([FromBody] CreateBrandCommand command, CancellationToken cancellationToken)
		{
			return commander.Call(command, cancellationToken);
		}
        [HttpPut("udpate")]
        public Task Update([FromBody] UpdateBrandCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpDelete("delete")]
        public async Task Delete([FromBody] DeleteBrandCommand command, CancellationToken cancellationToken)
        {
            await commander.Call(command, cancellationToken);
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<BrandView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await brandService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<BrandView> Get(long Id)
        {
            return await brandService.Get(Id);
        }
    }
}
