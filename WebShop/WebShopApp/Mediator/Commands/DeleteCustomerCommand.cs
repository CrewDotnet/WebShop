using FluentResults;
using MediatR;

namespace WebShopApp.Mediator.Commands
{
    public record DeleteCustomerCommand(Guid Id) : IRequest<Result>;
}