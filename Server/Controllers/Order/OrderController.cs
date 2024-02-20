using Stl.CommandR;
using Shared.Features;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructures;
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
        public async Task<ActionResult> Create(CreateOrderCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                if (String.IsNullOrEmpty(token) || token is null)
                {
                    return StatusCode(401, new { success = false, message = "token is required" });
                }
                command.Entity.Token = token;
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }


        [HttpGet("get/all/admin")]
        public async Task<ActionResult<TableResponse<OrderView>>> GetAll([FromQuery] TableOptions options, [FromHeader(Name = "Authorization")] string token,CancellationToken cancellationToken = default)
        {
            try
            {
                if (String.IsNullOrEmpty(token) || token is null)
                {
                    return StatusCode(401, new { success = false, message = "token is required" });
                }
                options.token = token;
                var result = await orderServices.GetAll(options, cancellationToken);
                return StatusCode(200, new { success = true, message = result });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get")]
        public async Task<ActionResult<OrderResponse>> Get([FromHeader] string token)
        {
            try
            {
                if (String.IsNullOrEmpty(token) || token is null)
                {
                    return StatusCode(401, new { success = false, message = "token is required" });
                }
                var orderResponse = await orderServices.Get(token);
                return orderResponse;
            }
            catch (CustomException ex) when (ex.Message == "OrderEntity Not Found")
            {
                return StatusCode(404, new { success = false, message = "Order not found" });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission" || ex.Message == "Payload is null")
            {
                return StatusCode(403, new { success = false, message = "Not Permission" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }


        [HttpGet("get/for/admin")]
        public async Task<ActionResult<OrderResponse>> GetForAdmin([FromHeader] string token, long OrderId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (String.IsNullOrEmpty(token) || token is null)
                {
                    return StatusCode(401, new { success = false, message = "token is required" });
                }
                var order = await orderServices.GetForAdmin(token, OrderId,cancellationToken);
                return order;
            }
            catch (CustomException ex) when (ex.Message == "OrderEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Order not found" });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(UpdateOrderCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                if (String.IsNullOrEmpty(token) || token is null)
                {
                    return StatusCode(401, new { success = false, message = "token is required" });
                }
                command.Entity.Token = token;
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "OrderEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Order not found" });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

    }
}
