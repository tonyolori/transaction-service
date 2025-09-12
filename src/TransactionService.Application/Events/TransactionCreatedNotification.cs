using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TransactionService.Domain.Events;

namespace TransactionService.Application.Events
{
    public class TransactionCreatedNotification(TransactionCreatedEvent @event) : INotification
    {
        public TransactionCreatedEvent Event { get; } = @event;
    }
}