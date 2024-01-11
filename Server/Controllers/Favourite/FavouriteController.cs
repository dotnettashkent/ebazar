using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Stl.CommandR;

namespace Server.Controllers.Favourite
{
    [Route("api/[controller]")]
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
        public Task Create([FromBody] CreateFavouriteCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpDelete("")]
        public Task Delete([FromBody] DeleteFavouriteCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }
    }
}
