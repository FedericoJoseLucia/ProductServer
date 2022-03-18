using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ProductServer.Application.Behaviors
{
    public class StopwatchLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<StopwatchLoggingBehavior<TRequest, TResponse>> logger;

        public StopwatchLoggingBehavior(ILogger<StopwatchLoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            TResponse response = await next().ConfigureAwait(false);

            stopwatch.Stop();

            LogRequestExecutionTime(stopwatch.ElapsedMilliseconds);

            return response;
        }

        private void LogRequestExecutionTime(long miliseconds)
        {
            logger.LogDebug("REQUEST {requestName} FINISHED RUNNING; TIME TAKEN: {@miliseconds}", typeof(TRequest).Name, miliseconds);
        }
    }
}
