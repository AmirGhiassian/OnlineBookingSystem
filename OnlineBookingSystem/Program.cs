using OnlineBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

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

