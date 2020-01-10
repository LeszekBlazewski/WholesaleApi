using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models;

namespace Wholesale.BL.RepositoryInterfaces
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory> Create(ProductCategory model);
        Task<List<ProductCategory>> GetAll();
        Task<ProductCategory> GetById(int id);
        Task<ProductCategory> Update(ProductCategory model);
        Task Delete(int id);
    }
}
