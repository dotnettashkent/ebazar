using Stl.CommandR;
using Shared.Features;
using Shared.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructures.Extensions;

namespace Server.Controllers.Courier
{
    [Route("api/courier")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        private readonly ICourierService courierService;
        private readonly ICommander commander;
        public CourierController(ICourierService courierService, ICommander commander)
        {
            this.commander = commander;
            this.courierService = courierService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] CreateCourierCommand command, CancellationToken cancellationToken)
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
        [HttpPut("udpate")]
        public async Task<ActionResult> Update([FromBody] UpdateCourierCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "CourierEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Courier not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] DeleteCourierCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "CourierEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Courier not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<CourierView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await courierService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ActionResult<CourierView>> Get(long Id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await courierService.GetById(Id);
                return StatusCode(408, new { success = true, messages = user });
            }
            catch (CustomException ex) when (ex.Message == "CourierEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "Courier not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }
    }
}
