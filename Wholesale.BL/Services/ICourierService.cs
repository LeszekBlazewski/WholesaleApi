using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models.Views;

namespace Wholesale.BL.Services
{
    public interface ICourierService
    {
        Task<IList<CourierStatsV>> GetStatsForAll();
        Task<CourierStatsV> GetStatsByCourierId(int id);
    }
}
