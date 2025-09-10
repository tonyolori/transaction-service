namespace TransactionService.Application.Handlers;

using MediatR;
using TransactionService.Application.Queries;
using TransactionService.Application.DTOs;
public class GetTransactionHandler : IRequestHandler<GetTransactionQuery, TransactionDto?>
{
    public Task<TransactionDto?> Handle(GetTransactionQuery req, CancellationToken ct)
    {
        // TODO: wire up to repository; return fake for scaffold
        var dto = new TransactionDto{
                Id = req.Id,
                AccountId = "acc-123",
                Amount = 5000m,
                Currency = "NGN",
                Status = "success",
                CreatedAt = DateTime.UtcNow
            };
        return Task.FromResult<TransactionDto?>(dto);
    }
}
