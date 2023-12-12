using Shared.Features;
using Shared.Features.Banner;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;

namespace Service.Features
{
    public class BannerService : IBannerService
    {
        public Task Create(CreateBannerCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Delete(DeleteBannerCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<BannerView>> Get(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TableResponse<BannerView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Update(UpdateBannerCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
