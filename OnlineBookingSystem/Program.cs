using OnlineBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


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

            app.MapRazorPages();
            app.Run();



        }

    }
}

