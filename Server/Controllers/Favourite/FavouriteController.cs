﻿using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Stl.CommandR;

namespace Server.Controllers.Favourite
{
    [Route("api/favourite")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService favouriteService;
        private readonly ICommander commander;

        public FavouriteController(ICommander commander, IFavouriteService favouriteService)
        {
            this.commander = commander;
            this.favouriteService = favouriteService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CreateFavouriteCommand command, CancellationToken cancellationToken)
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
        public async Task<ActionResult> Delete([FromBody] DeleteFavouriteCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "Favourite Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Favourite Not Found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/favourite")]
        public Task<TableResponse<ProductResultView>> GetAll(long userId)
        {
            return favouriteService.GetAll(userId);
        }
    }
}
