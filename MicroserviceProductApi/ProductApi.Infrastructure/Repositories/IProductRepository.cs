using ProductApi.Domain.Entities;
using ProductApi.Application.Interface;
using ProductApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductInterface
    {
        private readonly ProductDbContext _dbContext;

        public ProductRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            await _dbContext.Products.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _dbContext.Products.ToListAsync();

        public async Task<Product?> FindByIdAsync(int id) =>
            await _dbContext.Products.FindAsync(id);

        public async Task<Product?> GetByAsync(Expression<Func<Product, bool>> expression) =>
            await _dbContext.Products.FirstOrDefaultAsync(expression);

        public async Task<Product> UpdateAsync(Product entity)
        {
            var existing = await _dbContext.Products.FindAsync(entity.Id);
            if (existing == null) return null!;

            existing.Name = entity.Name;
            existing.Price = entity.Price;
            existing.Quantity = entity.Quantity;

            _dbContext.Products.Update(existing);
            await _dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Product entity)
        {
            var existing = await _dbContext.Products.FindAsync(entity.Id);
            if (existing == null) return false;

            _dbContext.Products.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
