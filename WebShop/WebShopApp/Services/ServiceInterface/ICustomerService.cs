using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;

namespace WebShopApp.Services
{
    public interface ICustomerService
    {
        Task<Result<IEnumerable<CustomerResponse>>> GetAllAsync();
        Task<Result<CustomerResponse?>> GetByIdAsync(Guid id);
        Task<Result<Customer>> AddAsync(CustomerRequest customerRequest);
        Task<Result> UpdateAsync(Guid id, CustomerRequest customerRequest);
        Task<Result> DeleteAsync(Guid id);
    }
}