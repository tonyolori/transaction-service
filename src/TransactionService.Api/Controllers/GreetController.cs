using Microsoft.AspNetCore.Mvc;
using TransactionService.Grpc;

namespace TransactionService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GreetController(Greeter.GreeterClient greeter) : ControllerBase
    {
        private readonly Greeter.GreeterClient _greeter = greeter;

        [HttpGet("{name}")]
        public async Task<IActionResult> SayHello(string name)
        {
            var reply = await _greeter.SayHelloAsync(new HelloRequest { Name = name });
            return Ok(reply.Message);
        }
    }
}
