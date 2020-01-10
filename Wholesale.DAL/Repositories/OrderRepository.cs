using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wholesale.BL.Models;
using Wholesale.BL.RepositoryInterfaces;

namespace Wholesale.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DeliveriesContext _context;

        public OrderRepository(DeliveriesContext context)
        {
            _context = context;
        }

        public async Task<Order> Create(Order model)
        {
            _context.Orders.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<List<Order>> GetAll()
        {
            return await _context.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(y => y.Product)
                .ThenInclude(z => z.Category)
                .ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(y => y.Product)
                .ThenInclude(z => z.Category)
                .FirstOrDefaultAsync(u => u.OrderId == id);
        }

        public async Task<Order> Update(Order model)
        {
            if (await _context.Orders.AllAsync(x => x.OrderId != model.OrderId))
                throw new InvalidOperationException("Order does not exist");
            _context.Update(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task Delete(int id)
        {
            var orderToDelete = await _context.Orders.FindAsync(id);
            if (orderToDelete == null)
                throw new InvalidOperationException("Order does not exist");
            _context.Orders.Remove(orderToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Order>> GetByUserId(int userId)
        {
            return await _context.Orders.Where(w => w.ClientId == userId || w.CourierId == userId)
                .Include(x => x.OrderDetails)
                .ThenInclude(y => y.Product)
                .ThenInclude(z => z.Category)
                .ToListAsync();
        }
    }
}
