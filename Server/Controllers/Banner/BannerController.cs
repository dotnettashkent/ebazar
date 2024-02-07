using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.CommandR;

namespace Server.Controllers.Banner
{
    [Route("api/banner")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService bannerService;
        private readonly ICommander commander;

        public BannerController(ICommander commander, IBannerService bannerService)
        {
            this.commander = commander;
            this.bannerService = bannerService;
        }

        [HttpPost("create")]
        public Task Create([FromForm] CreateBannerCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }
        [HttpPut("udpate")]
        public Task Update([FromBody] UpdateBannerCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpDelete("delete")]
        public async Task Delete([FromBody] DeleteBannerCommand command, CancellationToken cancellationToken)
        {
            await commander.Call(command, cancellationToken);
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<BannerView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await bannerService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<BannerView> Get(long Id, CancellationToken cancellationToken)
        {
            return await bannerService.Get(Id, cancellationToken);
        }
    }
}
