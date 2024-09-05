using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmileChef.Models.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChefApp.Models.DbModels
{
    [Table("Chefs")]
    [PrimaryKey(nameof(ChefId))]
    public class Chef
    {
        public int ChefId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MinLength(3, ErrorMessage = "First Name must be at least 3 characters long")]
        [MaxLength(25, ErrorMessage = "First Name cannot exceed 15 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [MinLength(3, ErrorMessage = "Last Name must be at least 3 characters long")]
        [MaxLength(25, ErrorMessage = "Last Name cannot exceed 15 characters")]
        public string LastName { get; set; } = null!;

        [InverseProperty(nameof(Subscription.Subscriber))]
        public List<Subscription> SubscribedTo { get; set; } = new();

        [InverseProperty(nameof(Subscription.Publisher))]
        public List<Subscription> PublishedSubscriptions { get; set; } = new();

        public List<Recipe> Recipes { get; set; } = new();

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
        public int? Rating { get; set; }

        //[Range(1, 35, ErrorMessage = "Subscription cost must be between £1 and £999999.")]
        [CustomRangeAttribute(1, 30, "Subscription Cost")]
        public decimal? SubscriptionCost { get; set; }

        //[Range(1, 999999, ErrorMessage = "Account balance must be between £1 and £25.99.")]
        [CustomRangeAttribute(1, 999999,"Account Balance")]
        public decimal? AccountBalance { get; set; }

        public List<RecipeBookmark> RecipeBookmarks { get; set; } = new();

        [ValidateNever]
        public List<Order> Orders { get; set; }

        public Restaurant Restaurant { get; set; }
    }

    public class CustomRangeAttribute : ValidationAttribute
    {
        private readonly decimal _minimum;
        private readonly decimal _maximum;
        private readonly string _propertyName;

        public CustomRangeAttribute(double minimum, double maximum, string propertyName)
        {
            _minimum = (decimal)minimum;
            _maximum = (decimal)maximum;
            _propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is decimal decimalValue)
            {
                if (decimalValue < _minimum)
                {
                    return new ValidationResult($"{_propertyName} cost must be at least £{_minimum}.");
                }
                else if (decimalValue > _maximum)
                {
                    return new ValidationResult($"{_propertyName} cost must not exceed £{_maximum}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
