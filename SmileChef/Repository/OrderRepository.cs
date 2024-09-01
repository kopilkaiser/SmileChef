using ChefApp.Models;
using Microsoft.EntityFrameworkCore;
using SmileChef.Models.DbModels;

namespace SmileChef.Repository
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(ChefAppContext context) : base(context)
        {
            
        }

        public override List<Order>? GetAll()
        {
            var orders = _context.Orders
                .Include(o => o.Chef)
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Recipe)
                .ToList();

            return orders;
        }
    }
}
