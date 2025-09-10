using Grpc.Core;
using TransactionService.Grpc;

namespace TransactionService.Grpc.Services;

public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger = logger;

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Request gotten from " + request.Name + " saying hello!");

        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name + " from Dev Commnunity Bank Transaction Service. Check the API specification for further guidance and next steps."
        });
    }
}
