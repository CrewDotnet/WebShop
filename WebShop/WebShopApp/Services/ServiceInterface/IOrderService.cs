using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;

namespace WebShopApp.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<OrderResponse?> GetByIdAsync(Guid id);
        Task<Order> AddAsync(OrderRequest orderRequest);
        Task<bool> UpdateAsync(Guid id, OrderRequest orderRequest);
        Task<bool> DeleteAsync(Guid id);
    }
}