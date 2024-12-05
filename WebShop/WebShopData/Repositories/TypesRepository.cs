using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShopData.Data;
using WebShopData.Models;

namespace WebShopData.Repositories
{
    public class TypesRepository : ITypesRepository
    {
        private readonly DataContext _context;

        public TypesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClothesType>> GetAllAsync() =>
            await _context.ClothesTypes.ToListAsync();

        public async Task<ClothesType?> GetByIdAsync(Guid id) =>
            await _context.ClothesTypes.FindAsync(id);

        public async Task AddAsync(ClothesType type)
        {
            await _context.ClothesTypes.AddAsync(type);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(ClothesType type)
        {
            _context.ClothesTypes.Update(type);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var type = await _context.ClothesTypes.FindAsync(id);
            if (type == null) return false;
            _context.ClothesTypes.Remove(type);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}