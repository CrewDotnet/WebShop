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
            var result = await _service.GetAllAsync();
            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClothesItemsResponse>> GetClothesItem(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.IsFailed)
            {
                return NotFound(result.Errors.Select(e => e.Message));
            }
            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClothesItem(Guid id, ClothesItemRequest itemRequest)
        {
            var result = await _service.UpdateAsync(id, itemRequest);
            if (result.IsFailed)
            {
                return NotFound(result.Errors.Select(e => e.Message));
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ClothesItemRequest>> PostClothesItem(ClothesItemRequest itemRequest)
        {
            // Id se automatski generise u servisu
            var result = await _service.AddAsync(itemRequest);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }
            // Vraca se rezultat sa generisanim id
            return CreatedAtAction(nameof(GetClothesItem), new { id = result.Value.Id }, result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClothesItem(Guid id)
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
