using AutoMapper;
using FluentResults;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Services.ServiceInterface;
using WebShopData.Models;
using WebShopData.Repositories;

namespace WebShopApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICustomerClient _customerClient;

        public CustomerService(ICustomerRepository repository, IMapper mapper, ICustomerClient customerClient)
        {
            _repository = repository;
            _mapper = mapper;
            _customerClient = customerClient;
        }
        public async Task FetchAndSaveCustomersAsync()
        {
            var customersFromApi = await _customerClient.GetCustomersFromApiAsync();

            foreach (var apiCustomer in customersFromApi)
            {
                if (!await _repository.CustomerExistsByNameAsync(apiCustomer.Name))
                {
                    var customer = _mapper.Map<Customer>(apiCustomer);
                    await _repository.AddAsync(customer);
                }
            }
        }
        public async Task<Result<IEnumerable<CustomerResponse>>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            if (!items.Any())
            {
                return Result.Fail<IEnumerable<CustomerResponse>>("No orders found.");
            }
            return Result.Ok(_mapper.Map<IEnumerable<CustomerResponse>>(items));
        }

        public async Task<Result<CustomerResponse?>> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return Result.Fail("Item not found.");
            }
            var response = _mapper.Map<CustomerResponse?>(item);
            return Result.Ok(response);
        }

        public async Task<Result<Customer>> AddAsync(CustomerRequest customerRequest)
        {
            var customer = _mapper.Map<Customer>(customerRequest);
            await _repository.AddAsync(customer);
            return Result.Ok(customer);
        }

        public async Task<Result> UpdateAsync(Guid id, CustomerRequest customerRequest)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return Result.Fail("Item not found.");
            }

            _mapper.Map(customerRequest, item);
            await _repository.UpdateAsync(item);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var exists = await _repository.GetByIdAsync(id);
            if (exists == null)
            {
                return Result.Fail("Item not found.");
            }

            await _repository.DeleteAsync(id);
            return Result.Ok();
        }
    }
}