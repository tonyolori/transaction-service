using System;
using MediatR;

namespace TransactionService.Application.Commands
{
    public record CreateShareableLinkCommand(Guid TransactionId, int ExpiresInSeconds) : IRequest<string>; 
}
