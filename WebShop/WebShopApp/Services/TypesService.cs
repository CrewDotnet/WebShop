using AutoMapper;
using FluentResults;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;
using WebShopData.Repositories;

namespace WebShopApp.Services
{
    public class TypesService : ITypesService
    {
        private readonly ITypesRepository _repository;
        private readonly IMapper _mapper;

        public TypesService (ITypesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ClothesTypesResponse>>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            if (!items.Any())
            {
                return Result.Fail<IEnumerable<ClothesTypesResponse>>("No orders found.");
            }
            return Result.Ok(_mapper.Map<IEnumerable<ClothesTypesResponse>>(items));
        }

        public async Task<Result<ClothesTypesResponse?>> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return Result.Fail("Item not found.");
            }
            var response = _mapper.Map<ClothesTypesResponse?>(item);
            return Result.Ok(response);
        }

        public async Task<Result<ClothesType>> AddAsync(ClothesTypeRequest typeRequest)
        {
            var type = _mapper.Map<ClothesType>(typeRequest);
            type.Id = Guid.NewGuid(); 
            await _repository.AddAsync(type);
            return Result.Ok(type);
        }

        public async Task<Result> UpdateAsync(Guid id, ClothesTypeRequest typeRequest)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                return Result.Fail("Item not found.");
            }

            _mapper.Map(typeRequest, item);
            await _repository.UpdateAsync(item);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var exists = await _repository.GetByIdAsync(id);
            if (exists == null)
            {
                return Result.Fail("Item not found.");
            }

            await _repository.DeleteAsync(id);
            return Result.Ok();
        }
    }
}