using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HomeMade
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            CircuitBreaker cb = new CircuitBreaker();
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);

                try
                {
                    await cb.ExecuteAsync(() => BadLogic(stoppingToken));
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"Worker failed with: {e.Message} at: {DateTimeOffset.Now}");
                }
                
            }
        }
        
        private async Task BadLogic(CancellationToken stoppingToken)
        {

            if (DateTime.Now.Second > 30)
            {
                throw new Exception("ERROR");
            }

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(5, stoppingToken);

        }
    }
}