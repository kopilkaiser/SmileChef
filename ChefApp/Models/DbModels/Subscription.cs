using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChefApp.Models.DbModels
{
    [Table("Subscriptions")]
    [PrimaryKey(nameof(SubscriptionId))]
    public class Subscription
    {
        [Column(Order = 0)]
        public int SubscriptionId { get; set; }

        [Column(Order = 1)]
        [ForeignKey(nameof(Subscriber))]
        public int SubscriberId { get; set; }

        [Column(Order = 2)]
        [ForeignKey(nameof(Publisher))]
        public int PublisherId { get; set; }

        public Chef? Subscriber { get; set; }

        public Chef? Publisher { get; set; }

        [Column(Order = 3)]
        public DateTime SubscriptionDate { get; set; }

        [Column(Order = 4)]
        [Precision(8, 2)]
        public decimal Amount { get; set; }

        [Column(Order = 5)]
        [EnumDataType(typeof(SubscriptionType))]
        public SubscriptionType SubscriptionType { get; set; }
    }

    public enum SubscriptionType
    {
        Weekly,
        Monthly,
        Yearly
    }
}
