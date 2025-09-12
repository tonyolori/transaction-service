using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TransactionService.Domain.Events;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Messaging
{
    public class TransactionEventConsumer : ITransactionEventConsumer
    {
        private readonly RabbitMqSettings _settings;
        private readonly ILogger<TransactionEventConsumer> _logger;
        private IConnection? _connection;
        private IChannel? _channel;

        public TransactionEventConsumer(
            IOptions<RabbitMqSettings> options,
            ILogger<TransactionEventConsumer> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var transactionEvent = JsonSerializer.Deserialize<TransactionCreatedEvent>(message);

                    _logger.LogInformation("Consumed TransactionCreatedEvent: {TransactionId}", transactionEvent?.TransactionId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from RabbitMQ.");
                }

                await Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(
                queue: _settings.QueueName,
                autoAck: true,
                consumer: consumer);

            _logger.LogInformation("Started consuming from queue: {QueueName}", _settings.QueueName);
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_channel != null)
                    await _channel.DisposeAsync();

                if (_connection != null)
                    await _connection.DisposeAsync();

                _logger.LogInformation("RabbitMQ consumer disposed.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error during RabbitMQ consumer disposal.");
            }
        }
    }
}
