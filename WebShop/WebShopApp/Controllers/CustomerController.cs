using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models;
using WebShopApp.Services;
using WebShopApp.Models.RequestModels;

namespace WebShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var result = await _service.GetAllAsync();
            return ValidatedResultPresenter.Present(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, CustomerRequest customerRequest)
        {
            var result = await _service.UpdateAsync(id, customerRequest);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer(CustomerRequest customerRequest)
        {
            var result = await _service.AddAsync(customerRequest);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return ValidatedResultPresenter.Present(result);
        }
    }
}
