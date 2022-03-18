using FluentValidation;

namespace ProductServer.Application.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Id).NotEmpty();

            When(x => x.Denomination is not null, () =>
            {
                RuleFor(x => x.Denomination).NotEmpty();
                RuleFor(x => x.Denomination).MaximumLength(100);
            });

            When(x => x.Price is not null, () =>
            {
                RuleFor(x => x.Price).LessThan(0);
            });
        }
    }
}
