using OnlineBookingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OnlineBookingSystem.Areas.Identity.Data;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using Microsoft.AspNetCore.Authorization;


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

            }).AddEntityFrameworkStores<IdentityContext>();

            builder.Services.AddDbContext<RestaurantContext>(opt =>
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

