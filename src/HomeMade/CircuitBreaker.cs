using System;
using System.Threading.Tasks;

namespace HomeMade
{
    public class CircuitBreaker
    {
        private int _currentNumberOfErrors = 0;
        private int _numberOfErrorsBeforeOpen = 3;
        private int _circuitState = 0; // 0 = closed, 1 = half-open, 2 Open
        private DateTime? _allowCallsAgain = null;

        public CircuitBreaker()
        {
            
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            if (AllowedToMakeCall() == false)
            {
                throw new CircuitBreakerException("Open Circuit. Stopping calls");
            }

            try
            {
                await action();
            }
            catch(Exception e)
            {
                
                RecordFailure();
                throw;
            }

            RecordSuccess();
        }

        private void RecordSuccess()
        {
            if (CircuitIsClosed())
            {
                _allowCallsAgain = null;
                _currentNumberOfErrors = 0;
            } 
            else if (CircuitIsHalfOpen())
            {
                _circuitState = 0;
                _currentNumberOfErrors = 0;
            }
            else
            {
                _circuitState = 1;    
            }
        }

        private void RecordFailure()
        {
            _currentNumberOfErrors++;
            _allowCallsAgain = DateTime.Now;

            if (_currentNumberOfErrors >= _numberOfErrorsBeforeOpen)
            {
                _circuitState = 2;
                _allowCallsAgain = DateTime.Now.AddSeconds(5);
            }
        }

        private bool AllowedToMakeCall()
        {
            if (CircuitIsClosed())
            {
                return true;
            }
            else if (CircuitIsHalfOpen())
            {
                return true;
            }
            
            return false;
        }

        private bool CircuitIsHalfOpen()
        {
            if (_allowCallsAgain != null)
            {
                return DateTime.Now >= _allowCallsAgain.Value;
            }

            return false;
        }

        private bool CircuitIsClosed()
        {
            return _circuitState == 0;
        }
    }
}