using FluentValidation.TestHelper;
using WebShopApp.Models.RequestModels;
using WebShopApp.Validators;

namespace WebShopTests.ValidatorsTests
{
    public class ClothesItemRequestValidatorTests
    {
        private readonly ClothesItemRequestValidator _validator;

        public ClothesItemRequestValidatorTests()
        {
            _validator = new ClothesItemRequestValidator();
        }

        [Fact]
        public void Validate_Name_ShouldHaveValidationError_WhenEmpty()
        {
            var model = new ClothesItemRequest { Name = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Name is required.");
        }

        [Fact]
        public void Validate_Price_ShouldHaveValidationError_WhenZeroOrNegative()
        {
            var model = new ClothesItemRequest { Price = -1 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Price)
                  .WithErrorMessage("Price must be greater than 0.");
        }

        [Fact]
        public void Validate_ClothesTypeId_ShouldHaveValidationError_WhenEmptyGuid()
        {
            var model = new ClothesItemRequest { ClothesTypeId = Guid.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ClothesTypeId)
                  .WithErrorMessage("ClothesTypeId cannot be an empty GUID.");
        }

        [Fact]
        public void Validate_Name_ShouldNotHaveValidationError_WhenValid()
        {
            var model = new ClothesItemRequest { Name = "T-shirt" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validate_ShouldNotHaveAnyValidationError_WhenValid()
        {
            var model = new ClothesItemRequest
            {
                Name = "Valid Name",
                Price = 20.5m,
                ClothesTypeId = Guid.NewGuid()
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}