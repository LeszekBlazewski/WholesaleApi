using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models.Views;

namespace Wholesale.BL.RepositoryInterfaces
{
    public interface ICourierRepository
    {
        Task<IList<CourierStatsV>> GetStatsForAll();
        Task<CourierStatsV> GetStatsByCourierId(int id);
    }
}
