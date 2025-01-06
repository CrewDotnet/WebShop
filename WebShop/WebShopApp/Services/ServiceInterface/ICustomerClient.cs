using WebShopApp.Models.RequestModels;

namespace WebShopApp.Services.ServiceInterface
{
    public interface ICustomerClient
    {
        Task<List<CustomerRequest>> GetCustomersFromApiAsync();
    }
}