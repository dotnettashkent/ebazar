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
            try
            {
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }
        [HttpPut("udpate")]
        public async Task<ActionResult> Update([FromBody] UpdateBrandCommand command, CancellationToken cancellationToken, [FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "BrandEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Brand not found" });
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
                var result = await commander.Call(command with { Token = token }, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "BrandEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Brand not found" });
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
        public async Task<ActionResult<BrandView>> Get(long Id, string token)
        {
            try
            {
                var user = await brandService.Get(Id, token);
                return StatusCode(408, new { success = true, messages = user });
            }
            catch (CustomException ex) when (ex.Message == "BrandEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Brand not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }
    }
}
