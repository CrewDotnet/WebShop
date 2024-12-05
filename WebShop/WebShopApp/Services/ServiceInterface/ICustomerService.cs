using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;

namespace WebShopApp.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResponse>> GetAllAsync();
        Task<CustomerResponse?> GetByIdAsync(Guid id);
        Task<Customer> AddAsync(CustomerRequest customerRequest);
        Task<bool> UpdateAsync(Guid id, CustomerRequest customerRequest);
        Task<bool> DeleteAsync(Guid id);
    }
}