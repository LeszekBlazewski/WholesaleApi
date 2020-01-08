using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models;

namespace Wholesale.BL.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);

        Task<User> GetById(int userId);

        Task<IEnumerable<User>> GetAll();

        Task<User> Create(User user, string password);

        Task<User> Update(User user, string password = null);

        Task Delete(int id);
    }
}
