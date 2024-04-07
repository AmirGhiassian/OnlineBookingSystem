using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    public class Restaurant
    {
        [Key]
        public int ResturantId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string Image { get; set; }

        [Required]
        public ICollection<Reservation> reservedTimes { get; set; } = new List<Reservation>();
    }
}