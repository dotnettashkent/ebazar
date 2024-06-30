using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Stl.CommandR;

namespace Server.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly ICommander commander;
        public CartController(ICartService cartService, ICommander commander)
        {
            this.cartService = cartService;
            this.commander = commander;
        }

        [HttpPost("")]
        public async Task<ActionResult> Create([FromBody] CreateCartCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token = "")
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

        [HttpPatch("patch")]
        public async Task<IActionResult> UpdateProductQuantityAsync([FromBody] UpdateCartCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token = "")
        {
            try
            {
                if (string.IsNullOrEmpty(token) || token is null)
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
            catch (CustomException ex) when (ex.Message == "Cart not found for the user.")
            {
                return StatusCode(404, new { success = false, messages = "Cart not found for the user." });
            }
            catch (CustomException ex) when (ex.Message == "Admin does not have permission to update a product quantity.")
            {
                return StatusCode(403, new { success = false, messages = "Admin does not have permission to update a product quantity." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteCartCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token = "")
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
            catch (CustomException ex) when (ex.Message == "CartEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Cart not found" });
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

        [HttpGet("get/carts")]
        public async Task<ActionResult<TableResponse<ProductResultView>>> GetAll([FromHeader(Name = "Authorization")] string token = "", CancellationToken cancellationToken = default)
        {

            try
            {
                if (string.IsNullOrEmpty(token) || token is null)
                {
                    return StatusCode(401, new { success = false, message = "token is required" });
                }

                var res = await cartService.GetAll(token, cancellationToken);
                if (res.Items.Count() == 0)
                {
                    return Ok(new
                    {
                        items = Array.Empty<string>(),
                        total_items = 0,
                        all_page = 0,
                        current_page = 0,

                    });
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }
            catch (CustomException ex) when (ex.Message == "Payload is null")
            {
                return StatusCode(403, new { success = false, messages = "Not Permission" });
            }
            catch (CustomException ex) when (ex.Message == "CartEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Cart not found" });
            }

        }
    }
}
