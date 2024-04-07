using OnlineBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OnlineBookingSystem.Areas.Identity.Data;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Verify.V2.Service;


namespace OnlineBookingSystem
{

    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDefaultIdentity<Customer>(options =>
            {

                //Different options can be set here

                // options.SignIn.RequireConfirmedAccount = false;
                // options.SignIn.RequireConfirmedEmail = false;
                // options.SignIn.RequireConfirmedPhoneNumber = false;
                // options.User.RequireUniqueEmail = true;
                // options.Lockout.AllowedForNewUsers = true;
                // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                // options.Lockout.MaxFailedAccessAttempts = 5;
                // options.Password.RequireDigit = true;
                // options.Password.RequireLowercase = true;
                // options.Password.RequireNonAlphanumeric = true;
                // options.Password.RequireUppercase = true;
                // options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<IdentityContext>();

            builder.Services.AddDbContext<ResturantContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("ResturantConnection"));

            });

            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));


            var app = builder.Build();
            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.MapDefaultControllerRoute();

            app.UseAuthorization();
            app.Run();



        }

    }
}

