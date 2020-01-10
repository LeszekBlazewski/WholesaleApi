using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models;

namespace Wholesale.BL.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);

        Task<User> GetById(int id);

        Task<List<User>> GetAll();

        Task<User> Create(User model, string password);

        Task<User> Update(User model, string password = null);

        Task Delete(int id);
    }
}
