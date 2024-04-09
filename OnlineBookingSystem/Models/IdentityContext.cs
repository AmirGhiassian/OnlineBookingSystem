using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineBookingSystem.Models
{


    /// <summary>
    /// Author: Amir Ghiassian
    /// This class is used to create a context for the identity database. This class inherits from IdentityDbContext
    /// which is a class that is used to store user information in the database. This class has a constructor that
    /// takes in a DbContextOptions object and passes it to the base constructor. This class also has an OnModelCreating
    /// method that takes in a ModelBuilder object and passes it to the base OnModelCreating method. This class is used
    /// to create a context for the identity database.
    /// </summary>

    public class IdentityContext : IdentityDbContext<Customer>
    {
        /// <summary>
        /// Constructor that takes in a DbContextOptions object and passes it to the base constructor.
        /// </summary>
        /// <param name="options"></param>
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// This method is used to create a model for the identity database.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }

}