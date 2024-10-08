﻿using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features
{
    public interface IOrderServices : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<OrderView>> GetAllPending(TableOptions options, CancellationToken cancellationToken = default);
        Task<TableResponse<OrderView>> GetAllInProcess(TableOptions options, CancellationToken cancellationToken = default);
        Task<TableResponse<OrderView>> GetAllAccept(TableOptions options, CancellationToken cancellationToken = default);
        Task<TableResponse<OrderView>> GetAllCancelled(TableOptions options, CancellationToken cancellationToken = default);

        //[ComputeMethod]
        Task<OrderResponse> Get(string token);
        Task<OrderResponse> GetForAdmin(string token, long OrderId, CancellationToken cancellationToken = default);

        [CommandHandler]
        Task Create(CreateOrderCommand command, CancellationToken cancellationToken = default);

        [CommandHandler]
        Task Delete(DeleteOrderCommand command, CancellationToken cancellationToken = default);

        [CommandHandler]
        Task Update(UpdateOrderCommand command, CancellationToken cancellationToken = default);

        //[CommandHandler]
        //Task UpdateItem(UpdateItemOrderCommand command, CancellationToken cancellationToken = default);

        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
