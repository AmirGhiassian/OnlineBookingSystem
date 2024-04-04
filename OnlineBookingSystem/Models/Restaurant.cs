using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    public class Restaurant
    {
        [Key]
        public int ResturantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Reservation[] reservedTimes { get; set; }
    }
}