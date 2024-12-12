using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models.RequestModels;
using WebShopApp.Services;
using WebShopApp.Models;

namespace WebShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClothesItemsController : ControllerBase
    {
        private readonly IClothesService _service;

        public ClothesItemsController(IClothesService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetClothesItems()
        {
            var result = await _service.GetAllAsync();
            return ValidatedResultPresenter.Present(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClothesItem(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClothesItem(Guid id, ClothesItemRequest itemRequest)
        {
            var result = await _service.UpdateAsync(id, itemRequest);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostClothesItem(ClothesItemRequest itemRequest)
        {
            var result = await _service.AddAsync(itemRequest);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClothesItem(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return ValidatedResultPresenter.Present(result);
        }
    }
}

