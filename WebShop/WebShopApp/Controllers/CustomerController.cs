using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models;
using WebShopApp.Services;
using WebShopApp.Models.RequestModels;
using MediatR;
using WebShopApp.Queries;
using WebShopApp.Mediator.Commands;

namespace WebShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly IMediator _mediator;

        public CustomerController(ICustomerService service, IMediator mediator)
        {
            _service = service;
            _mediator = mediator;
        }

        // bez mediatora:
        // [HttpGet]
        // public async Task<IActionResult> GetCustomers()
        // {
        //     var result = await _service.GetAllAsync();
        //     return ValidatedResultPresenter.Present(result);
        // }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var result = await _mediator.Send(new GetCustomersQuery());
            return ValidatedResultPresenter.Present(result);
        }

        // bez mediatora:
        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetCustomer(Guid id)
        // {
        //     var result = await _service.GetByIdAsync(id);
        //     return ValidatedResultPresenter.Present(result);
        // }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery(id));
            return ValidatedResultPresenter.Present(result);
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutCustomer(Guid id, CustomerRequest customerRequest)
        // {
        //     var result = await _service.UpdateAsync(id, customerRequest);
        //     return ValidatedResultPresenter.Present(result);
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerRequest customerRequest)
        {
            var command = new UpdateCustomerCommand(id, customerRequest);
            var result = await _mediator.Send(command);

            return ValidatedResultPresenter.Present(result);
        }

        // [HttpPost]
        // public async Task<IActionResult> PostCustomer(CustomerRequest customerRequest)
        // {
        //     var result = await _service.AddAsync(customerRequest);
        //     return ValidatedResultPresenter.Present(result);
        // }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerRequest customerRequest)
        {
            var command = new AddCustomerCommand(customerRequest);
            var result = await _mediator.Send(command);

            return ValidatedResultPresenter.Present(result);
        }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteCustomer(Guid id)
        // {
        //     var result = await _service.DeleteAsync(id);
        //     return ValidatedResultPresenter.Present(result);
        // }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var command = new DeleteCustomerCommand(id);
            var result = await _mediator.Send(command);

            return ValidatedResultPresenter.Present(result);
        }
    }
}
