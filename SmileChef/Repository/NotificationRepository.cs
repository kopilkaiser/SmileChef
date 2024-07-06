using ChefApp.Models;
using SmileChef.Models.DbModels;

namespace SmileChef.Repository
{
    public class NotificationRepository : GenericRepository<NotifySubscribers>
    {
        public NotificationRepository(ChefAppContext context) : base(context)
        {
        }
    }
}
