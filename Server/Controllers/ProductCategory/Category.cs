using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.CommandR;

namespace Server.Controllers.ProductCategory
{
    [Route("api/category")]
    [ApiController]
    public class Category : ControllerBase
    {
        private readonly IProductCategoryService productCategoryService;
        private readonly ICommander commander;

        public Category(ICommander commander, IProductCategoryService productCategoryService)
        {
            this.commander = commander;
            this.productCategoryService = productCategoryService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create(CreateProductCategoryCommand command, CancellationToken cancellationToken)
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
        [HttpPut("udpate")]
        public async Task<ActionResult> Update(UpdateProductCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductCategoryEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Category not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(DeleteProductCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductCategoryEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Category not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<ProductCategoryView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await productCategoryService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ActionResult<ProductCategoryView>> Get(long Id, string token)
        {
            try
            {
                var user = await productCategoryService.Get(Id, token);
                return StatusCode(408, new { success = true, messages = user });
            }
            catch (CustomException ex) when (ex.Message == "ProductCategoryEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Category not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }
    }
}
