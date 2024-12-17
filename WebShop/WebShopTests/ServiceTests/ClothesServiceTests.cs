using AutoFixture;
using Moq;
using WebShopApp.Services;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;
using WebShopData.Repositories;
using AutoMapper;
using FluentAssertions;
using WebShopApp.Models.RequestModels;

namespace WebShopTests.ServiceTests
{
    public class ClothesServiceTests
    {
        private readonly Mock<IClothesRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ClothesService _clothesService;
        private readonly IFixture _fixture; // AutoFixture instanca

        public ClothesServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                            .ForEach(b => _fixture.Behaviors.Remove(b)); // Ukloni ThrowingRecursionBehavior
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior()); // Dodaj OmitOnRecursionBehavior

            _mockRepository = _fixture.Freeze<Mock<IClothesRepository>>();
            _mockMapper = _fixture.Freeze<Mock<IMapper>>();
            _clothesService = new ClothesService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsItems_WhenItemsExist()
        {
            // Arrange
            var clothes = _fixture.CreateMany<ClothesItem>(3).ToList(); // AutoFixture kreira listu od 3 ClothesItem

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(clothes);

            var clothesResponses = _fixture.Build<ClothesItemsResponse>()
                                            .With(x => x.Id, clothes[0].Id) // Primer povezivanja
                                            .CreateMany(3);

            _mockMapper.Setup(m => m.Map<IEnumerable<ClothesItemsResponse>>(clothes))
                       .Returns(clothesResponses);

            // Act
            var result = await _clothesService.GetAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Count());
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFail_WhenNoItemsExist()
        {
            // Arrange
            var emptyList = new List<ClothesItem>();
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(emptyList);

            // Act
            var result = await _clothesService.GetAllAsync();

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains(result.Errors, e => e.Message == "No orders found.");
        }

        // TEST: GetByIdAsync - Successfully returns an item
        [Fact]
        public async Task GetByIdAsync_ReturnsItem_WhenItemExists()
        {
            // Arrange
            var fixture = new Fixture();
            var id = Guid.NewGuid();

            // Kreiramo ClothesType koji je povezan sa ClothesItem
            var clothesType = fixture.Create<ClothesType>();

            // Kreiramo ClothesItem sa explicitno dodeljenim ClothesType
            var clothesItem = fixture.Build<ClothesItem>()
                                    .With(c => c.Id, id)
                                    .With(c => c.ClothesType, clothesType)  // Dodeljujemo ClothesType objekat
                                    .Create();

            // Kreiramo ClothesItemsResponse kao odgovarajući response model
            var response = fixture.Create<ClothesItemsResponse>();

            // Postavljamo mock na repository da vraća clothingItem
            _mockRepository.Setup(r => r.GetByIdAsync(id))
                        .ReturnsAsync(clothesItem);

            // Postavljamo mock na mapper da mapira ClothesItem u odgovarajući response model
            _mockMapper.Setup(m => m.Map<ClothesItemsResponse?>(clothesItem))
                    .Returns(response);

            // Act
            var result = await _clothesService.GetByIdAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(response);
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        // TEST: GetByIdAsync - Returns Failure when item doesn't exist
        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenItemDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetByIdAsync(id))
                        .ReturnsAsync((ClothesItem?)null);

            // Act
            var result = await _clothesService.GetByIdAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == "Nema takvog artikla bree");
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        // TEST: AddAsync - Successfully adds a new item
        [Fact]
        public async Task AddAsync_SuccessfulCase_ReturnsSuccess()
        {
            // Arrange
            var fixture = new Fixture();
            var clothesRequest = fixture.Create<ClothesItemRequest>(); // Kreira ClothesItemRequest sa AutoFixture

            var clothesItem = new ClothesItem // Simuliraj ClothesItem nakon mape
            {
                Id = Guid.NewGuid(),
                Name = clothesRequest.Name,
                Price = clothesRequest.Price
            };

            _mockMapper.Setup(m => m.Map<ClothesItem>(clothesRequest))
                .Returns(clothesItem); // Mapiraj request na model za bazu

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<ClothesItem>()))
               .Returns(Task.CompletedTask);

            // Act
            var result = await _clothesService.AddAsync(clothesRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(r => r.AddAsync(clothesItem), Times.Once);
        }

        // TEST: UpdateAsync - Successfully updates an item
        [Fact]
        public async Task UpdateAsync_ReturnsSuccess_WhenItemIsUpdated()
        {
            // Arrange
            var fixture = new Fixture();
            var id = Guid.NewGuid();
            var request = fixture.Create<ClothesItemRequest>();
            var existingItem = fixture.Build<ClothesItem>()
                                    .With(c => c.Id, id)
                                    .Create();

            _mockRepository.Setup(r => r.GetByIdAsync(id))
                        .ReturnsAsync(existingItem);

            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<ClothesItem>()))
                        .ReturnsAsync(true);

            _mockMapper.Setup(m => m.Map(request, existingItem));

            // Act
            var result = await _clothesService.UpdateAsync(id, request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(existingItem), Times.Once);
        }

        // TEST: UpdateAsync - Returns Failure when item doesn't exist
        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenItemDoesNotExist()
        {
            // Arrange
            var fixture = new Fixture();
            var id = Guid.NewGuid();
            var request = fixture.Create<ClothesItemRequest>();

            _mockRepository.Setup(r => r.GetByIdAsync(id))
                        .ReturnsAsync((ClothesItem?)null);

            // Act
            var result = await _clothesService.UpdateAsync(id, request);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == "Nema takvog artikla bree");
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        // TEST: DeleteAsync - Successfully deletes an item
        [Fact]
        public async Task DeleteAsync_ReturnsSuccess_WhenItemIsDeleted()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fixture = new Fixture();
            var existingItem = fixture.Create<ClothesItem>();

            _mockRepository.Setup(r => r.GetByIdAsync(id))
                        .ReturnsAsync(existingItem);

            _mockRepository.Setup(r => r.DeleteAsync(id))
                        .ReturnsAsync(true);

            // Act
            var result = await _clothesService.DeleteAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        // TEST: DeleteAsync - Returns Failure when item doesn't exist
        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenItemDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetByIdAsync(id))
                        .ReturnsAsync((ClothesItem?)null);

            // Act
            var result = await _clothesService.DeleteAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == "Nema takvog artikla bree");
            _mockRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}

