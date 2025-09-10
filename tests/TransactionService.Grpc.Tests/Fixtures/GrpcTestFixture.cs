using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using TransactionService.Grpc;

namespace TransactionService.Grpc.Tests.Fixtures;

public class GrpcTestFixture : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Add test-specific overrides or mocks here if needed
        });

        return base.CreateHost(builder);
    }
}
