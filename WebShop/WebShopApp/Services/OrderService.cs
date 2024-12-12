using AutoMapper;
using FluentResults;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;
using WebShopData.Repositories;

namespace WebShopApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClothesRepository _itemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IMapper mapper, IClothesRepository itemRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = repository;
            _mapper = mapper;
            _itemRepository = itemRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Result<IEnumerable<OrderResponse>>> GetAllAsync()
        {
            var items = await _orderRepository.GetAllAsync();
            if (!items.Any())
            {
                return Result.Fail<IEnumerable<OrderResponse>>("No orders found.");
            }
            return Result.Ok(_mapper.Map<IEnumerable<OrderResponse>>(items));
        }

        public async Task<Result<OrderResponse?>> GetByIdAsync(Guid id)
        {
            var item = await _orderRepository.GetByIdAsync(id);
            if (item == null)
            {
                return Result.Fail<OrderResponse?>("Order not found.");
            }
            return Result.Ok(_mapper.Map<OrderResponse?>(item));
        }

        public async Task<Result<Order>> AddAsync(OrderRequest orderRequest)
        {
            var order = _mapper.Map<Order>(orderRequest);
            order.Id = Guid.NewGuid(); // Automatically generate ID

            foreach (var clothesItemId in orderRequest.ClothesItemsId)
            {
                var clothesItem = await _itemRepository.GetByIdAsync(clothesItemId);
                if (clothesItem == null)
                {
                    return Result.Fail<Order>($"Clothes item with ID {clothesItemId} not found.");
                }
                    order.ClothesItems.Add(clothesItem);
            }

            // Ručno izračunaj i postavi TotalPrice
            order.TotalPrice = order.ClothesItems.Sum(item => item.Price);

            var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
            if (customer == null)
            {
                return Result.Fail<Order>("Customer not found.");
            }

            if (customer.HasDiscount)
            {
                order.TotalPrice -= 1000;
                customer.HasDiscount = false;
            }

            customer.OrdersCount++;

            if (customer.OrdersCount == 3)
            {
                customer.HasDiscount = true;
                customer.OrdersCount = 0;
            }

            customer.TotalSpent += order.TotalPrice;
            await _customerRepository.UpdateAsync(customer);

            await _orderRepository.AddAsync(order);
            return Result.Ok(order);
        }

        public async Task<Result> UpdateAsync(Guid id, OrderRequest orderRequest)
        {
            var item = await _orderRepository.GetByIdAsync(id);
            if (item == null)
            {
                return Result.Fail("Item not found.");
            }

            _mapper.Map(orderRequest, item);
            await _orderRepository.UpdateAsync(item);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var exists = await _orderRepository.GetByIdAsync(id);
            if (exists == null)
            {
                return Result.Fail("Item not found.");
            }

            await _orderRepository.DeleteAsync(id);
            return Result.Ok();
        }
    }
}