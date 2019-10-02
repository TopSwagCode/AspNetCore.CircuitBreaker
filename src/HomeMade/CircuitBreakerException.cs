using System;

namespace HomeMade
{
    public class CircuitBreakerException : Exception
    {
        public CircuitBreakerException() : base()
        {
            
        }

        public CircuitBreakerException(string message) : base(message)
        {
            
        }
    }
}