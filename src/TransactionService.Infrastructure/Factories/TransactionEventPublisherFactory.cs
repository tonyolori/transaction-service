using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Interfaces;
using TransactionService.Infrastructure.Messaging;

namespace TransactionService.Infrastructure.Factories
{
    public class TransactionEventPublisherFactory(IOptions<RabbitMqSettings> options) : ITransactionEventPublisherFactory
    {
        private readonly IOptions<RabbitMqSettings> _options = options;

        public async Task<ITransactionEventPublisher> CreateAsync()
        {
            var publisher = new TransactionEventPublisher(_options);
            await publisher.InitializeAsync();
            return publisher;
        }
    }
}