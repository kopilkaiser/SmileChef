using ChefApp.Models;
using Microsoft.EntityFrameworkCore;
using SmileChef.Models.DbModels;

namespace SmileChef.Services
{
    public class OrderService : IOrderService
    {
        private readonly ChefAppContext _context;

        public OrderService(ChefAppContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await 
                _context.Orders.Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<bool> SaveOrderAsync(Order order)
        {
            bool result = false;
            try { 
                if (order.OrderId == 0)
                {
                    _context.Orders.Add(order);
                }
                else
                {
                    _context.Orders.Update(order);
                }

                await _context.SaveChangesAsync();
                result = true;
            }catch(Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
