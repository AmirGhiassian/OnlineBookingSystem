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


            builder.Services.AddDefaultIdentity<Customer>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<IdentityContext>();

            builder.Services.AddDbContext<ResturantContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("ResturantConnection"));

            });

            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));


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

