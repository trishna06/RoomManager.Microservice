using System;
using System.Net;
using System.Text;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace RoomManager.API.Extensions
{
    public static class ExceptionExtensions
    {
        public static HttpStatusCode GetHttpCodeFromException(this Exception ex)
        {
            if (ex is InvalidOperationException or ArgumentException or ValidationException)
                return HttpStatusCode.BadRequest;

            string errorType = ex.GetType().Name.ToLowerInvariant();
            if (errorType.Contains("notfound"))
                return HttpStatusCode.NotFound;

            if (errorType.Contains("existed"))
                return HttpStatusCode.Conflict;

            return HttpStatusCode.InternalServerError;
        }

        public static void LogException(this ILogger logger, Exception ex)
        {
            if (logger != null)
            {
                string exceptionType = ex.GetType().Name;
                logger.LogError(ex.FormatExceptionForLogging(), exceptionType);
            }
        }

        public static void LogExceptionWithCommand(this ILogger logger, Exception ex, object command)
        {
            if (logger != null)
            {
                string exceptionType = ex.GetType().Name;
                string message = "Command: {@Command}\r\n" + ex.FormatExceptionForLogging();
                logger.LogError(message, command, exceptionType);
            }
        }

        private static string FormatExceptionForLogging(this Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new();

            sb.AppendLine("Exception: {@ExceptionType}")
              .AppendLine(ex.ToString().EscapeFormat());

            Exception baseException = ex.GetBaseException();

            if (baseException != ex)
            {
                sb.AppendLine("=== Base Exception === ")
                  .AppendLine(baseException.ToString().EscapeFormat());
            }

            return sb.ToString();
        }

        private static string EscapeFormat(this string str)
        {
            return str.Replace("{", "{{").Replace("}", "}}");
        }
    }
}
