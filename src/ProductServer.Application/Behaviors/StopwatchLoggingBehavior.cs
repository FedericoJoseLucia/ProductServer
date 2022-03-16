using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ProductServer.Application.Behaviors
{
    public class StopwatchLoggingBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    {
        private readonly ILogger<StopwatchLoggingBehavior<TRequest, TResult>> logger;

        public StopwatchLoggingBehavior(ILogger<StopwatchLoggingBehavior<TRequest, TResult>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            TResult result = await next().ConfigureAwait(false);

            stopwatch.Stop();

            LogCommandExecutionTime(stopwatch.ElapsedMilliseconds);

            return result;
        }

        private void LogCommandExecutionTime(long miliseconds)
        {
            logger.LogDebug("COMMAND {commandName} FINISHED RUNNING; TIME TAKEN: {@miliseconds}", typeof(TRequest).Name, miliseconds);
        }
    }
}
