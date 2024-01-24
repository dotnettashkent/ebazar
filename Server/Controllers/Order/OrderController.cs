using Stl.CommandR;
using Shared.Features;
using Shared.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructures.Extensions;

namespace Server.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices orderServices;
        private readonly ICommander commander;
        public OrderController(IOrderServices orderServices, ICommander commander)
        {
            this.orderServices = orderServices;
            this.commander = commander;
        }

        [HttpPost("create")]
        public Task Create(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<OrderView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await orderServices.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<OrderResponse> Get([FromQuery] long UserId, CancellationToken cancellationToken = default)
        {
            return await orderServices.Get(UserId, cancellationToken);
        }
    }
}
