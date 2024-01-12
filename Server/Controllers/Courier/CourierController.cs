using Microsoft.AspNetCore.Mvc;
using Service.Features;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.CommandR;

namespace Server.Controllers.Courier
{
    [Route("api/courier")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        private readonly ICourierService courierService;
        private readonly ICommander commander;
        public CourierController(ICourierService courierService, ICommander commander)
        {
            this.courierService = courierService;
            this.commander = commander;
        }

        [HttpPost("create")]
        public Task Create([FromBody] CreateCourierCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }
        [HttpPut("udpate")]
        public Task Update([FromBody] UpdateCourierCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpDelete("delete")]
        public async Task Delete([FromBody] DeleteCourierCommand command, CancellationToken cancellationToken)
        {
            await commander.Call(command, cancellationToken);
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<CourierView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await courierService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<CourierView> Get(long Id, CancellationToken cancellationToken)
        {
            return await courierService.GetById(Id,cancellationToken);
        }
    }
}
