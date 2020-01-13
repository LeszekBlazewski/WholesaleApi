using System.Collections.Generic;
using System.Threading.Tasks;
using Wholesale.BL.Enums;
using Wholesale.BL.Models;
using Wholesale.BL.RepositoryInterfaces;

namespace Wholesale.BL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> Create(Order model)
        {
            return await _orderRepository.Create(model);
        }

        public async Task<List<Order>> GetAll()
        {
            return await _orderRepository.GetAll();
        }

        public async Task<Order> GetById(int id)
        {
            return await _orderRepository.GetById(id);
        }

        public async Task<Order> Update(Order model)
        {
            return await _orderRepository.Update(model);
        }

        public async Task Delete(int id)
        {
            await _orderRepository.Delete(id);
        }

        public async Task<IList<Order>> GetByUserId(int userId)
        {
            return await _orderRepository.GetByUserId(userId);
        }

        public async Task<IList<Order>> GetAllAvailable()
        {
            return await _orderRepository.GetAllAvailable();
        }

        public async Task<IList<Order>> GetForCourierByStatus(int courierId, OrderStatus status)
        {
            return await _orderRepository.GetForCourierByStatus(courierId, status);
        }
    }
}
