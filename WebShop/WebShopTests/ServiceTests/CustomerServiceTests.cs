using AutoMapper;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;
using WebShopData.Repositories;
using Moq;
using FluentAssertions;
using WebShopApp.Services;
using WebShopApp.Services.ServiceInterface;

namespace WebShopTests.ServiceTests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICustomerClient> _mockCustomerClient;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockCustomerClient = new Mock<ICustomerClient>();

            _customerService = new CustomerService(_mockRepository.Object, _mockMapper.Object, _mockCustomerClient.Object);
        }

        [Fact]
        public async Task FetchAndSaveCustomersAsync_SavesCustomer_WhenHeIsNotInTable()
        {
            // Arrange: Mock API podataka
            var apiCustomers = new List<CustomerRequest>
            {
                new() { Name = "John Doe" },
                new() { Name = "Jane Smith" }
            };

            _mockCustomerClient
                .Setup(client => client.GetCustomersFromApiAsync())
                .ReturnsAsync(apiCustomers);

            _mockRepository
                .Setup(repo => repo.CustomerExistsByNameAsync("John Doe"))
                .ReturnsAsync(true);
            _mockRepository
                .Setup(repo => repo.CustomerExistsByNameAsync("Jane Smith"))
                .ReturnsAsync(false);

            // Mock - Mapiranje klijent requesta na model
            _mockMapper
                .Setup(m => m.Map<Customer>(It.Is<CustomerRequest>(cr => cr.Name == "Jane Smith")))
                .Returns(new Customer { Name = "Jane Smith" });

            // Mock - Poziv metode AddAsync
            _mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);

            // Act: Poziv servisa
            await _customerService.FetchAndSaveCustomersAsync();

            // Assert: Provera da li je `AddAsync` pozvan tačno jednom za Jane Smith
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCustomers_WhenCustomersExist()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), Name = "John Doe", TotalSpent = 200 },
                new Customer { Id = Guid.NewGuid(), Name = "Jane Smith", TotalSpent = 500 }
            };

            var customerResponses = customers.Select(c => new CustomerResponse
            {
                Id = c.Id,
                Name = c.Name,
                TotalSpent = c.TotalSpent
            }).ToList();

            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);
            _mockMapper.Setup(m => m.Map<IEnumerable<CustomerResponse>>(customers)).Returns(customerResponses);

            // Act
            var result = await _customerService.GetAllAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNullOrEmpty();
            result.Value.Should().HaveCount(2);
            result.Value.Should().ContainSingle(x => x.Name == "John Doe");
            result.Value.Should().ContainSingle(x => x.Name == "Jane Smith");
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFail_WhenNoCustomersExist()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Customer>());

            // Act
            var result = await _customerService.GetAllAsync();

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "No orders found.");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCustomer_WhenCustomerExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customer = new Customer { Id = id, Name = "John Doe", TotalSpent = 200 };
            var response = new CustomerResponse { Id = id, Name = "John Doe", TotalSpent = 200 };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(customer);
            _mockMapper.Setup(m => m.Map<CustomerResponse>(customer)).Returns(response);

            // Act
            var result = await _customerService.GetByIdAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenCustomerDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.GetByIdAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Item not found.");
        }

        [Fact]
        public async Task AddAsync_ReturnsSuccess_WhenCustomerIsAdded()
        {
            // Arrange
            var customerRequest = new CustomerRequest { Name = "John Doe" };
            var customer = new Customer { Id = Guid.NewGuid(), Name = "John Doe", TotalSpent = 0 };

            _mockMapper.Setup(m => m.Map<Customer>(customerRequest)).Returns(customer);
            _mockRepository.Setup(r => r.AddAsync(customer)).Returns(Task.CompletedTask);

            // Act
            var result = await _customerService.AddAsync(customerRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be("John Doe");
            _mockRepository.Verify(r => r.AddAsync(customer), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess_WhenCustomerExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existingCustomer = new Customer { Id = id, Name = "John Doe", TotalSpent = 200 };
            var updateRequest = new CustomerRequest { Name = "Updated John" };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingCustomer);
            _mockMapper.Setup(m => m.Map(updateRequest, existingCustomer));

            // Act
            var result = await _customerService.UpdateAsync(id, updateRequest);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(r => r.UpdateAsync(existingCustomer), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenCustomerDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateRequest = new CustomerRequest { Name = "Updated John" };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.UpdateAsync(id, updateRequest);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Item not found.");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess_WhenCustomerExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customerToDelete = new Customer { Id = id, Name = "John Doe", TotalSpent = 200 };

            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(customerToDelete);
            _mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _customerService.DeleteAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFail_WhenCustomerDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.DeleteAsync(id);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Item not found.");
        }
    }
}
