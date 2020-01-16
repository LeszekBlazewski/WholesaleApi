using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wholesale.BL.Enums;
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
            var prodcuts = await _context.Products.Where(x => model.OrderDetails.Select(z => z.Product).Contains(x)).ToListAsync();

            model.OrderDetails.ToList().ForEach(o => o.Product = prodcuts.First(y => y.ProductId == o.Product.ProductId));

            prodcuts.ForEach(z =>
            {
                if (z.Stock > 0)
                    z.Stock--;
            });

            _context.Products.UpdateRange(prodcuts);

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
                .Include(u => u.Client)
                .ThenInclude(v => v.Address)
                .ToListAsync();
        }

        public async Task<Order> GetById(int id)
        {
            return await _context.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(y => y.Product)
                .ThenInclude(z => z.Category)
                .Include(u => u.Client)
                .ThenInclude(v => v.Address)
                .FirstOrDefaultAsync(u => u.OrderId == id);
        }

        public async Task<Order> Update(Order model)
        {
            var orderToUpdate = await _context.Orders.FindAsync(model.OrderId);
            if (orderToUpdate == null)
                throw new InvalidOperationException("Order does not exist");

            if (model.Status == OrderStatus.InProgress && orderToUpdate.Status != OrderStatus.Created)
                throw new InvalidOperationException("This order is not available anymore. Please refresh your order list");

            orderToUpdate.CourierId = model.CourierId;
            orderToUpdate.Status = model.Status;

            _context.Update(orderToUpdate);
            await _context.SaveChangesAsync();
            return orderToUpdate;
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

        public async Task<IList<Order>> GetAllAvailable()
        {
            return await _context.Orders
                .Where(x => x.Status == OrderStatus.Created)
                .Include(x => x.OrderDetails)
                .ThenInclude(y => y.Product)
                .ThenInclude(z => z.Category)
                .Include(u => u.Client)
                .ThenInclude(v => v.Address)
                .ToListAsync();
        }

        public async Task<IList<Order>> GetForCourierByStatus(int courierId, OrderStatus status)
        {
            return await _context.Orders
                .Where(x => x.Status == status && x.CourierId == courierId)
                .Include(x => x.OrderDetails)
                .ThenInclude(y => y.Product)
                .ThenInclude(z => z.Category)
                .Include(u => u.Client)
                .ThenInclude(v => v.Address)
                .ToListAsync();
        }

        public async Task<decimal> GetOrdersTotalWorth(DateTime from, DateTime to)
        {
            var totalWorth = (await _context.OrderWorth
                .FromSqlInterpolated($"CALL orders_total_worth({from:yyyy-MM-dd}::DATE, {to:yyyy-MM-dd}::DATE, {1m});")
                .ToListAsync())
                .SingleOrDefault()?.TotalWorth;
            return totalWorth ?? 0m;
        }
    }
}
