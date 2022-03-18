using FluentValidation;
using MediatR;
using ProductServer.Application.SeedWork;

namespace ProductServer.Application.Behaviors
{
    public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, CommandResult>
        where TRequest : class, ICommand
        where TResponse : CommandResult
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public CommandValidationBehavior(IEnumerable<IValidator<TRequest>> validators) 
        {
            this.validators = validators;
        }

        public async Task<CommandResult> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<CommandResult> next)
        {
            if (!validators.Any())
                return await next().ConfigureAwait(false); ;

            ValidationContext<TRequest> context = new(request);

            IEnumerable<string> errors = validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage);

            if (errors.Any())
                return CommandResult.Error(errors);

            return await next().ConfigureAwait(false); ;
        }
    }
}
