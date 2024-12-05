using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models.RequestModels;
using WebShopApp.Services;
using WebShopData.Models;

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
        public async Task<ActionResult<IEnumerable<ClothesItem>>> GetClothesItems()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClothesItem>> GetClothesItem(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClothesItem(Guid id, ClothesItemRequest itemRequest)
        {
            var updated = await _service.UpdateAsync(id, itemRequest);
            if (!updated) return NotFound();

            return NoContent();
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClothesItem>> PostClothesItem(ClothesItemRequest itemRequest)
        {
            // ID se automatski generiše u servisu
            var createdItem = await _service.AddAsync(itemRequest);
            
            // Vraća se rezultat sa generisanim ID-jem
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
