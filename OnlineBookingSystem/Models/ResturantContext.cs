using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace OnlineBookingSystem.Models
{

    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        { }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }


        public DbSet<IdentityRole> Roles { get; set; }



    }
}