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
        public async Task<ActionResult> Create([FromBody] CreateProductSubCategoryCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string? token)
        {
            try
            {
                if (String.IsNullOrEmpty(token))
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
                var validationErrors = StaticHelperMethod.ValidateCreateSubCategoryCommand(command);
                if (validationErrors.Any())
                {
                    var errorObjects = validationErrors.Select(errorJson => StaticHelperMethod.DeserializeError(errorJson)).ToList();
                    return StatusCode(400, new { success = false, message = errorObjects });
                }
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "Already exists")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Allaqachon mavjud",
                    ["msg_ru"] = "Уже существует",
                    ["msg_en"] = "Already exists"
                };
                return StatusCode(403, new { success = false, message = errorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }
        [HttpPut("udpate")]
        public async Task<ActionResult> Update([FromBody] UpdateProductSubCategoryCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string? token)
        {
            try
            {
                if (String.IsNullOrEmpty(token))
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
                var validationErrors = StaticHelperMethod.ValidateUpdateSubCategoryCommand(command);
                if (validationErrors.Any())
                {
                    var errorObjects = validationErrors.Select(errorJson => StaticHelperMethod.DeserializeError(errorJson)).ToList();
                    return StatusCode(400, new { success = false, message = errorObjects });
                }
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductSubCategoryEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sub kategoriya topilmadi",
                    ["msg_ru"] = "Суб категория не найден",
                    ["msg_en"] = "Sub category not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteProductSubCategoryCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string? token)
        {
            try
            {
                if (String.IsNullOrEmpty(token))
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
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "ProductCategoryEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sub kategoriya topilmadi",
                    ["msg_ru"] = "Суб категория не найден",
                    ["msg_en"] = "Sub category not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
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
        public async Task<ActionResult<ProductSubCategoryView>> Get(long Id)
        {
            try
            {
                var user = await productSubCategoryService.Get(Id);
                return StatusCode(200, new { success = true, messages = user });
            }
            catch (CustomException ex) when (ex.Message == "ProductSubCategoryEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Sub kategoriya topilmadi",
                    ["msg_ru"] = "Суб категория не найден",
                    ["msg_en"] = "Sub category not found"
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
