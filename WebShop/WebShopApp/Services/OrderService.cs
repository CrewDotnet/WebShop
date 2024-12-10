using AutoMapper;
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

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var items = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(items);
        }

        public async Task<OrderResponse?> GetByIdAsync(Guid id)
        {
            var item = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderResponse?>(item);
        }

        public async Task<Order> AddAsync(OrderRequest orderRequest)
        {
            var order = _mapper.Map<Order>(orderRequest);
            order.Id = Guid.NewGuid(); // Automatically generate ID

            foreach (var clothesItemId in orderRequest.ClothesItemsId)
            {
                var clothesItem = await _itemRepository.GetByIdAsync(clothesItemId);
                if (clothesItem != null)
                {
                    order.ClothesItems.Add(clothesItem);
                }
            }

            // Ručno izračunaj i postavi TotalPrice
            order.TotalPrice = order.ClothesItems.Sum(item => item.Price);

            var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
            if (customer != null)
            {
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
            }

            await _orderRepository.AddAsync(order);
            return order;
        }

        public async Task<bool> UpdateAsync(Guid id, OrderRequest orderRequest)
        {
            var order = _mapper.Map<Order>(orderRequest);
            order.Id = id; // Assign the existing ID for update
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _orderRepository.DeleteAsync(id);
        }
    }
}