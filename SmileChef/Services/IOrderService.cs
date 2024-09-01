using SmileChef.Models.DbModels;

namespace SmileChef.Services
{
    public interface IOrderService
    {
        Task<Order?> GetOrderByIdAsync(int id);
        Task<bool> SaveOrderAsync(Order order);
        // Other methods to manipulate orders
    }
}
