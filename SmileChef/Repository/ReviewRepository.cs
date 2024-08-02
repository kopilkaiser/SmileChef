using ChefApp.Models;
using ChefApp.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using SmileChef.Models.DbModels;

namespace SmileChef.Repository
{
    public class ReviewRepository : GenericRepository<Review>
    {
        public ReviewRepository(ChefAppContext context) : base(context)
        {
        }

        public override Review? GetById(int id)
        {
            var review = _context.Reviews.Include(r => r.Reviewer)
                .FirstOrDefault(r => r.Id == id);

            if (review == null) throw new Exception($"No Review has been found with id: {id}");

            return review;
        }
    }
}
