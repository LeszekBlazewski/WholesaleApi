using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wholesale.BL.Models.Views;
using Wholesale.BL.RepositoryInterfaces;

namespace Wholesale.DAL.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly DeliveriesContext _context;

        public CourierRepository(DeliveriesContext context)
        {
            _context = context;
        }

        public async Task<IList<CourierStatsV>> GetStatsForAll()
        {
            return await _context.CourierStats.ToListAsync();
        }

        public async Task<CourierStatsV> GetStatsByCourierId(int id)
        {
            return await _context.CourierStats.SingleOrDefaultAsync(x => x.CourierId == id);
        }
    }
}
