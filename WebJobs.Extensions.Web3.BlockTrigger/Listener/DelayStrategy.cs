using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.Web3.BlockTrigger.Listener
{
    public class DelayStrategy: IDelayStrategy
    {
        private readonly TimeSpan _minInterval;
        private readonly TimeSpan _maxInterval;
        private TimeSpan _currentInterval;
        private double _backOffExponent;

        public DelayStrategy(TimeSpan minInterval, TimeSpan maxInterval)
        {
            ValidateIntervalRange(minInterval, maxInterval);
            _minInterval = minInterval;
            _maxInterval = maxInterval;

            _currentInterval = minInterval;
        }

        public TimeSpan GetNextDelay(bool executionSuccess)
        {
            if (executionSuccess)
            {
                _currentInterval = _minInterval;
                _backOffExponent = 0;
                return _currentInterval;
            }

            double factor = Math.Pow(2.0, _backOffExponent);

            TimeSpan nextInterval = _currentInterval * factor;

            if (nextInterval < _minInterval)
                return _minInterval;

            if (nextInterval >= _maxInterval)
                return _maxInterval;

            _backOffExponent += 1.0;

            return nextInterval;
        }

        private void ValidateIntervalRange(TimeSpan minInterval, TimeSpan maxInterval)
        {
            if (minInterval.Ticks < 100)
                throw new ArgumentOutOfRangeException("minInterval", "minInterval must be at least 100 milisec.");

            if (maxInterval.Ticks < 1000)
                throw new ArgumentOutOfRangeException("maxInterval", "maxInterval must be at least 1 sec");

            if (minInterval > maxInterval)
                throw new ArgumentException("minInterval must be less than maxInterval");
        }
    }
}
