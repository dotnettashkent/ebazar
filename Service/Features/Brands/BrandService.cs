using Shared.Features;
using Shared.Features.Brands;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;

namespace Service.Features.Brands
{
	public class BrandService : IBrandService
	{
		public Task<TableResponse<BrandView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
		public Task<BrandView> Get(long id, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
		public Task Create(CreateBannerCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Delete(DeleteBannerCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}



		public Task Update(UpdateBannerCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
