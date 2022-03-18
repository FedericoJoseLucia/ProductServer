using MediatR;
using Microsoft.Extensions.Logging;
using ProductServer.Application.SeedWork;

namespace ProductServer.Application.Behaviors
{
    public class CommandLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, CommandResult>
        where TRequest : class, ICommand
        where TResponse : CommandResult
    {
        private readonly ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger;

        public CommandLoggingBehavior(ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<CommandResult> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<CommandResult> next)
        {
            LogCommandHandling(request);

            CommandResult result = await next().ConfigureAwait(false);

            LogCommandHandled(request, result);

            return result;
        }

        private void LogCommandHandling(TRequest request)
        {
            logger.LogDebug("HANDLING COMMAND {commandName}; PARAMETER: {@commandData}", typeof(TRequest).Name, request);
        }

        private void LogCommandHandled(TRequest request, CommandResult result)
        {
            if (result.IsSuccess)
                logger.LogInformation("COMMAND {commandName} HANDLED SUCCESSFULLY", typeof(TRequest).Name);
            else
                logger.LogError("COMMAND {commandName} HANDLED WITH ERRORS; PARAMETER: {@commandData}; ERRORS: {@errors}", typeof(TRequest).Name, request, result.Errors);
        }
    }
}
