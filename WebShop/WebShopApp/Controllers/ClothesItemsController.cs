using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models.RequestModels;
using WebShopApp.Services;

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
        public async Task<ActionResult<IEnumerable<ClothesItemsResponse>>> GetClothesItems()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClothesItemsResponse>> GetClothesItem(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClothesItem(Guid id, ClothesItemRequest itemRequest)
        {
            var updated = await _service.UpdateAsync(id, itemRequest);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ClothesItemRequest>> PostClothesItem(ClothesItemRequest itemRequest)
        {
            // Id se automatski generise u servisu
            var createdItem = await _service.AddAsync(itemRequest);
            
            // Vraca se rezultat sa generisanim id
            return CreatedAtAction(nameof(GetClothesItem), new { id = createdItem.Id }, createdItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClothesItem(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
