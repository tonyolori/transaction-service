using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TransactionService.Worker
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Worker running at: " + DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken); // run every second
            }
        }
    }
}
