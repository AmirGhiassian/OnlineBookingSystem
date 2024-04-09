using System.ComponentModel.DataAnnotations;


/// <summary>
/// Author: Daniel O'Brien
/// This class defines a model for managing reservations within an Online Booking System, 
/// encapsulating details such as reservation date, time, party size, and contact information.
/// It includes validation for required fields and a method to check if any essential reservation information is missing.
/// </summary>


namespace OnlineBookingSystem.Models
{
    /// <summary>
    /// Model class for a single reservation, used for storing reservation information within the database of reservations
    /// </summary>
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        public int RestaurantId { get; set; } //Foreign key to the restaurant where the reservation is made
        [Required]
        public string? Name { get; set; } //Name of the person who books the reservation
        [Required]
        public string? Email { get; set; } //Email of the reservation
        [Required]
        public string? Phone { get; set; } //Phone number of the user
        [Required]
        public string? Date { get; set; } //The DateAndTime requested for reservation
        //Time of the date will be used to determine the special prices for the reservation
        [Required]
        public TimeSpan Time { get; set; } // The time of the reservation
        public double Price { get; set; } // The price of the reservation
        [Required]
        public int PartySize { get; set; }
        public string? SpecialRequests { get; set; } //Optional field

        public bool isEmpty()
        {
            if (Name == null || Email == null || Phone == null || Date == null || Time == null || PartySize == 0)
            {
                return true;
            }
            return false;
        }

    }


}