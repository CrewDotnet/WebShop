using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopData.Models;

namespace WebShopData.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task AddAsync(Customer customer);
        Task<bool> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> CustomerExistsByNameAsync(string name);
    }
}