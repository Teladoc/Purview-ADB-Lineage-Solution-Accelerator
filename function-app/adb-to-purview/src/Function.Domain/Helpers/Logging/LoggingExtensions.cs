using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Function.Domain.Helpers.Logging
{
    public static class LoggingExtensions
    {
        public static void LogError(this ILogger logger, Exception ex, int errorCode, string message, params object[] args)
        {
            var newArgs = args.Prepend(errorCode).ToArray();
            try
            {
                var logMessage = "Error Code: {code}. Message: " + message;
                logger.LogError(ex, logMessage, newArgs);
            }
            catch (Exception innerex)
            {                
                logger.LogError(innerex, "Error occurred logging error. Original Error: {error}. Custom Message: {message}. ArgCount: {argCount}", ex.ToString(), message, newArgs.Length);
            }
        }

        public static void LogWarning(this ILogger logger, int warningCode, string message, params object[] args)
        {
            var newArgs = args.Prepend(warningCode).ToArray();
            try
            {
                var logMessage = "Warning Code: {code}. Message: " + message;
                logger.LogWarning(logMessage, newArgs);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred logging warning. Original Message: {message}.", message);
            }            
        }
    }
}