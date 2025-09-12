using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Interfaces
{
    public interface ITransactionEventPublisherFactory
    {
        Task<ITransactionEventPublisher> CreateAsync();
    }
}