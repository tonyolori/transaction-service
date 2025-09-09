namespace TransactionService.Application.Handlers;


using MediatR;
using TransactionService.Infrastructure.DocGen;

public record GenerateReceiptCommand(Guid TransactionId, string Format) : IRequest<byte[]>;

public class GenerateReceiptHandler : IRequestHandler<GenerateReceiptCommand, byte[]>
{
    private readonly IDocumentGenerator _docGen;
    public GenerateReceiptHandler(IDocumentGenerator docGen) => _docGen = docGen;
    public async Task<byte[]> Handle(GenerateReceiptCommand req, CancellationToken ct)
    {
        var bytes = await _docGen.GenerateReceiptAsync(req.TransactionId, req.Format);
        return bytes;
    }
}
