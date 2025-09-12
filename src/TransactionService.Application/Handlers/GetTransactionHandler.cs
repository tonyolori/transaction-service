using MediatR;
using TransactionService.Application.Queries;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Handlers
{
    public class GetTransactionHandler : IRequestHandler<GetTransactionQuery, TransactionDto?>
    {
        public Task<TransactionDto?> Handle(GetTransactionQuery req, CancellationToken ct)
        {
            // TODO: wire up to repository; return fake for scaffold
            var dto = new TransactionDto
            {
                Id = req.Id,
                AccountId = Guid.NewGuid(),
                DestinationAccountId = Guid.NewGuid(),
                Amount = 5000m,
                OpeningBalance = 10000m,
                Currency = TransactionCurrency.NGN,
                Status = TransactionStatus.SUCCESS,
                CreatedAt = DateTime.UtcNow
            };
            return Task.FromResult<TransactionDto?>(dto);
        }
    }
}