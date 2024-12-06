using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models.RequestModels;
using WebShopApp.Services;

namespace WebShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClothesTypesController : ControllerBase
    {
        private readonly ITypesService _service;

        public ClothesTypesController(ITypesService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClothesTypesResponse>>> GetClothesTypes()
        {
            var types = await _service.GetAllAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClothesTypesResponse>> GetClothesType(Guid id)
        {
            var type = await _service.GetByIdAsync(id);
            if (type == null)
            {
                return NotFound();
            }
            return Ok(type);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClothesType(Guid id, ClothesTypeRequest typeRequest)
        {
            var updated = await _service.UpdateAsync(id, typeRequest);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ClothesTypeRequest>> PostClothesType(ClothesTypeRequest typeRequest)
        {
            var createdType = await _service.AddAsync(typeRequest);
            return CreatedAtAction(nameof(GetClothesType), new { id = createdType.Id }, createdType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClothesType(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}