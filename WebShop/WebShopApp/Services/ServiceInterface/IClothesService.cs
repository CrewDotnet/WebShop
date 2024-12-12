using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using WebShopApp.Models.RequestModels;
using WebShopApp.Models.ResponseModels;
using WebShopData.Models;

namespace WebShopApp.Services
{
    public interface IClothesService
    {
        Task<Result<IEnumerable<ClothesItemsResponse>>> GetAllAsync();
        Task<Result<ClothesItemsResponse?>> GetByIdAsync(Guid id);
        Task<Result<ClothesItem>> AddAsync(ClothesItemRequest request);
        Task<Result> UpdateAsync(Guid id, ClothesItemRequest item);
        Task<Result> DeleteAsync(Guid id);
    }
}