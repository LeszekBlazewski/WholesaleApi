using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models;
using Wholesale.BL.RepositoryInterfaces;

namespace Wholesale.BL.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _repository;

        public ProductCategoryService(IProductCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductCategory> Create(ProductCategory model)
        {
            return await _repository.Create(model);
        }

        public async Task<List<ProductCategory>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<ProductCategory> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<ProductCategory> Update(ProductCategory model)
        {
            return await _repository.Update(model);
        }

        public async Task Delete(int id)
        {
            await _repository.Delete(id);
        }
    }
}
