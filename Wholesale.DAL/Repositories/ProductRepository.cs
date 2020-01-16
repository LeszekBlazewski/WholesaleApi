using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wholesale.BL.Models;
using Wholesale.BL.Models.Views;
using Wholesale.BL.RepositoryInterfaces;

namespace Wholesale.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DeliveriesContext _context;

        public ProductRepository(DeliveriesContext context)
        {
            _context = context;
        }

        public async Task<Product> Create(Product model)
        {
            _context.ProductCategories.Attach(model.Category);
            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(y => y.ProductId == id);
        }

        public async Task<Product> Update(Product model)
        {
            if (await _context.Products.AllAsync(x => x.CategoryId == model.CategoryId))
                throw new InvalidOperationException("Product does not exist");
            _context.Products.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task Delete(int id)
        {
            var productToDelete = await _context.Products.FindAsync(id);
            if (productToDelete == null)
                throw new InvalidOperationException("Product does not exist");
            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<ProductStatsV>> GetStats()
        {
            return await _context.ProductStats.OrderByDescending(x => x.NumberSold).ToListAsync();
        }
    }
}
