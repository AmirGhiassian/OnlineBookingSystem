using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}