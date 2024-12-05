using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;

namespace WebShopApp.Services
{
    public interface ITypesService
    {
        Task<IEnumerable<ClothesTypesResponse>> GetAllAsync();
        Task<ClothesTypesResponse?> GetByIdAsync(Guid id);
        Task<ClothesType> AddAsync(ClothesTypeRequest typeRequest);
        Task<bool> UpdateAsync(Guid id, ClothesTypeRequest typeRequest);
        Task<bool> DeleteAsync(Guid id);
    }
}