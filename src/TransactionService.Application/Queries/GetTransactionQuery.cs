using MediatR;
using TransactionService.Application.DTOs;

namespace TransactionService.Application.Queries
{
    public record GetTransactionQuery(Guid Id) : IRequest<TransactionDto?>;
    
}
