/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Features;
using Shared.Features;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.CommandR;

namespace Server.Controllers.Address
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly ICommander commander;
        private readonly IAddressService addressService;
        public AddressController(ICommander commander, IAddressService addressService)
        {
            this.commander = commander;
            this.addressService = addressService;
        }


        [HttpPost("create")]
        public Task Create([FromBody] CreateAddressCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }
        [HttpPut("udpate")]
        public Task Update([FromBody] UpdateAddressCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpDelete("delete")]
        public async Task Delete([FromBody] DeleteAddressCommand command, CancellationToken cancellationToken)
        {
            await commander.Call(command, cancellationToken);
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<AddressView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await addressService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<AddressView> Get(long Id, CancellationToken cancellationToken)
        {
            return await addressService.Get(Id, cancellationToken);
        }
    }
}
*/