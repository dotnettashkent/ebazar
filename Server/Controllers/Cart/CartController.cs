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
        public async Task<ActionResult> Create([FromBody] CreateCartCommand command, CancellationToken cancellationToken)
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

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteCartCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "CartEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Cart not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/carts")]
        public async Task<ActionResult<TableResponse<ProductResultView>>> GetAll(string token, CancellationToken cancellationToken)
        {

            try
            {
                return await cartService.GetAll(token, cancellationToken);
            }
            catch(CustomException ex) when (ex.Message == "CartEntity Not Found") 
            {
                return StatusCode(408, new { success = false, messages = "Cart not found" });
            }

        }
    }
}
