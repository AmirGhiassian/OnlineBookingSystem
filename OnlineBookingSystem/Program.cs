using OnlineBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OnlineBookingSystem.Areas.Identity.Data;


namespace OnlineBookingSystem
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();


            builder.Services.AddDefaultIdentity<Customer>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<OnlineBookingSystemIdentityDbContext>();

            builder.Services.AddDbContext<DBContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            builder.Services.AddDbContext<OnlineBookingSystemIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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

