﻿using Stl.CommandR;
using Shared.Features;
using Shared.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructures.Extensions;

namespace Server.Controllers.Banner
{
    [Route("api/banner")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService bannerService;
        private readonly ICommander commander;

        public BannerController(ICommander commander, IBannerService bannerService)
        {
            this.commander = commander;
            this.bannerService = bannerService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromForm] CreateBannerCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
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

            var validationErrors = StaticHelperMethod.ValidateCreateBannerCommand(command);
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
                return StatusCode(403, new { success = false, message = "You are not allowed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpPut("udpate")]
        public async Task<ActionResult> Update([FromForm] UpdateBannerCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
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

            var validationErrors = StaticHelperMethod.ValidateUpdateBannerCommand(command);
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
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteBannerCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
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
            catch (CustomException ex) when (ex.Message == "BannerEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Banner topilmadi",
                    ["msg_ru"] = "Баннер не найден",
                    ["msg_en"] = "Banner not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<BannerView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await bannerService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ActionResult<BannerView>> Get(long Id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await bannerService.Get(Id);
                return StatusCode(200, new { success = true, messages = user });
            }
            catch (CustomException ex) when (ex.Message == "BannerEntity Not Found")
            {
                var errorMessage = new Dictionary<string, string>
                {
                    ["msg_uz"] = "Banner topilmadi",
                    ["msg_ru"] = "Баннер не найден",
                    ["msg_en"] = "Banner not found"
                };
                return StatusCode(400, new { success = false, message = errorMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, success = false });
            }
        }
    }
}
