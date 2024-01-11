using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("")]
        public Task Create([FromBody] CreateCartCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpDelete("")]
        public Task Delete([FromBody] DeleteCartCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpGet("get/favourites")]
        public Task<TableResponse<ProductView>> GetAll(long userId)
        {
            return favouriteService.GetAll(userId);
        }
    }
}
