using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TransactionService.Domain.Interfaces;
using TransactionService.Application.Events;

namespace TransactionService.Application.Handlers
{
    public class TransactionCreatedHandler(ITransactionEventPublisher publisher) : INotificationHandler<TransactionCreatedNotification>
    {
        private readonly ITransactionEventPublisher _publisher = publisher;
        public async Task Handle(TransactionCreatedNotification notification, CancellationToken cancellationToken)
        {
            await _publisher.PublishAsync(notification.Event);
        }
    }
}