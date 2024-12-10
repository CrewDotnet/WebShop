using FluentValidation;
using WebShopApp.Models.RequestModels;

namespace WebShopApp.Validators
{
    public class ClothesTypeRequestValidator : AbstractValidator<ClothesTypeRequest>
    {
        public ClothesTypeRequestValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.")
                .Length(3, 20).WithMessage("Type must be between 3 and 20 characters.");
        }
    }
}