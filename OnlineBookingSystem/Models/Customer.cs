using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace OnlineBookingSystem.Models
{
    public class Customer //Model class for a single customer, used for registration within the CustomerRepo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [MaybeNull]
        public List<Reservation> Reservations { get; set; }
    }
}