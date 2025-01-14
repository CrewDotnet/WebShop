using FluentResults;
using MediatR;
using WebShopApp.Mediator.Commands;
using WebShopApp.Services;

namespace WebShopApp.Handlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result>
    {
        private readonly ICustomerService _customerService;

        public UpdateCustomerCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.UpdateAsync(request.Id, request.Customer);
        }
    }
}
