using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopData.Models;

namespace WebShopData.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(Guid id);
    }
}