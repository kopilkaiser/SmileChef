namespace SmileChef.ViewModels
{
    public class ChefSubsciptionViewModel
    {
        public List<ChefViewModel> Chefs { get; set; } = new();

        public ChefViewModel? CurrentChef { get; set; }
    }
}
