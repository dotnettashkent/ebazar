using Stl.CommandR;
using Shared.Features;
using Shared.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructures.Extensions;

namespace Server.Controllers.Brand
{
    [Route("api/brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService brandService;
        private readonly ICommander commander;
        public BrandController(IBrandService brandService, ICommander commander)
        {
            this.brandService = brandService;
            this.commander = commander;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromForm] CreateBrandCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
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

            var validationErrors = StaticHelperMethod.ValidateCreateBrandCommand(command);
            if (validationErrors.Any())
            {
                var errorObjects = validationErrors.Select(errorJson => StaticHelperMethod.DeserializeError(errorJson)).ToList();
                return StatusCode(400, new { success = false, message = errorObjects });
            }

            try
            {
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return Ok(new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "Not Permission")
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
        [HttpPut("udpate")]
        public async Task<ActionResult> Update([FromForm] UpdateBrandCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
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

            var validationErrors = StaticHelperMethod.ValidateUpdateBrandCommand(command);
            if (validationErrors.Any())
            {
                var errorObjects = validationErrors.Select(errorJson => StaticHelperMethod.DeserializeError(errorJson)).ToList();
                return StatusCode(400, new { success = false, message = errorObjects });
            }

            try
            {
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return Ok(new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "BrandEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Brand topilmadi",
                    ["msg_ru"] = "Бренд не найден",
                    ["msg_en"] = "Brand not found"
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

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteBrandCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
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
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "BrandEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Brand topilmadi",
                    ["msg_ru"] = "Бренд не найден",
                    ["msg_en"] = "Brand not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }

            catch (CustomException ex) when (ex.Message == "Not Permission")
            {
                return StatusCode(403, new { success = false, messages = "You are not allowed" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<BrandView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await brandService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ActionResult<BrandView>> Get(long Id)
        {
            try
            {
                var user = await brandService.Get(Id);
                return StatusCode(200, new { success = true, messages = user });
            }
            catch (CustomException ex) when (ex.Message == "BrandEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Brand topilmadi",
                    ["msg_ru"] = "Ссылка обязательна",
                    ["msg_en"] = "Бренд не найден"
                };
                return StatusCode(400, new { success = false, error = errorMessage });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }
    }
}
