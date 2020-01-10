using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wholesale.BL.Models;
using Wholesale.BL.RepositoryInterfaces;

namespace Wholesale.DAL.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly DeliveriesContext _context;

        public ProductCategoryRepository(DeliveriesContext context)
        {
            _context = context;
        }

        public async Task<ProductCategory> Create(ProductCategory model)
        {
            _context.ProductCategories.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<List<ProductCategory>> GetAll()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetById(int id)
        {
            return await _context.ProductCategories.FindAsync(id);
        }

        public async Task<ProductCategory> Update(ProductCategory model)
        {
            if (await _context.ProductCategories.AllAsync(x => x.CategoryId == model.CategoryId))
                throw new InvalidOperationException("Category does not exist");
            _context.ProductCategories.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task Delete(int id)
        {
            var categoryToDelete = await _context.ProductCategories.FindAsync(id);
            if (categoryToDelete == null)
                throw new InvalidOperationException("Category does not exist");
            _context.ProductCategories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
