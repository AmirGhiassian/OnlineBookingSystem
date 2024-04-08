using System.ComponentModel.DataAnnotations;

/// <summary>
/// Author: Amir Ghiassian
/// This class is used to create a register view model object that will be used to store registration information.
///This class has a Username, PhoneNumber, Email, Password, and ConfirmPassword field that 
///will be used to store the user's registration information.
/// </summary>
namespace OnlineBookingSystem.Models
{
    /// <summary>
    /// Model class for a single register view model, used for registration within the database of users
    /// </summary>
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

        /// <summary>
        /// Required ConfirmPassword field that will be used to store the user's confirmation password.
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string? ConfirmPassword { get; set; }
    }
}