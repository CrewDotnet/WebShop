using AutoMapper;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;
using WebShopData.Repositories;

namespace WebShopApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService (ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerResponse>>(items);
        }

        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<CustomerResponse?>(item);
        }

        public async Task<Customer> AddAsync(CustomerRequest customerRequest)
        {
            var customer = _mapper.Map<Customer>(customerRequest);
            customer.Id = Guid.NewGuid();
            await _repository.AddAsync(customer);
            return customer;
        }

        public async Task<bool> UpdateAsync(Guid id, CustomerRequest customerRequest)
        {
            var customer = _mapper.Map<Customer>(customerRequest);
            customer.Id = id; 
            return await _repository.UpdateAsync(customer);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}