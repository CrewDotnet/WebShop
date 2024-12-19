using FluentValidation.TestHelper;
using WebShopApp.Models.RequestModels;
using WebShopApp.Validators;

namespace WebShopTests.ValidatorsTests
{
    public class ClothesTypeRequestValidatorTests
    {
        private readonly ClothesTypeRequestValidator _validator;

        public ClothesTypeRequestValidatorTests()
        {
            _validator = new ClothesTypeRequestValidator();
        }

        [Fact]
        public void Validate_Type_ShouldHaveValidationError_WhenEmpty()
        {
            var model = new ClothesTypeRequest { Type = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Type)
                  .WithErrorMessage("Type is required.");
        }

        [Fact]
        public void Validate_Type_ShouldHaveValidationError_WhenTooShort()
        {
            var model = new ClothesTypeRequest { Type = "ab" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Type)
                  .WithErrorMessage("Type must be between 3 and 20 characters.");
        }

        [Fact]
        public void Validate_Type_ShouldHaveValidationError_WhenTooLong()
        {
            var model = new ClothesTypeRequest { Type = "ThisIsWayTooLongTypeeeee" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Type)
                  .WithErrorMessage("Type must be between 3 and 20 characters.");
        }

        [Fact]
        public void Validate_Type_ShouldNotHaveValidationError_WhenValid()
        {
            var model = new ClothesTypeRequest { Type = "T-shirt" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Type);
        }
    }
}
