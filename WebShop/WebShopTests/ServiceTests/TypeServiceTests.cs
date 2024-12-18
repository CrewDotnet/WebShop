using AutoMapper;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;
using WebShopData.Repositories;
using Moq;
using FluentAssertions;
using WebShopApp.Services;

namespace WebShopTests.ServiceTests
{
    public class TypesServiceTests
    {
        private readonly Mock<ITypesRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TypesService _typesService;

        public TypesServiceTests()
        {
            _mockRepository = new Mock<ITypesRepository>();
            _mockMapper = new Mock<IMapper>();
            _typesService = new TypesService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsTypes_WhenTypesExist()
        {
            // Arrange
            var types = new List<ClothesType>
            {
                new ClothesType { Id = Guid.NewGuid(), Type = "Shirt" },
                new ClothesType { Id = Guid.NewGuid(), Type = "Pants" }
            };

            var typesResponse = types.Select(t => new ClothesTypesResponse
            {
                Id = t.Id,
                Type = t.Type
            }).ToList();

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(types);
            _mockMapper.Setup(m => m.Map<IEnumerable<ClothesTypesResponse>>(types)).Returns(typesResponse);

            // Act
            var result = await _typesService.GetAllAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNullOrEmpty();
            result.Value.Should().HaveCount(2);
            result.Value.Should().ContainSingle(x => x.Type == "Shirt");
            result.Value.Should().ContainSingle(x => x.Type == "Pants");
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFail_WhenNoTypesExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ClothesType>());

            // Act
            var result = await _typesService.GetAllAsync();

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "No orders found.");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsType_WhenTypeExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var type = new ClothesType { Id = id, Type = "Shirt" };
            var response = new ClothesTypesResponse { Id = id, Type = "Shirt" };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(type);
            _mockMapper.Setup(m => m.Map<ClothesTypesResponse>(type)).Returns(response);

            // Act
            var result = await _typesService.GetByIdAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenTypeDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ClothesType?)null);

            // Act
            var result = await _typesService.GetByIdAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Item not found.");
        }

        [Fact]
        public async Task AddAsync_ReturnsSuccess_WhenTypeIsAdded()
        {
            // Arrange
            var typeRequest = new ClothesTypeRequest { Type = "Shirt" };
            var type = new ClothesType { Id = Guid.NewGuid(), Type = "Shirt" };

            _mockMapper.Setup(m => m.Map<ClothesType>(typeRequest)).Returns(type);
            _mockRepository.Setup(r => r.AddAsync(type)).Returns(Task.CompletedTask);

            // Act
            var result = await _typesService.AddAsync(typeRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Type.Should().Be("Shirt");
            _mockRepository.Verify(r => r.AddAsync(type), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess_WhenTypeExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existingType = new ClothesType { Id = id, Type = "Shirt" };
            var updateRequest = new ClothesTypeRequest { Type = "Updated Shirt" };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingType);
            _mockMapper.Setup(m => m.Map(updateRequest, existingType));

            // Act
            var result = await _typesService.UpdateAsync(id, updateRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(r => r.UpdateAsync(existingType), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenTypeDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateRequest = new ClothesTypeRequest { Type = "Updated Shirt" };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ClothesType?)null);

            // Act
            var result = await _typesService.UpdateAsync(id, updateRequest);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Item not found.");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess_WhenTypeExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var typeToDelete = new ClothesType { Id = id, Type = "Shirt" };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(typeToDelete);
            _mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _typesService.DeleteAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenTypeDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ClothesType?)null);

            // Act
            var result = await _typesService.DeleteAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Item not found.");
        }
    }
}