using OnlineBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

<<<<<<< HEAD
=======

>>>>>>> 344d11edb121d1cc1d6dbd33df72734b6204df5d

namespace OnlineBookingSystem
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DBContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });




            var app = builder.Build();
            app.UseHttpsRedirection();

            //We might not need this, so commented out
            //app.UseStaticFiles();

            app.UseRouting();

            app.MapDefaultControllerRoute();


            app.Run();



        }

    }
}

