using AutoMapper;
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

        public async Task<IEnumerable<ClothesTypesResponse>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClothesTypesResponse>>(items);
        }

        public async Task<ClothesTypesResponse?> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<ClothesTypesResponse?>(item);
        }

        public async Task<ClothesType> AddAsync(ClothesTypeRequest typeRequest)
        {
            var type = _mapper.Map<ClothesType>(typeRequest);
            type.Id = Guid.NewGuid(); 
            await _repository.AddAsync(type);
            return type;
        }

        public async Task<bool> UpdateAsync(Guid id, ClothesTypeRequest typeRequest)
        {
            var type = _mapper.Map<ClothesType>(typeRequest);
            type.Id = id;
            return await _repository.UpdateAsync(type);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}