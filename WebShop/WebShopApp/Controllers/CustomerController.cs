using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models.RequestModels;
using WebShopApp.Services;

namespace WebShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController (ICustomerService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetCustomers()
        {
            var customers = await _service.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomer(Guid id)
        {
            var customer = await _service.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, CustomerRequest customerRequest)
        {
            var updated = await _service.UpdateAsync(id, customerRequest);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CustomerRequest>> PostCustomer(CustomerRequest customerRequest)
        {
            var createdCustomer = await _service.AddAsync(customerRequest);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}