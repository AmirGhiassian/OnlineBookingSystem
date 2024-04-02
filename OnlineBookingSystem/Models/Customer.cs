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
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

        [MaybeNull]
        public List<Reservation> Reservations { get; set; }
    }
}