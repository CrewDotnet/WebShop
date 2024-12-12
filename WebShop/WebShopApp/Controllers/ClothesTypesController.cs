using Microsoft.AspNetCore.Mvc;
using WebShopApp.Models.ResponseModels;
using WebShopApp.Models.RequestModels;
using WebShopApp.Services;
using WebShopApp.Models;

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
        public async Task<IActionResult> GetClothesTypes()
        {
            var result = await _service.GetAllAsync();
            return ValidatedResultPresenter.Present(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClothesType(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClothesType(Guid id, ClothesTypeRequest typeRequest)
        {
            var result = await _service.UpdateAsync(id, typeRequest);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostClothesType(ClothesTypeRequest typeRequest)
        {
            var result = await _service.AddAsync(typeRequest);
            return ValidatedResultPresenter.Present(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClothesType(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return ValidatedResultPresenter.Present(result);
        }
    }
}
