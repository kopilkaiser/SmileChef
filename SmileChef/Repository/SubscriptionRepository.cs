using ChefApp.Models;
using ChefApp.Models.DbModels;

namespace SmileChef.Repository
{
    public class SubscriptionRepository : GenericRepository<Subscription>
    {
        public SubscriptionRepository(ChefAppContext context) : base(context)
        {
        }
    }
}
