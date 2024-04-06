using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    public class LoginViewModel
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }
    }
}