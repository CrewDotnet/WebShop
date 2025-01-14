using FluentResults;
using MediatR;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Queries;
using WebShopApp.Services;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerResponse?>>
{
    private readonly ICustomerService _customerService;

    public GetCustomerByIdQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<Result<CustomerResponse?>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _customerService.GetByIdAsync(request.Id);
    }
}
