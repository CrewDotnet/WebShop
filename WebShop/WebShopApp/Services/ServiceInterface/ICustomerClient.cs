using Refit;
using WebShopApp.Models.RequestModels;

namespace WebShopApp.Services.ServiceInterface
{
    public interface ICustomerClient
    {
        [Get("/users")]
        Task<List<CustomerRequest>> GetCustomersFromApiAsync();
    }
}