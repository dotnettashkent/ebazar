using Stl.CommandR;
using Shared.Features;
using Shared.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructures.Extensions;

namespace Server.Controllers.Product
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ICommander commander;

        public ProductController(IProductService productService, ICommander commander)
        {
            this.productService = productService;
            this.commander = commander;
        }

        [HttpPost("create")]
        public Task Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpPut("udpate")]
        public Task Update([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            return commander.Call(command, cancellationToken);
        }

        [HttpDelete("delete")]
        public async Task Delete([FromBody] DeleteProductCommand command, CancellationToken cancellationToken)
        {
            await commander.Call(command, cancellationToken);
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<ProductResultView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await productService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ProductResultView> Get(long Id)
        {
            return await productService.GetById(Id);
        }

    }
}
