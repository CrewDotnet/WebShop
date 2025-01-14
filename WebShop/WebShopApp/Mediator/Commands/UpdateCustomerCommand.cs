using FluentResults;
using MediatR;
using WebShopApp.Models.RequestModels;

namespace WebShopApp.Mediator.Commands
{
    public record UpdateCustomerCommand(Guid Id, CustomerRequest Customer) : IRequest<Result>;
}