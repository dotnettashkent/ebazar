using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.CommandR;
using System.Text.Json.Serialization;

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
        public async Task<ActionResult> Create([FromBody] CreateProductCategoryCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string? token)
        {
            try
            {
                if (String.IsNullOrEmpty(token))
                {
                    return StatusCode(401, new { success = false, message = "token is required" });
                }
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "Already exists")
            {
                return StatusCode(408, new { success = false, messages = "Already exists" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] UpdateProductCategoryCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string? token)
        {
            try
            {
                if (String.IsNullOrEmpty(token))
                {
                    return StatusCode(401, new { success = false, message = "token is required" });
                }
                var result = await commander.Call(command with { Token = token }, cancellationToken);
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
        public async Task<ActionResult> Delete([FromBody] DeleteProductCategoryCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string? token)
        {
            try
            {
                if (String.IsNullOrEmpty(token))
                {
                    return StatusCode(401, new { success = false, message = "token is required" });
                }
                var result = await commander.Call(command with { Token = token }, cancellationToken);
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
        public async Task<ActionResult<ProductCategoryView>> Get(long Id)
        {
            try
            {
                var user = await productCategoryService.Get(Id);
                return StatusCode(200, new { success = true, messages = user });
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

        [HttpGet("get/category/id")]
        public async Task<ActionResult<ProductCategoryView>> GetByCategoryId(long category_id)
        {
            try
            {
                var user = await productCategoryService.GetByCategory(category_id);
                return StatusCode(200, new { success = true, messages = user });
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
