using System.ComponentModel.DataAnnotations;

///<Summary>
///   Author: Eric Hanoun
///   This class is used to create a restaurant object that will be used to store restaurant information
///   in the database. It has a primary key of RestaurantId which is used to uniquely identify a
///   restaurant in the database. The class has a Name, Address, Phone, Description, and Image field that
///   will be used to store the restaurant's information. This class also has a list of reservations that
///   are used to store the reservations that the restaurant has.
///</Summary>

namespace OnlineBookingSystem.Models
{
    /// <summary>
    /// Model class for a single restaurant, used for storing restaurant information within the database of restaurants
    /// </summary>
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }

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

        
    }
}