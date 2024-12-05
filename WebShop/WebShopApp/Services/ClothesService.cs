using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<ClothesItemsResponse>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClothesItemsResponse>>(items);
        }

        public async Task<ClothesItemsResponse?> GetByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<ClothesItemsResponse?>(item);
        }

        public async Task<ClothesItem> AddAsync(ClothesItemRequest request)
        {
            var item = _mapper.Map<ClothesItem>(request);
            item.Id = Guid.NewGuid(); // Generisanje ID-a
            await _repository.AddAsync(item);
            return item; // Vraćamo kreirani entitet
        }

        public async Task<bool> UpdateAsync(Guid id, ClothesItemRequest request)
        {
            var item = _mapper.Map<ClothesItem>(request);
            item.Id = id; // Postavi ID jer ga DTO nema
            return await _repository.UpdateAsync(item);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}