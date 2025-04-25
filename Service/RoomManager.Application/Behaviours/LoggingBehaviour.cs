using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EventBusUtility.Helper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace RoomManager.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Stopwatch sw = new Stopwatch();
            _logger.LogInformation("----- Handling command {CommandName} ({@Command})", request.GetGenericTypeName(), request);
            sw.Start();
            TResponse response = await next();
            sw.Stop();
            _logger.LogInformation("----- Command {CommandName} handled - response: {@Response}. The handler took {@Timespan} ms.", request.GetGenericTypeName(), response, sw.ElapsedMilliseconds.ToString());
            return response;
        }
    }
}
