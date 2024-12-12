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
            var result = await _service.GetAllAsync();
            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomer(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.IsFailed)
            {
                return NotFound(result.Errors.Select(e => e.Message));
            }
            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, CustomerRequest customerRequest)
        {
            var result = await _service.UpdateAsync(id, customerRequest);
            if (result.IsFailed)
            {
                return NotFound(result.Errors.Select(e => e.Message));
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CustomerRequest>> PostCustomer(CustomerRequest customerRequest)
        {
            // Id se automatski generise u servisu
            var result = await _service.AddAsync(customerRequest);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }
            // Vraca se rezultat sa generisanim id
            return CreatedAtAction(nameof(GetCustomer), new { id = result.Value.Id }, result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.IsFailed)
            {
                return NotFound(result.Errors.Select(e => e.Message));
            }
            return NoContent();
        }
    }
}