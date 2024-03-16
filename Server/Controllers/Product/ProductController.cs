using Stl.CommandR;
using Shared.Features;
using Shared.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructures.Extensions;
using Stl.Fusion;
using Service.Features;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading;

namespace Server.Controllers.Product
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IFileService fileService;
        private readonly ICommander commander;

        public ProductController(IProductService productService, ICommander commander, IFileService fileService)
        {
            this.productService = productService;
            this.commander = commander;
            this.fileService = fileService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromForm] CreateProductCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string? token)
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
                    return StatusCode(400, new { success = false, message = errorMessage });
                }
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Mahsulot topilmadi",
                    ["msg_ru"] = "Продукт не найден",
                    ["msg_en"] = "Product not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpPut("udpate")]
        public async Task<ActionResult> Update([FromForm] UpdateProductCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
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
                    return StatusCode(400, new { success = false, message = errorMessage });
                }
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Mahsulot topilmadi",
                    ["msg_ru"] = "Продукт не найден",
                    ["msg_en"] = "Product not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteProductCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
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
                    return StatusCode(400, new { success = false, message = errorMessage });
                }
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Mahsulot topilmadi",
                    ["msg_ru"] = "Продукт не найден",
                    ["msg_en"] = "Product not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpDelete("delete/file")]
        public async Task<IActionResult> DeleteImage(string fileName, [FromHeader(Name = "Authorization")] string token)
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
                    return StatusCode(400, new { success = false, message = errorMessage });
                }
                var result = await fileService.DeleteOneImage(fileName, token);
                if (result)
                {
                    return StatusCode(200, new { success = true });
                }
                else
                {
                    return StatusCode(400, new { success = false });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
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
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Mahsulot topilmadi",
                    ["msg_ru"] = "Продукт не найден",
                    ["msg_en"] = "Product not found"
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
