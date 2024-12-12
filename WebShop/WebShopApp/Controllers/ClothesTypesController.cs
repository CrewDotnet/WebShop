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
            var result = await _service.GetAllAsync();
            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClothesTypesResponse>> GetClothesType(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.IsFailed)
            {
                return NotFound(result.Errors.Select(e => e.Message));
            }
            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClothesType(Guid id, ClothesTypeRequest typeRequest)
        {
            var result = await _service.UpdateAsync(id, typeRequest);
            if (result.IsFailed)
            {
                return NotFound(result.Errors.Select(e => e.Message));
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ClothesTypeRequest>> PostClothesType(ClothesTypeRequest typeRequest)
        {
            var result = await _service.AddAsync(typeRequest);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors.Select(e => e.Message));
            }
            // Vraca se rezultat sa generisanim id
            return CreatedAtAction(nameof(GetClothesType), new { id = result.Value.Id }, result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClothesType(Guid id)
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