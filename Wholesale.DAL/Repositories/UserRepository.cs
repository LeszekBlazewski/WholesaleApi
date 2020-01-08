using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wholesale.BL.Models;
using Wholesale.BL.RepositoryInterfaces;

namespace Wholesale.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DeliveriesContext _context;

        public UserRepository(DeliveriesContext context)
        {
            _context = context;
        }

        public async Task<User> GetById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsIdTaken(int id)
        {
            return await _context.Users.AnyAsync(x => x.UserId == id);
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
            return user != null;
        }
    }
}
