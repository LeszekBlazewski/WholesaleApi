using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models;
using Wholesale.BL.Models.Views;

namespace Wholesale.BL.RepositoryInterfaces
{
    public interface IProductRepository
    {
        Task<Product> Create(Product model);
        Task<List<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<Product> Update(Product model);
        Task Delete(int id);
        Task<IList<ProductStatsV>> GetStats();
    }
}
