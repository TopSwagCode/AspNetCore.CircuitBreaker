# TopSwagCode.DesignPatterns.CircuitBreaker

Small sample project for how to implement CircuitBreaker pattern in Dotnet Core 3.0 with HttpClientFactory and Polly. Below can be seen a gif showing the project working (Click on image to view full size).

* On the left the sample ExternalService showing Logger for requests.
* Upper right OurService calling the ExternalService.
* Below right the ExternalService calling it self.

![Sample.gif](Sample.gif)

We can see the service working on the first call. Afterwards there is 3 call's that fail. Then we open the circuit and stop calling the service and instead instantly reply with a BrokenCircuitException. Ensureing our service does not waste resource calling a broken service and letting the external service get a chance of getting back into a stable state before we start using it again. 

If you want to read more about the Circuit Breaker Pattern you can find my blog post about it here: [https://topswagcode.com/2016/02/07/Circuit-Breaker-Pattern/](https://topswagcode.com/2016/02/07/Circuit-Breaker-Pattern/)
