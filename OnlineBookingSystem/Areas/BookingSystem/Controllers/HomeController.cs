using Microsoft.AspNetCore.Mvc;
using OnlineBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace OnlineBookingSystem.Controllers.Home
{
    [Authorize]
    [Area("Home")]
    public class HomeController : Controller
    {

        private readonly ResturantContext _context; //Singleton Database Context
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;




        private void ResturantDatabaseInit()
        {
            if (_context.Restaurants.Count() == 0)
            {
                _context.Restaurants.Add(new Restaurant
                {
                    Name = "McDonalds",
                    Address = "1234 Main St",
                    Phone = "555-555-5555",
                    Description = "Fast Food",
                    Image = "https://www.mcdonalds.com/is/image/content/dam/usa/nfl/nutrition/items/hero/desktop/t-mcdonalds-Big-Mac.jpg?$Product_Desktop$",
                    reservedTimes = Array.Empty<Reservation>()

                });
                _context.SaveChanges();
            }



        }

        public HomeController(ResturantContext context, UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            ResturantDatabaseInit();
        }







        public IActionResult Dashboard()
        {

            return View(_context.Restaurants.ToList());
        }

        public IActionResult Reservations()
        {
            return View();
        }



        //HttpGet for MakeNewRes.cshtml
        public IActionResult MakeNewRes(int restaurantId) //Get the restaurant ID
        {
            var restaurant = _context.Restaurants.Find(restaurantId); //Find the restaurant with the given ID
            if (restaurant == null)
            {
                return NotFound(); //If the restaurant is not found, return a 404 error
            }

            ViewBag.Restaurant = restaurant; //Pass the restaurant to the view
            return View(); //Return the view
        }

        //HttpPost for MakeNewRes.cshtml
        [HttpPost]
        public IActionResult MakeNewRes(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                // Check if a time has been inputted
                if (reservation.Time != TimeSpan.Zero)
                {
                    // Calculate the price based on the reservation time
                    if (reservation.Time.Hours >= 6 && reservation.Time.Hours < 12)
                    {
                        reservation.Price = 10; // Breakfast price
                    }
                    else if (reservation.Time.Hours >= 12 && reservation.Time.Hours < 18)
                    {
                        reservation.Price = 20; // Lunch price
                    }
                    else if (reservation.Time.Hours >= 18 && reservation.Time.Hours < 23)
                    {
                        reservation.Price = 30; // Dinner price
                    }
                    else
                    {
                        reservation.Price = 0; // Default price
                    }
                }

                // Add the price for guests
                reservation.Price += reservation.PartySize * 1.50;

                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("MakeNewRes", reservation);
        }

        //HttpGet for FeedbackForm.cshtml
        [HttpGet]
        public IActionResult FeedbackForm(int id)
        {
            ViewBag.ReservationId = id; // Pass the reservation ID to the view
            return View();
        }

        //HttpPost for GiveFeedback.cshtml
        [HttpPost]
        public IActionResult GiveFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();

                return RedirectToAction("ShowReservation", new { id = feedback.ReservationId });
            }

            // If model state is not valid, return the same view with the current feedback model
            return View("FeedbackForm", feedback);
        }
    }
}