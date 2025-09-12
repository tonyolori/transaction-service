using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionService.Domain.Interfaces
{
    public interface ITransactionEventConsumer : IAsyncDisposable
    {
        Task StartConsumingAsync(CancellationToken cancellationToken);
    }
}