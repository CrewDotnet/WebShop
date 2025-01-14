using FluentResults;
using MediatR;
using WebShopApp.Models.ResponseModels;

namespace WebShopApp.Queries
{
    public class GetCustomersQuery : IRequest<Result<IEnumerable<CustomerResponse>>>
    {
    }
}