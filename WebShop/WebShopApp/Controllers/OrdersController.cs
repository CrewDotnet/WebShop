using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models.RequestModels;
using WebShopApp.Services;

namespace WebShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController (IOrderService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders()
        {
            var result = await _service.GetAllAsync();

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(Guid id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, OrderRequest orderRequest)
        {
            var result = await _service.UpdateAsync(id, orderRequest);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder(OrderRequest orderRequest)
        {
            var result = await _service.AddAsync(orderRequest);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }

            return CreatedAtAction(nameof(GetOrder), new { id = result.Value.Id }, result.Value);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var result = await _service.DeleteAsync(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }

            return NoContent();
        }
    }
}