using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;

namespace WebShopApp.Services
{
    public interface IClothesService
    {
        Task<IEnumerable<ClothesItemsResponse>> GetAllAsync();
        Task<ClothesItemsResponse?> GetByIdAsync(Guid id);
        Task<ClothesItem> AddAsync(ClothesItemRequest request);
        Task<bool> UpdateAsync(Guid id, ClothesItemRequest item);
        Task<bool> DeleteAsync(Guid id);
    }
}