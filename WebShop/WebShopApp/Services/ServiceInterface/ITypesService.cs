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
    public interface ITypesService
    {
        Task<Result<IEnumerable<ClothesTypesResponse>>> GetAllAsync();
        Task<Result<ClothesTypesResponse?>> GetByIdAsync(Guid id);
        Task<Result<ClothesType>> AddAsync(ClothesTypeRequest typeRequest);
        Task<Result> UpdateAsync(Guid id, ClothesTypeRequest typeRequest);
        Task<Result> DeleteAsync(Guid id);
    }
}