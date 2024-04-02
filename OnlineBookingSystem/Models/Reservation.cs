using System.ComponentModel.DataAnnotations;

namespace OnlineBookingSystem.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        
        public int CustomerId { get; set; } //Foreign key to the customer who made the reservation
        [Required]
        public string Name { get; set; } //Name of the person who books the reservation
        [Required]
        public string Email { get; set; } //Email of the reservation
        [Required]
        public string Phone { get; set; } //Phone number of the user
        [Required]
        public string Date { get; set; } //The DateAndTime requested for reservation
        //Time of the date will be used to determine the special prices for the reservation
        [Required]
        public int PartySize { get; set; }
        public string SpecialRequests { get; set; } //Optional field



    }
}