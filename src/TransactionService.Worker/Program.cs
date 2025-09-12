using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using TransactionService.Worker;
using TransactionService.Infrastructure.Extensions;
using TransactionService.Infrastructure.Messaging;
using TransactionService.Domain.Interfaces;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInfrastructure(context.Configuration);
        services.AddHostedService<TransactionEventConsumerWorker>();
        services.AddScoped<ITransactionEventConsumer, TransactionEventConsumer>();
    })
    .Build();

await host.RunAsync();
