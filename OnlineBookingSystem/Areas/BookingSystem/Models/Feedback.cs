using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        public int ReservationId { get; set; } //Foreign key to the reservation where the feedback is made

        public int RestaurantId { get; set; } //Foreign key to the restaurant where the feedback is made

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Phone { get; set; }

        [Required]
        public required string Message { get; set; }
    }
}