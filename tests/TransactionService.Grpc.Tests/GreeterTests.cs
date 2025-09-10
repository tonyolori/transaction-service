using Xunit;
using Grpc.Net.Client;
using TransactionService.Grpc;
using TransactionService.Grpc.Tests.Fixtures;
using System.Threading.Tasks;

public class GreeterServiceTests(GrpcTestFixture fixture) : IClassFixture<GrpcTestFixture>
{
    private readonly GrpcTestFixture _fixture = fixture;

    [Fact]
    public async Task SayHello_ReturnsExpectedMessage()
    {
        var channel = GrpcChannel.ForAddress(_fixture.Server.BaseAddress, new GrpcChannelOptions
        {
            HttpHandler = _fixture.Server.CreateHandler()
        });

        var client = new Greeter.GreeterClient(channel);

        var response = await client.SayHelloAsync(new HelloRequest { Name = "Test" });

        Assert.NotNull(response);
        Assert.Equal("Hello Test from Dev Commnunity Bank Transaction Service. Check the API specification for further guidance and next steps.", response.Message);
    }
}
