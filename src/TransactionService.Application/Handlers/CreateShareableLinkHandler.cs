namespace TransactionService.Application.Handlers;

using MediatR;
using TransactionService.Application.Commands;
using TransactionService.Infrastructure.DocGen;
using TransactionService.Infrastructure.Signing;
using TransactionService.Infrastructure.Messaging;

public class CreateShareableLinkHandler : IRequestHandler<CreateShareableLinkCommand, string>
{
    private readonly IDocumentGenerator _docGen;
    private readonly ILinkSigner _signer;
    private readonly IInMemoryQueue _queue;

    public CreateShareableLinkHandler(IDocumentGenerator docGen, ILinkSigner signer, IInMemoryQueue queue)
    {
        _docGen = docGen;
        _signer = signer;
        _queue = queue;
    }

    public async Task<string> Handle(CreateShareableLinkCommand req, CancellationToken ct)
    {
        // For simplicity: generate synchronously small receipt, sign token and return gateway link
        // In production: check caches, size, async threshold, S3 upload etc.
        var bytes = await _docGen.GenerateReceiptAsync(req.TransactionId, "pdf");

        // Here we'd upload if needed; for this scaffold we store to in-memory queue and sign a token
        var token = _signer.SignToken(req.TransactionId, TimeSpan.FromSeconds(req.ExpiresInSeconds));
        // store metadata for verification later
        _queue.Enqueue(new InMemoryQueueItem { Id = req.TransactionId.ToString(), Payload = Convert.ToBase64String(bytes) });
        // return a Gateway-exposed link pattern (API Gateway will map /shared/receipt?token=... to service)
        return $"/shared/receipt?token={token}";
    }
}
