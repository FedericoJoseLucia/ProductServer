using FluentValidation;

namespace ProductServer.Application.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Denomination).NotNull();
            RuleFor(x => x.Denomination).NotEmpty();
            RuleFor(x => x.Denomination).MaximumLength(100);

            RuleFor(x => x.Price).LessThan(0);
        }
    }
}
