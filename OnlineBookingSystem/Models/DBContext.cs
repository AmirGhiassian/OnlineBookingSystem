using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using OnlineBookingSystem.Models;
namespace OnlineBookingSystem
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        { }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }



    }
}