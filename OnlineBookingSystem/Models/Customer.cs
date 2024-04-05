using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNet.Identity.EntityFramework;


namespace OnlineBookingSystem.Models
{
    public class Customer : IdentityUser //Model class for a single customer, used for registration within the CustomerRepo
    {
        [Required]
        public required List<Reservation> Reservations { get; set; }
    }
}