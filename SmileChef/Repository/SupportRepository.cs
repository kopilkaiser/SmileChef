using ChefApp.Models;
using Microsoft.EntityFrameworkCore;
using SmileChef.Models.DbModels;

namespace SmileChef.Repository
{
    public class SupportRepository : GenericRepository<SupportMessage>
    {
        public SupportRepository(ChefAppContext context) : base(context)
        {
        }

        public override List<SupportMessage>? GetAll()
        {
            var supports = _context.SupportMessages
                .Include(sm => sm.Sender)
                .ThenInclude(s => s.User)
                .Include(sm => sm.AdminUser)
                .ToList();
            return supports;
        }

        public override async Task<List<SupportMessage>>? GetAllAsync()
        {
            var supports = await _context.SupportMessages
                .Include(sm => sm.Sender)
                .ThenInclude(s => s.User)
                .Include(sm => sm.AdminUser)
                .ToListAsync();
            return supports;
        }

        public override SupportMessage? GetById(int id)
        {
            var support = _context.SupportMessages
                 .Include(sm => sm.Sender)
                        .ThenInclude(navigationPropertyPath: s => s.User)
                 .Include(sm => sm.AdminUser)
                 .FirstOrDefault(sm => sm.SupportMessageId == id);

            ArgumentNullException.ThrowIfNull(support);
            return support;
        }
    }
}
