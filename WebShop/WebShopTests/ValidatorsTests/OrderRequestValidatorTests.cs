using FluentValidation.TestHelper;
using WebShopApp.Models.RequestModels;
using WebShopApp.Validators;

namespace WebShopTests.ValidatorsTests
{
    public class OrderRequestValidatorTests
    {
        private readonly OrderRequestValidator _validator;

        public OrderRequestValidatorTests()
        {
            _validator = new OrderRequestValidator();
        }

        [Fact]
        public void CustomerId_ShouldHaveValidationError_WhenEmpty()
        {
            var model = new OrderRequest { CustomerId = Guid.Empty, ClothesItemsId = new List<Guid> { Guid.NewGuid() } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CustomerId)
                  .WithErrorMessage("CustomerId cannot be an empty GUID.");
        }

        [Fact]
        public void CustomerId_ShouldNotHaveValidationError_WhenValid()
        {
            var model = new OrderRequest { CustomerId = Guid.NewGuid(), ClothesItemsId = new List<Guid> { Guid.NewGuid() } };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.CustomerId);
        }

        [Fact]
        public void ClothesItemsId_ShouldHaveValidationError_WhenEmpty()
        {
            var model = new OrderRequest { CustomerId = Guid.NewGuid(), ClothesItemsId = new List<Guid>() };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ClothesItemsId)
                  .WithErrorMessage("At least one ClothesItemId is required.");
        }

        [Fact]
        public void ClothesItemsId_ShouldHaveValidationError_WhenContainsEmptyGuid()
        {
            var model = new OrderRequest { CustomerId = Guid.NewGuid(), ClothesItemsId = new List<Guid> { Guid.NewGuid(), Guid.Empty } };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor("ClothesItemsId[1]")
                  .WithErrorMessage("ClothesItemId cannot be an empty GUID.");
        }

        [Fact]
        public void ClothesItemsId_ShouldNotHaveValidationError_WhenAllAreValid()
        {
            var model = new OrderRequest { CustomerId = Guid.NewGuid(), ClothesItemsId = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() } };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.ClothesItemsId);
        }
    }
}
