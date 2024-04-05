using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNet.Identity.EntityFramework;


namespace OnlineBookingSystem.Models
{
    public class Customer : IdentityUser //Model class for a single customer, used for registration within the CustomerRepo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string NormalizedUserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [MaybeNull]

        public List<Reservation> Reservations { get; set; }
    }
}