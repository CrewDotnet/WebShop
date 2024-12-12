using AutoMapper;
using FluentResults;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;
using WebShopData.Repositories;

namespace WebShopApp.Services
{
    public class ClothesService : IClothesService
    {
        private readonly IClothesRepository _repository;
        private readonly IMapper _mapper;

        public ClothesService(IClothesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<ClothesItemsResponse>>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            if (!items.Any())
            {
                return Result.Fail<IEnumerable<ClothesItemsResponse>>("No orders found.");
            }
            return Result.Ok(_mapper.Map<IEnumerable<ClothesItemsResponse>>(items));
        }

        public async Task<Result<ClothesItemsResponse?>> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return Result.Fail("Nema takvog artikla bree");
            }
            var response = _mapper.Map<ClothesItemsResponse?>(item);
            return Result.Ok(response);
        }

        public async Task<Result<ClothesItem>> AddAsync(ClothesItemRequest request)
        {
            var item = _mapper.Map<ClothesItem>(request);
            item.Id = Guid.NewGuid(); // Generisanje ID-a
            await _repository.AddAsync(item);
            return Result.Ok(item); // Vraćamo kreirani entitet
        }

        public async Task<Result> UpdateAsync(Guid id, ClothesItemRequest request)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return Result.Fail("Nema takvog artikla bree");
            }

            _mapper.Map(request, item);
            await _repository.UpdateAsync(item);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var exists = await _repository.GetByIdAsync(id);
            if (exists == null)
            {
                return Result.Fail("Nema takvog artikla bree");
            }

            await _repository.DeleteAsync(id);
            return Result.Ok();
        }
    }
}