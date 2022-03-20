using FluentValidation;
using FluentValidation.Results;
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

            var validationResults = await ValidateAsync(request, cancellationToken).ConfigureAwait(false);

            var errors = GetErrors(validationResults);

            if (errors.Any())
                return CommandResult.Error(errors);

            return await next().ConfigureAwait(false); ;
        }

        private async Task<IEnumerable<ValidationResult>> ValidateAsync(TRequest request, CancellationToken cancellationToken)
        {
            ValidationContext<TRequest> context = new(request);

            var validationTasks = validators
                .Select(validator => validator.ValidateAsync(context, cancellationToken));

            return await Task.WhenAll(validationTasks).ConfigureAwait(false);
        }

        private static IEnumerable<string> GetErrors(IEnumerable<ValidationResult> validationResults)
        {
            foreach (var validationResult in validationResults)
                foreach (var error in validationResult.Errors)
                    yield return error.ErrorMessage;
        }
    }
}
