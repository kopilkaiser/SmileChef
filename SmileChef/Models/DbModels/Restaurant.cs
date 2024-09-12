using ChefApp.Models.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SmileChef.Models.DbModels
{
    [Table("Restaurants")]
    public class Restaurant
    {
        public int RestaurantId { get; set; }

        [Required(ErrorMessage = "Latitude is required")]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public double Lat { get; set; }

        [Required(ErrorMessage = "Longitude is required")]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public double Lng { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string? Title { get; set; }

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [MaxLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
        public string? Phone { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Operating time cannot exceed 50 characters")]
        public string? OperatingTime { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Location cannot exceed 50 characters")]
        public string? Location { get; set; }

        public Chef Chef { get; set; }

        [ForeignKey(nameof(Chef))]
        public int ChefId { get; set; }
    }
}
