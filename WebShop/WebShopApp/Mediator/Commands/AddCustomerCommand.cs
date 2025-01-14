using FluentResults;
using MediatR;
using WebShopApp.Models.RequestModels;
using WebShopData.Models;

namespace WebShopApp.Mediator.Commands
{
    public record AddCustomerCommand(CustomerRequest Customer) : IRequest<Result<Customer>>;
}
