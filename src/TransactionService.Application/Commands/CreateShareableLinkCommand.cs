namespace TransactionService.Application.Commands;

using MediatR;

public record CreateShareableLinkCommand(Guid TransactionId, int ExpiresInSeconds) : IRequest<string>;
