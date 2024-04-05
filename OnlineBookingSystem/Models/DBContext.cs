using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
namespace OnlineBookingSystem
{

    public class DBContext : IdentityDbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        { }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }


        public DbSet<IdentityUser> Customers { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }

    }
}