using Service.Data;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Fusion.EntityFramework;

namespace Service.Features
{
    public class OrderService : IOrderServices
    {
        #region Initialize
        private readonly DbHub<AppDbContext> dbHub;
        public OrderService(DbHub<AppDbContext> dbHub)
        {
            this.dbHub = dbHub;
        }
        #endregion
        #region Queries
        public Task<TableResponse<OrderView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<OrderView> GetById(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Mutations
        public Task Create(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Delete(DeleteOrderCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }



        public Task Update(UpdateOrderCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Helpers
        #endregion

    }
}
