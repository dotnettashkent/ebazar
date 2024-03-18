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


        [HttpGet("get/all/pending")]
        public async Task<ActionResult<TableResponse<OrderView>>> GetAllPending([FromQuery] TableOptions options, [FromHeader(Name = "Authorization")] string token,CancellationToken cancellationToken = default)
        {
            try
            {
                if (String.IsNullOrEmpty(token) || token is null)
                {
                    var errorMessage = new Dictionary<string, string>
                    {
                        ["key"] = "token",
                        ["msg_uz"] = "Token majburiy",
                        ["msg_ru"] = "Токен обязательна",
                        ["msg_en"] = "Token is required"
                    };
                    return StatusCode(401, new { success = false, message = errorMessage });
                }
                options.token = token;
                var result = await orderServices.GetAllPending(options, cancellationToken);
                return StatusCode(200, new { success = true, result.Items, result.TotalItems, result.AllPage, result.CurrentPage });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(403, new { success = false, message = errorMessage });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }
        [HttpGet("get/all/process")]
        public async Task<ActionResult<TableResponse<OrderView>>> GetAllProcessing([FromQuery] TableOptions options, [FromHeader(Name = "Authorization")] string token, CancellationToken cancellationToken = default)
        {
            try
            {
                if (String.IsNullOrEmpty(token) || token is null)
                {
                    var errorMessage = new Dictionary<string, string>
                    {
                        ["key"] = "token",
                        ["msg_uz"] = "Token majburiy",
                        ["msg_ru"] = "Токен обязательна",
                        ["msg_en"] = "Token is required"
                    };
                    return StatusCode(401, new { success = false, message = errorMessage });
                }
                options.token = token;
                var result = await orderServices.GetAllInProcess(options, cancellationToken);
                return StatusCode(200, new { success = true, result.Items, result.TotalItems, result.AllPage, result.CurrentPage });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/all/accept")]
        public async Task<ActionResult<TableResponse<OrderView>>> GetAllAccept([FromQuery] TableOptions options, [FromHeader(Name = "Authorization")] string token, CancellationToken cancellationToken = default)
        {
            try
            {
                if (String.IsNullOrEmpty(token) || token is null)
                {
                    var errorMessage = new Dictionary<string, string>
                    {
                        ["key"] = "token",
                        ["msg_uz"] = "Token majburiy",
                        ["msg_ru"] = "Токен обязательна",
                        ["msg_en"] = "Token is required"
                    };
                    return StatusCode(401, new { success = false, message = errorMessage });
                }
                options.token = token;
                var result = await orderServices.GetAllAccept(options, cancellationToken);
                return StatusCode(200, new { success = true, result.Items, result.TotalItems, result.AllPage, result.CurrentPage });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/all/cancel")]
        public async Task<ActionResult<TableResponse<OrderView>>> GetAllCancel([FromQuery] TableOptions options, [FromHeader(Name = "Authorization")] string token, CancellationToken cancellationToken = default)
        {
            try
            {
                if (String.IsNullOrEmpty(token) || token is null)
                {
                    var errorMessage = new Dictionary<string, string>
                    {
                        ["key"] = "token",
                        ["msg_uz"] = "Token majburiy",
                        ["msg_ru"] = "Токен обязательна",
                        ["msg_en"] = "Token is required"
                    };
                    return StatusCode(401, new { success = false, message = errorMessage });
                }
                options.token = token;
                var result = await orderServices.GetAllCancelled(options, cancellationToken);
                return StatusCode(200, new { success = true, result.Items, result.TotalItems, result.AllPage, result.CurrentPage });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
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
                    var errorMessage = new Dictionary<string, string>
                    {
                        ["key"] = "token",
                        ["msg_uz"] = "Token majburiy",
                        ["msg_ru"] = "Токен обязательна",
                        ["msg_en"] = "Token is required"
                    };
                    return StatusCode(401, new { success = false, message = errorMessage });
                }
                var orderResponse = await orderServices.Get(token);
                return orderResponse;
            }
            catch (CustomException ex) when (ex.Message == "OrderEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Buyurtma topilmadi",
                    ["msg_ru"] = "Заказ не найден",
                    ["msg_en"] = "Order not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission" || ex.Message == "Payload is null")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
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
                    var errorMessage = new Dictionary<string, string>
                    {
                        ["key"] = "token",
                        ["msg_uz"] = "Token majburiy",
                        ["msg_ru"] = "Токен обязательна",
                        ["msg_en"] = "Token is required"
                    };
                    return StatusCode(401, new { success = false, message = errorMessage });
                }
                var order = await orderServices.GetForAdmin(token, OrderId,cancellationToken);
                return order;
            }
            catch (CustomException ex) when (ex.Message == "OrderEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Buyurtma topilmadi",
                    ["msg_ru"] = "Заказ не найден",
                    ["msg_en"] = "Order not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
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
                    var errorMessage = new Dictionary<string, string>
                    {
                        ["key"] = "token",
                        ["msg_uz"] = "Token majburiy",
                        ["msg_ru"] = "Токен обязательна",
                        ["msg_en"] = "Token is required"
                    };
                    return StatusCode(401, new { success = false, message = errorMessage });
                }
                command.Entity.Token = token;
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "OrderEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Buyurtma topilmadi",
                    ["msg_ru"] = "Заказ не найден",
                    ["msg_en"] = "Order not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sizga ruxsat berilmagan",
                    ["msg_ru"] = "Тебе не разрешено",
                    ["msg_en"] = "You are not allowed"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

    }
}
