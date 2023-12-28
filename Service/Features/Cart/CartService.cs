using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;

namespace Service.Features.Cart
{
	public class CartService : ICartService
	{
		public Task Create(CreateBrandCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Delete(DeleteBrandCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<CartView> Get(long Id, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TableResponse<CartView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Update(UpdateBrandCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
