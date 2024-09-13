using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmileChef.Models.DbModels
{
    [Table("Users")]
    [PrimaryKey(nameof(UserId))]
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "User Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Password is required")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters long")]
        [MaxLength(25, ErrorMessage = "Password cannot exceed 25 characters")]
        // Optional: Add regex for password complexity
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Password must have at least one uppercase, one lowercase, one number, and one special character.")]
        public string Password { get; set; }

        public bool? IsAdmin { get; set; }
    }
}
