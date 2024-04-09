using System.ComponentModel.DataAnnotations;

/// <summary>
/// Author: Daniel O'Brien
/// This class is used to create a login view model object that will be used to store login information.
/// It has a required UserName and Password field that will be used to store the user's login information.
/// 

namespace OnlineBookingSystem.Models
{

    /// <summary>
    /// Model class for a single login view model, used for login within the database of users
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}