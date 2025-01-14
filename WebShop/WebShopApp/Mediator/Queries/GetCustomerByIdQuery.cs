using FluentResults;
using MediatR;
using WebShopApp.Models.ResponseModels;

namespace WebShopApp.Queries
{
    public record GetCustomerByIdQuery(Guid Id) : IRequest<Result<CustomerResponse?>>;

}