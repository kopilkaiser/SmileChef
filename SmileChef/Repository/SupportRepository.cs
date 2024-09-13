using ChefApp.Models;
using SmileChef.Models.DbModels;

namespace SmileChef.Repository
{
    public class SupportRepository : GenericRepository<SupportMessage>
    {
        public SupportRepository(ChefAppContext context) : base(context)
        {
        }
    }
}
