using ChefApp.Models.DbModels;

namespace SmileChef.ViewModels
{
    public class SubscriptionViewModel
    {
        public int SubscriptionId { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public decimal Amount { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string? PublisherName { get; set; }
        public string? SubscriberName { get; set; }
    }
}
