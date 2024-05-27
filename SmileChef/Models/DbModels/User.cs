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
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(15, ErrorMessage = "Password cannot exceed 15 characters")]
        [Length(6, 15, ErrorMessage = "Password length needs to be between 6 to 15 characters")]
        public string Password { get; set; }
    }
}
