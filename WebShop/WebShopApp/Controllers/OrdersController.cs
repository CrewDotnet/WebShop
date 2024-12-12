using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models.RequestModels;
using WebShopApp.Services;
using WebShopApp.Models;

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
        public async Task<IActionResult> GetOrders()
        {
            var result = await _service.GetAllAsync();
            return ValidatedResultPresenter.Present(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder(OrderRequest orderRequest)
        {
            var result = await _service.AddAsync(orderRequest);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, OrderRequest orderRequest)
        {
            var result = await _service.UpdateAsync(id, orderRequest);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return ValidatedResultPresenter.Present(result);
        }
    }
}