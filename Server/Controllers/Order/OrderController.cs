using Microsoft.AspNetCore.Mvc;
using Service.Features;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
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

        [HttpPost("create")]
        public async Task<ActionResult> Create(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }


        [HttpGet("get/all")]
        public async Task<TableResponse<OrderView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await orderServices.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ActionResult<OrderResponse>> Get([FromQuery] string token, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await orderServices.Get(token, cancellationToken);
                return user;
            }
            catch (CustomException ex) when (ex.Message == "OrderEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Order not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "OrderEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Order not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

    }
}
