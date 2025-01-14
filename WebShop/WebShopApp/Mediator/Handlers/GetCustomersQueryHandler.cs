using FluentResults;
using MediatR;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Queries;
using WebShopApp.Services;

namespace WebShopApp.Handlers
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<IEnumerable<CustomerResponse>>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomersQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Result<IEnumerable<CustomerResponse>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _customerService.GetAllAsync();
        }
    }
}