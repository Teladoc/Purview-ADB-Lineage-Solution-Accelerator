using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UnitTests.Mocks
{
    public class FakeLogger<TLog>(ILogger<TLog> logger) : ILogger where TLog : class, new()
    {
        private readonly ILogger<TLog> _logger = logger;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return _logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            try
            {
                _logger.Log(logLevel, eventId, state, exception, formatter);
            }
            catch (Exception ex) when (ex.Message.Contains("An error occurred while writing to logger(s). (Index (zero based) must be greater than or equal to zero and less than the size of the argument list.)"))
            {
                ArgumentExceptionRaised = true;
                throw;
            }
        }

        public bool ArgumentExceptionRaised { get; private set; } = false;
    }
}