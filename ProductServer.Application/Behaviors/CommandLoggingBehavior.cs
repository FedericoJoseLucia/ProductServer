using MediatR;
using Microsoft.Extensions.Logging;
using ProductServer.Application.SeedWork;

namespace ProductServer.Application.Behaviors
{
    public class CommandLoggingBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult> 
        where TRequest : IRequest<TResult>
        where TResult : ICommandResult
    {
        private readonly ILogger<CommandLoggingBehavior<TRequest, TResult>> logger;

        public CommandLoggingBehavior(ILogger<CommandLoggingBehavior<TRequest, TResult>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            LogCommandHandling(request);

            TResult result = await next().ConfigureAwait(false);

            LogCommandHandled(request, result);

            return result;
        }

        private void LogCommandHandling(TRequest request)
        {
            logger.LogDebug("HANDLING COMMAND {commandName}; PARAMETER: {@commandData}", typeof(TRequest).Name, request);
        }

        private void LogCommandHandled(TRequest request, TResult result)
        {
            if (result.IsSuccess)
                logger.LogInformation("COMMAND {commandName} HANDLED SUCCESSFULLY", typeof(TRequest).Name);
            else
                logger.LogError("COMMAND {commandName} HANDLED WITH ERROR; PARAMETER: {@commandData}; RESULT MESSAGE: {@commandResultMessage}", typeof(TRequest).Name, request, result.Message);
        }
    }
}
