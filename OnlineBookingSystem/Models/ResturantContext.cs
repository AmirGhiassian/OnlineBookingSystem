using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Author: Daniel O'Brien
/// This class extends DbContext to define a context for a restaurant booking system, 
/// managing entities such as Reservations, Feedback, Restaurants, and Roles within a database.
/// It facilitates the interaction between the application and the underlying database structure.
/// </summary>
namespace OnlineBookingSystem.Models
{

    /// <summary>
    /// Represents the database context for the restaurant booking system.
    /// </summary>
    public class RestaurantContext : DbContext
    {
        /// <summary>
        /// Constructor for the RestaurantContext class, 
        /// taking in a DbContextOptions object and passing it to the base constructor.
        /// </summary>
        /// <param name="options">The options to be used by a DbContext.</param>
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        { }



        /// <summary>
        /// Represents the set of all Feedback in the database.
        /// </summary>
        public DbSet<Feedback> Feedbacks { get; set; }

        /// <summary>
        /// Represents the set of all Restaurants in the database.
        /// </summary>
        public DbSet<Restaurant> Restaurants { get; set; }

        /// <summary>
        /// Represents the set of all Roles in the database.
        /// </summary>
        public DbSet<IdentityRole> Roles { get; set; }



    }
}