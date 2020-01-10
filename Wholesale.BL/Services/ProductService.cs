using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models;
using Wholesale.BL.Models.Views;
using Wholesale.BL.RepositoryInterfaces;

namespace Wholesale.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> Create(Product model)
        {
            return await _repository.Create(model);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Product> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Product> Update(Product model)
        {
            return await _repository.Update(model);
        }

        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<ProductStatsV>> GetStats()
        {
            return await _repository.GetStats();
        }
    }
}
