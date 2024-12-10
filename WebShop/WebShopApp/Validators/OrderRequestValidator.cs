using FluentValidation;
using WebShopApp.Models.RequestModels;

namespace WebShopApp.Validators
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.")
                .NotEqual(Guid.Empty).WithMessage("CustomerId cannot be an empty GUID.");

            RuleFor(x => x.ClothesItemsId)
                .NotEmpty().WithMessage("At least one ClothesItemId is required.")
                .ForEach(id => id.NotEqual(Guid.Empty).WithMessage("ClothesItemId cannot be an empty GUID."));
        }
    }
}