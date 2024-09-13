using ChefApp.Models.DbModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmileChef.Models.DbModels
{
    [Table("SupportMessages")]
    public class SupportMessage
    {
        [Key]
        public int SupportMessageId { get; set; }

        [Required(ErrorMessage = "Provide page address of the issue")]
        public string? SourceUrl { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string? Message { get; set; }

        [Required(ErrorMessage = "Please select a valid support type")]
        public SupportType? SupportType { get; set; }

        [ValidateNever]
        public Chef? Sender { get; set; }

        [ForeignKey(name: nameof(Sender))]
        public int ChefId { get; set; }

        [ValidateNever]
        public User? AdminUser { get; set; }

        [ForeignKey(name: nameof(AdminUser))]
        public int? UserId { get; set; }

        [ValidateNever]
        public string? AdminMessage { get; set; }

        [ValidateNever]
        public DateTime? Created { get; set; }

        [ValidateNever]
        public DateTime? ClosedDate { get; set; }

        [ValidateNever]
        public SupportStatus SupportStatus { get; set; }
    }

    public enum SupportStatus
    {
        Ongoing,
        Resolved
    }

    public enum SupportType
    {
        Recipe,
        Chef,
        Restaurant,
        Review,
        Order,
        Subscription,
        RecipeBookmark
    }
}
