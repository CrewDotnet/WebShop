using FluentResults;
using MediatR;
using WebShopApp.Mediator.Commands;
using WebShopApp.Services;

namespace WebShopApp.Handlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result>
    {
        private readonly ICustomerService _customerService;

        public DeleteCustomerCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.DeleteAsync(request.Id);
        }
    }
}
