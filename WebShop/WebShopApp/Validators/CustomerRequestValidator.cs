using FluentValidation;
using WebShopApp.Models.RequestModels;

namespace WebShopApp.Validators
{
    public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
    {        
        public CustomerRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 20).WithMessage("Name must be between 3 and 20 characters.");
        }
    }
}