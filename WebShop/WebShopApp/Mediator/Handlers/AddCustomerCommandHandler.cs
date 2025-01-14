using FluentResults;
using MediatR;
using WebShopApp.Mediator.Commands;
using WebShopApp.Services;
using WebShopData.Models;

namespace WebShopApp.Handlers
{
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, Result<Customer>>
    {
        private readonly ICustomerService _customerService;

        public AddCustomerCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Result<Customer>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            return await _customerService.AddAsync(request.Customer);
        }
    }
}
