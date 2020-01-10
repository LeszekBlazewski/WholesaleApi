using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models;

namespace Wholesale.BL.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        Task<Order> Create(Order model);
        Task<List<Order>> GetAll();
        Task<Order> GetById(int id);
        Task<Order> Update(Order model);
        Task Delete(int id);
        Task<IList<Order>> GetByUserId(int userId);
    }
}
