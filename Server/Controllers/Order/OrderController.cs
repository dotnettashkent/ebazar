using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Stl.CommandR;

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

        [HttpPost("")]
        public Task Create(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }   
    }
}
