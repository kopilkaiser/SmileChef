using ChefApp.Models.DbModels;
using SmileChef.Models.DbModels;
using SmileChef.ViewModels;

namespace SmileChef.Repository
{
    public interface IChefRepository : IRepository<Chef>
    {
        List<ChefViewModel> GetChefsWithDetails();

        Task<List<ChefViewModel>> GetChefsWithDetailsAsync();

        List<Restaurant> GetAllRestaurants();

        Restaurant GetRestaurantByChefId(int chefId);
    }
}
