using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TransactionService.Infrastructure.Messaging
{
    public class RabbitMqHealthCheck(IOptions<RabbitMqSettings> options) : IHealthCheck
    {
        private readonly RabbitMqSettings _settings = options.Value;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.HostName,
                    Port = _settings.Port,
                    UserName = _settings.UserName,
                    Password = _settings.Password
                };

                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                return HealthCheckResult.Healthy("RabbitMQ connection is healthy");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    "RabbitMQ connection failed", ex);
            }
        }
    }
}