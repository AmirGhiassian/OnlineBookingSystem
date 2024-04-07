using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
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