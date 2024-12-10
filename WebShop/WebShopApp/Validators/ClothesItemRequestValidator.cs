using FluentValidation;
using WebShopApp.Models.RequestModels;

namespace WebShopApp.Validators
{
    public class ClothesItemRequestValidator : AbstractValidator<ClothesItemRequest>
    {
        public ClothesItemRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 20).WithMessage("Name must be between 3 and 20 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.ClothesTypeId)
                .NotEmpty().WithMessage("ClothesTypeId is required.")
                .NotEqual(Guid.Empty).WithMessage("ClothesTypeId cannot be an empty GUID.");
        }
    }
}