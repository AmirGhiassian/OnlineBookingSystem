using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookingSystem.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; } //Foreign key to the restaurant where the reservation is made

        [ForeignKey("Restaurant")]
        public string RestaurantName { get; set; } //Name of the restaurant where the reservation is made

        [ForeignKey("Customer")]
        public int CustId { get; set; } //Foreign key to the customer who made the reservation
        [Required]
        public required string Name { get; set; } //Name of the person who books the reservation
        [Required]
        public required string Email { get; set; } //Email of the reservation
        [Required]
        public required string Phone { get; set; } //Phone number of the user
        [Required]
        public required string Date { get; set; } //The DateAndTime requested for reservation
        //Time of the date will be used to determine the special prices for the reservation
        [Required]
        public TimeSpan Time { get; set; } // The time of the reservation
        public double Price { get; set; } // The price of the reservation
        [Required]
        public int PartySize { get; set; }
        public required string SpecialRequests { get; set; } //Optional field



    }
}