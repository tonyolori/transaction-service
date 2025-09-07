namespace TransactionService.Application.Queries;

using MediatR;
using TransactionService.Application.DTOs;
public record GetTransactionQuery(Guid Id) : IRequest<TransactionDto?>;
