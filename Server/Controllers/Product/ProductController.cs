using Stl.CommandR;
using Shared.Features;
using Shared.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructures.Extensions;

namespace Server.Controllers.Product
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ICommander commander;

        public ProductController(IProductService productService, ICommander commander)
        {
            this.productService = productService;
            this.commander = commander;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromForm] CreateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductEntity Not Found")
            {
                return StatusCode(404, new { success = false, messages = "Product Not Found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpPut("udpate")]
        public async Task<ActionResult> Update([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductEntity Not Found")
            {
                return StatusCode(404, new { success = false, messages = "Product not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductEntity Not Found")
            {
                return StatusCode(404, new { success = false, messages = "Product not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<ProductResultView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await productService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ActionResult<ProductResultView>> Get(long Id, CancellationToken cancellation)
        {
            try
            {
                var user = await productService.GetById(Id,cancellation);
                return user;
            }
            catch (CustomException ex) when (ex.Message == "ProductEntity Not Found")
            {
                return StatusCode(404, new { success = false, messages = "Product not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

    }
}
