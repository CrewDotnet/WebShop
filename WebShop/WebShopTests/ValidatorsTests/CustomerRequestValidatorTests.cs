using FluentValidation.TestHelper;
using WebShopApp.Models.RequestModels;
using WebShopApp.Validators;

namespace WebShopTests.ValidatorsTests
{
    public class CustomerRequestValidatorTests
    {
        private readonly CustomerRequestValidator _validator;

        public CustomerRequestValidatorTests()
        {
            _validator = new CustomerRequestValidator();
        }

        [Fact]
        public void Name_ShouldHaveValidationError_WhenEmpty()
        {
            var model = new CustomerRequest { Name = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Name is required.");
        }

        [Fact]
        public void Name_ShouldHaveValidationError_WhenTooShort()
        {
            var model = new CustomerRequest { Name = "Jo" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Name must be between 3 and 20 characters.");
        }

        [Fact]
        public void Name_ShouldHaveValidationError_WhenTooLong()
        {
            var model = new CustomerRequest { Name = "ThisNameIsDefinitelyTooLong" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Name must be between 3 and 20 characters.");
        }

        [Fact]
        public void Name_ShouldNotHaveValidationError_WhenValid()
        {
            var model = new CustomerRequest { Name = "Alice" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }
    }
}