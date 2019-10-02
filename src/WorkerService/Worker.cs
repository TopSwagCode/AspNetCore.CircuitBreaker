using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace WorkerService
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
            

            while (!stoppingToken.IsCancellationRequested)
            {
                // Attempt #1
                //await BadLogic(stoppingToken);
                
                // Attempt #2
                //await MakeTheBadManGoAway(stoppingToken);

                // Attempt #3
                //await TryingMakesItWorse(stoppingToken);
                
                // Attempt #4
                //await GettingThere(stoppingToken);
                
                // Attempt #5
                //await GettingCloser(stoppingToken);
                
                // Attempt #6
                //SetupCircuitBreaker();
                //await CallBadLogicWrappedInCircuitBreaker(stoppingToken);
                
                /*
                 * Future -> Add Fallback to call other methods / services
                 */
            }
        }

        // Attempt #1
        private async Task BadLogic(CancellationToken stoppingToken)
        {

            if (DateTime.Now.Second > 30)
            {
                throw new Exception();
            }

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);

        }
        
        // Attempt #2
        private async Task MakeTheBadManGoAway(CancellationToken stoppingToken)
        {
            try
            {
                if (DateTime.Now.Second > 30)
                {
                    throw new Exception();
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Worker failed at at: {time}", DateTimeOffset.Now);
            }
        }
        
        // Attempt #3
        private async Task TryingMakesItWorse(CancellationToken stoppingToken)
        {
            await Policy.Handle<Exception>()
                .RetryAsync( 
                3,
                (e,i) => _logger.LogInformation($"Error '{e.Message}' at retry #{i}"))
                .ExecuteAsync( () => BadLogic(stoppingToken));
        }
        
        // Attempt #4
        private async Task GettingThere(CancellationToken stoppingToken)
        {
            // Dies after 3 retries.
            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (e,i) => _logger.LogInformation($"Error '{e.Message}' at retry #{i}"))
                .ExecuteAsync(() => BadLogic(stoppingToken));
        }
        
        // Attempt #5
        private async Task GettingCloser(CancellationToken stoppingToken)
        {
            await Policy.Handle<Exception>()
                .WaitAndRetryForeverAsync(
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (e,i) => _logger.LogInformation($"Error '{e.Message}' at retry #{i}"))
                .ExecuteAsync(() => BadLogic(stoppingToken));
        }
        
        // Attempt #6
        private async Task CallBadLogicWrappedInCircuitBreaker(CancellationToken stoppingToken)
        {
            try
            {
                await circuitPolicy.ExecuteAsync(() => BadLogic(stoppingToken));
            }
            catch (Exception e)
            {
                await Task.Delay(1000);
                Console.WriteLine(e.Message);
            }
            

            
        }

        public AsyncCircuitBreakerPolicy circuitPolicy { get; set; }
        
        private void SetupCircuitBreaker()
        {
            circuitPolicy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(
                    3,
                    TimeSpan.FromSeconds(10),
                    (exception, span) =>
                        Console.WriteLine(String.Format("Circuit breaking for {0} ms due to {1}",
                            span.TotalMilliseconds, exception.Message)),
                    () => _logger.LogInformation($"Close circuit and allow calls again"),
                    () => _logger.LogInformation($"HalfOpen circuit allow test call again"));
        }
    }
}