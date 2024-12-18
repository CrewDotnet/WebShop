using AutoMapper;
using Moq;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;
using WebShopData.Repositories;
using WebShopApp.Services;
using Xunit;
using FluentAssertions;

namespace WebShopTests.ServiceTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IClothesRepository> _mockClothesRepository;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockClothesRepository = new Mock<IClothesRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _orderService = new OrderService(_mockOrderRepository.Object, _mockMapper.Object, _mockClothesRepository.Object, _mockCustomerRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOrders_WhenOrdersExist()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), TotalPrice = 50 },
                new Order { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), TotalPrice = 100 }
            };

            var orderResponses = orders.Select(o => new OrderResponse { Id = o.Id, CustomerId = o.CustomerId, TotalPrice = o.TotalPrice }).ToList();

            _mockOrderRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);
            _mockMapper.Setup(m => m.Map<IEnumerable<OrderResponse>>(orders)).Returns(orderResponses);

            // Act
            var result = await _orderService.GetAllAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
            result.Value.Should().HaveCount(2);
            result.Value.Should().Contain(o => o.TotalPrice == 50);
            result.Value.Should().Contain(o => o.TotalPrice == 100);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFail_WhenNoOrdersExist()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Order>());

            // Act
            var result = await _orderService.GetAllAsync();

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "No orders found.");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOrder_WhenOrderExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var order = new Order { Id = id, CustomerId = Guid.NewGuid(), TotalPrice = 50 };
            var orderResponse = new OrderResponse { Id = id, CustomerId = order.CustomerId, TotalPrice = order.TotalPrice };

            _mockOrderRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(order);
            _mockMapper.Setup(m => m.Map<OrderResponse>(order)).Returns(orderResponse);

            // Act
            var result = await _orderService.GetByIdAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(orderResponse);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenOrderDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockOrderRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Order?)null);

            // Act
            var result = await _orderService.GetByIdAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Order not found.");
        }

        [Fact]
        public async Task AddAsync_ReturnsSuccess_WhenOrderIsCreated()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var clothesItem = new ClothesItem { Id = itemId, Price = 20 };
            var orderRequest = new OrderRequest { CustomerId = customerId, ClothesItemsId = new List<Guid> { itemId } };
            var customer = new Customer { Id = customerId, TotalSpent = 500, HasDiscount = false, OrdersCount = 2 };
            var order = new Order { Id = Guid.NewGuid(), CustomerId = customerId, TotalPrice = 20 };

            _mockClothesRepository.Setup(c => c.GetByIdAsync(itemId)).ReturnsAsync(clothesItem);
            _mockCustomerRepository.Setup(c => c.GetByIdAsync(customerId)).ReturnsAsync(customer);
            _mockOrderRepository.Setup(o => o.AddAsync(order)).Returns(Task.CompletedTask);

            _mockMapper.Setup(m => m.Map<Order>(orderRequest)).Returns(order);

            // Act
            var result = await _orderService.AddAsync(orderRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.TotalPrice.Should().Be(20);
            _mockCustomerRepository.Verify(c => c.UpdateAsync(customer), Times.Once);
            _mockOrderRepository.Verify(o => o.AddAsync(order), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ReturnsFail_WhenClothesItemNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var clothesItemId = Guid.NewGuid();
            var orderRequest = new OrderRequest { CustomerId = customerId, ClothesItemsId = new List<Guid> { clothesItemId } };

            _mockClothesRepository.Setup(c => c.GetByIdAsync(clothesItemId)).ReturnsAsync((ClothesItem?)null);

            // Act
            var result = await _orderService.AddAsync(orderRequest);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == $"Clothes item with ID {clothesItemId} not found.");
        }

        [Fact]
        public async Task AddAsync_ReturnsFail_WhenCustomerNotFound()
        {
            // Arrange
            var orderRequest = new OrderRequest { CustomerId = Guid.NewGuid() };
            var order = new Order { CustomerId = orderRequest.CustomerId };

            _mockClothesRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ClothesItem { Id = Guid.NewGuid(), Price = 20 });
            _mockCustomerRepository.Setup(c => c.GetByIdAsync(orderRequest.CustomerId)).ReturnsAsync((Customer?)null);

            // Act
            var result = await _orderService.AddAsync(orderRequest);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Customer not found.");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess_WhenOrderExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var orderRequest = new OrderRequest { CustomerId = Guid.NewGuid() };
            var order = new Order { Id = id, CustomerId = Guid.NewGuid(), TotalPrice = 100 };

            _mockOrderRepository.Setup(o => o.GetByIdAsync(id)).ReturnsAsync(order);
            _mockMapper.Setup(m => m.Map(orderRequest, order));

            // Act
            var result = await _orderService.UpdateAsync(id, orderRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockOrderRepository.Verify(o => o.UpdateAsync(order), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenOrderDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var orderRequest = new OrderRequest { CustomerId = Guid.NewGuid() };

            _mockOrderRepository.Setup(o => o.GetByIdAsync(id)).ReturnsAsync((Order?)null);

            // Act
            var result = await _orderService.UpdateAsync(id, orderRequest);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Item not found.");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess_WhenOrderExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var order = new Order { Id = id };

            _mockOrderRepository.Setup(o => o.GetByIdAsync(id)).ReturnsAsync(order);
            _mockOrderRepository.Setup(o => o.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _orderService.DeleteAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockOrderRepository.Verify(o => o.DeleteAsync(id), Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenOrderDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();

            _mockOrderRepository.Setup(o => o.GetByIdAsync(id)).ReturnsAsync((Order?)null);

            // Act
            var result = await _orderService.DeleteAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Item not found.");
        }
    }
}