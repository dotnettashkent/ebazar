using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.CommandR;

namespace Server.Controllers.ProductSubCategory
{
    [Route("api/subcategory")]
    [ApiController]
    public class SubCategory : ControllerBase
    {
        private readonly IProductSubCategoryService productSubCategoryService;
        private readonly ICommander commander;

        public SubCategory(ICommander commander, IProductSubCategoryService productSubCategoryService)
        {
            this.commander = commander;
            this.productSubCategoryService = productSubCategoryService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromForm] CreateProductSubCategoryCommand command, CancellationToken cancellationToken)
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
        public async Task<ActionResult> Update([FromBody] UpdateProductSubCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductSubCategoryEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Sub category not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteProductSubCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductSubCategoryEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Sub category not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<ProductSubCategoryView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await productSubCategoryService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ActionResult<ProductSubCategoryView>> Get(long Id, string token)
        {
            try
            {
                var user = await productSubCategoryService.Get(Id, token);
                return StatusCode(408, new { success = true, messages = user });
            }
            catch (CustomException ex) when (ex.Message == "ProductSubCategoryEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Sub category not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }
    }
}
