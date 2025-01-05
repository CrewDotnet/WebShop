using FluentResults;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;

namespace WebShopApp.Services
{
    public interface IOrderService
    {
        Task<Result<IEnumerable<OrderResponse>>> GetAllAsync();
        Task<Result<OrderResponse?>> GetByIdAsync(Guid id);
        Task<Result<Order>> AddAsync(OrderRequest orderRequest);
        Task<Result> UpdateAsync(Guid id, OrderRequest orderRequest);
        Task<Result> DeleteAsync(Guid id);
    }
}