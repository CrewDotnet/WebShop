using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShopData.Data;
using WebShopData.Models;

namespace WebShopData.Repositories
{
        public class ClothesRepository : IClothesRepository
        {
            private readonly DataContext _context;

        public ClothesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClothesItem>> GetAllAsync() =>
            await _context.ClothesItems.ToListAsync();

        public async Task<ClothesItem?> GetByIdAsync(Guid id) =>
            await _context.ClothesItems.FindAsync(id);

        public async Task AddAsync(ClothesItem item)
        {
            await _context.ClothesItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(ClothesItem item)
        {
            _context.ClothesItems.Update(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var item = await _context.ClothesItems.FindAsync(id);
            if (item == null) return false;
            _context.ClothesItems.Remove(item);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}