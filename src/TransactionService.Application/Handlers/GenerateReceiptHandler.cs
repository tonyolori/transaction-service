using MediatR;
using TransactionService.Infrastructure.DocGen;
using TransactionService.Application.Commands;

namespace TransactionService.Application.Handlers
{
    public class GenerateReceiptHandler(IDocumentGenerator docGen) : IRequestHandler<GenerateReceiptCommand, byte[]>
    {
        private readonly IDocumentGenerator _docGen = docGen;

        public async Task<byte[]> Handle(GenerateReceiptCommand req, CancellationToken ct)
        {
            var bytes = await _docGen.GenerateReceiptAsync(req.TransactionId, req.Format);
            return bytes;
        }
    }
}