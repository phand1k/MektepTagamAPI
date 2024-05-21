﻿using MektepTagamAPI.Data;
using MektepTagamAPI.Models;
using MektepTagamAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MektepTagamAPI.Repositories.Implementations
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;
        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Car> GetByIdAsync(int id)
        {
            return await _context.Cars.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _context.Cars.Where(p => p.IsDeleted == false).ToListAsync();
        }

        public async Task AddAsync(Car product)
        {
            _context.Cars.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car product)
        {
            _context.Cars.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                await UpdateAsync(product);
            }
        }
        public async Task<bool> ExistsWithName(string name)
        {
            return await _context.Cars.AnyAsync(c => c.Name == name && c.IsDeleted == false);
        }
    }
}
