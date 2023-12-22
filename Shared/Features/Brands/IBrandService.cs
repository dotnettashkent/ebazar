using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Features.Brands
{
	public interface IBrandService : IComputeService
	{
		//[ComputeMethod]
		Task<TableResponse<BrandView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
		//[ComputeMethod]
		Task<BrandView> Get(long Id, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Create(CreateBannerCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Update(UpdateBannerCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Delete(DeleteBannerCommand command, CancellationToken cancellationToken = default);
		Task<Unit> Invalidate() { return TaskExt.UnitTask; }
	}
}
