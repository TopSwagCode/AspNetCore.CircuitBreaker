# TopSwagCode.DesignPatterns.CircuitBreaker

Small sample project for how to implement CircuitBreaker pattern in Dotnet Core 3.0 with HttpClientFactory and Polly. Below can be seen a gif showing the project working (Click on image to view full size).

* On the left the sample ExternalService showing Logger for requests.
* Upper right OurService calling the ExternalService.
* Below right the ExternalService calling it self.

![Sample.gif](Sample.gif)

We can see the service working on the first call. Afterwards there is 3 call's that fail. Then we open the circuit and stop calling the service and instead instantly reply with a BrokenCircuitException. Ensureing our service does not waste resource calling a broken service and letting the external service get a chance of getting back into a stable state before we start using it again. 

If you want to read more about the Circuit Breaker Pattern you can find my blog post about it here: [https://topswagcode.com/2016/02/07/Circuit-Breaker-Pattern/](https://topswagcode.com/2016/02/07/Circuit-Breaker-Pattern/)

For the project I created 2 Services.

* WeatherService
* WeatherServiceWithRetry

I add CircuitBreaker and Retry logic for them using [Polly](https://github.com/App-vNext/Polly).

``` csharp
    services.AddHttpClient<WeatherServiceWithRetry>( client =>
    {
        client.BaseAddress = new Uri("https://localhost:44366/");
    })
    .AddPolicyHandler(ServicePolicies.GetCircuitBreakerPolicy())
    .AddPolicyHandler(ServicePolicies.GetRetryPolicy())
    .SetHandlerLifetime(TimeSpan.FromMinutes(5));


    services.AddHttpClient<WeatherService>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:44366/");
    })
    .AddPolicyHandler(ServicePolicies.GetCircuitBreakerPolicy())
    .SetHandlerLifetime(TimeSpan.FromMinutes(5));
```

To reuse my policies for multiple HttpClient's, I have put the policies in their own ServicePolicies.cs file. 

``` csharp
    public static class ServicePolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(10));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
```

The retry policy can be as creative as you want it. In my sample I am using Math.Pow with the number of retry attemps.
So I will retry 3 times at: 2, 4 and 8 seconds.

# Supported by:

![https://www.jetbrains.com/?from=TopSwagCode](jetbrains.svg)