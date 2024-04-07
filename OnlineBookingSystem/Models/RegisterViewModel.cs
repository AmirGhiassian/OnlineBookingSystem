using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    //Model class for a single customer, used for registration within the database of customers
    public class RegisterViewModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }
        
        [Required]
        [EmailAddress]
        public required string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string? ConfirmPassword { get; set; }
    }
}