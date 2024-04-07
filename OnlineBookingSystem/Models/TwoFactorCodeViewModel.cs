using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    public class TwoFactorCodeViewModel
    {
        [Required]
        public required string code { get; set; }
    }
}
