using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Models;

namespace Wholesale.BL.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetById(int userId);
        Task<User> GetByEmail(string email);
        Task<List<User>> GetAll();
        Task Create(User user);
        Task Update(User user);
        Task Delete(int id);
        Task<bool> IsIdTaken(int id);
        Task<bool> IsEmailTaken(string email);
    }
}
