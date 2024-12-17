using Moq;
using WebShopApp.Services;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;
using WebShopData.Repositories;
using FluentAssertions;
using AutoMapper;

namespace WebShopTests.ServiceTests
{
    public class ClothesServiceT
    {
        private readonly Mock<IClothesRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ClothesService _clothesService;

        public ClothesServiceT()
        {
            _mockRepository = new Mock<IClothesRepository>();
            _mockMapper = new Mock<IMapper>();
            _clothesService = new ClothesService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsItems_WhenItemsExist()
        {
            // Arrange
            var clothes = new List<ClothesItem>
            {
                new ClothesItem { Id = Guid.NewGuid(), Name = "Top", Price = 19 },
                new ClothesItem { Id = Guid.NewGuid(), Name = "Shirt", Price = 25 }
            };

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(clothes);

            var clothesResponses = clothes.Select(item => new ClothesItemsResponse
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price
            });

            _mockMapper.Setup(m => m.Map<IEnumerable<ClothesItemsResponse>>(clothes)).Returns(clothesResponses);

            // Act
            var result = await _clothesService.GetAllAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNullOrEmpty();
            result.Value.Should().HaveCount(2);
            result.Value.Should().ContainSingle(x => x.Name == "Top");
            result.Value.Should().ContainSingle(x => x.Name == "Shirt");
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFail_WhenNoItemsExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ClothesItem>());

            // Act
            var result = await _clothesService.GetAllAsync();

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "No orders found.");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsItem_WhenItemExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var clothesItem = new ClothesItem { Id = id, Name = "Shirt", Price = 25 };
            var response = new ClothesItemsResponse { Id = id, Name = "Shirt", Price = 25 };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(clothesItem);
            _mockMapper.Setup(m => m.Map<ClothesItemsResponse>(clothesItem)).Returns(response);

            // Act
            var result = await _clothesService.GetByIdAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenItemDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ClothesItem?)null);

            // Act
            var result = await _clothesService.GetByIdAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Nema takvog artikla bree");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess_WhenItemExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existingItem = new ClothesItem { Id = id, Name = "Shirt", Price = 25 };
            var updateRequest = new ClothesItemRequest { Name = "Updated Shirt", Price = 30 };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingItem);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<ClothesItem>())).ReturnsAsync(true);

            // Act
            var result = await _clothesService.UpdateAsync(id, updateRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<ClothesItem>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenItemDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateRequest = new ClothesItemRequest { Name = "Updated Shirt", Price = 30 };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ClothesItem?)null);

            // Act
            var result = await _clothesService.UpdateAsync(id, updateRequest);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Nema takvog artikla bree");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess_WhenItemExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var itemToDelete = new ClothesItem { Id = id, Name = "Shirt", Price = 25 };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(itemToDelete);
            _mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _clothesService.DeleteAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenItemDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ClothesItem?)null);

            // Act
            var result = await _clothesService.DeleteAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Nema takvog artikla bree");
        }

        [Fact]
        public async Task AddAsync_ReturnsSuccess_WhenItemIsAdded()
        {
            // Arrange
            var newItemRequest = new ClothesItemRequest { Name = "New Shirt", Price = 35 };
            var newItem = new ClothesItem { Id = Guid.NewGuid(), Name = "New Shirt", Price = 35 };

            _mockMapper.Setup(m => m.Map<ClothesItem>(newItemRequest)).Returns(newItem);
            _mockRepository.Setup(r => r.AddAsync(newItem)).Returns(Task.CompletedTask);

            // Act
            var result = await _clothesService.AddAsync(newItemRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be("New Shirt");
            result.Value.Price.Should().Be(35);
            _mockRepository.Verify(r => r.AddAsync(newItem), Times.Once);
        }
    }
}
