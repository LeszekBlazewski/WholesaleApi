using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models.Views;
using Wholesale.BL.RepositoryInterfaces;

namespace Wholesale.BL.Services
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _repository;

        public CourierService(ICourierRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<CourierStatsV>> GetStatsForAll()
        {
            return await _repository.GetStatsForAll();
        }

        public async Task<CourierStatsV> GetStatsByCourierId(int id)
        {
            return await _repository.GetStatsByCourierId(id);
        }
    }
}
