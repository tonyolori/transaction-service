using System;
using MediatR;

namespace TransactionService.Application.Commands
{
    public record GenerateReceiptCommand(Guid TransactionId, string Format) : IRequest<byte[]>;
}