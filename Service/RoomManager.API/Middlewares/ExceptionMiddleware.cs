using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microservice.Utility.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using RoomManager.API.Extensions;

namespace RoomManager.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string requestBodyString = null;
            try
            {
                context.Request.EnableBuffering();
                using (StreamReader reader = new StreamReader(context.Request.Body,
                                                              encoding: Encoding.UTF8,
                                                              detectEncodingFromByteOrderMarks: false,
                                                              bufferSize: 1024,
                                                              leaveOpen: true))
                {
                    requestBodyString = await reader.ReadToEndAsync().ConfigureAwait(false);
                    context.Request.Body.Position = 0;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogExceptionWithCommand(ex, requestBodyString);
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)exception.GetHttpCodeFromException();
            string requestPath = context.Request.GetDisplayUrl();
            ArcstoneErrorModel error = new ArcstoneErrorModel(exception, requestPath, context.Request.Method);
            return context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(error));
        }
    }
}
