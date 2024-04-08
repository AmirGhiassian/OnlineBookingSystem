using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    public class Restaurant
    {
        [Key]
        public int ResturantId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? Phone { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Image { get; set; }

        [Required]
        public List<Reservation> reservations { get; set; } = new List<Reservation>();
    }
}