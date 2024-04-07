using Microsoft.AspNetCore.Mvc;
using OnlineBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
namespace OnlineBookingSystem.Controllers
{
    public class HomeController : Controller
    {

        private readonly ResturantContext _context; //Singleton Database Context
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;

        private PasswordHasher<Customer> passwordHasher = new PasswordHasher<Customer>();

        public HomeController(ResturantContext context, UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        //ViewResult for Index.cshtml, to be called when a button is clicked
        public ViewResult Index()
        {
            return View("LoginPage"); //Bring user to starting login page
        }

        //HttpGet for Register.cshtml
        [HttpGet]
        public IActionResult Register() => View();

        //HttpPost for Register.cshtml
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if Password and ConfirmPassword fields match
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Password and confirmation password do not match.");
                    return View(model);
                }

                // Create a new IdentityUser object with the user's username and email
                var result = await _userManager.CreateAsync(new Customer() { UserName = model.Username, Email = model.Email, Reservations = new List<Reservation>(), PasswordHash = passwordHasher.HashPassword(new Customer(), model.Password) });

                if (result.Succeeded)
                {
                    return RedirectToAction("LoginPage");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // return the view with the model
            return View(model);
        }



        public IActionResult Dashboard()
        {

            return View(_context.Restaurants.ToList());
        }

        public IActionResult Reservations()
        {
            return View();
        }

        public IActionResult LoginPage() => View(); //Get request for LoginPage.cshtml

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel account)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(account.UserName);

                Debug.WriteLine(user);
                var result = await _signInManager.CheckPasswordSignInAsync(user, account.Password, false);
                Debug.WriteLine(result);
                if (result.Succeeded)
                {
                    return RedirectToAction("Dashboard", _context.Restaurants.ToList());
                }
                else
                {
                    Debug.WriteLine("Invalid Login Attempt");
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return View("LoginPage");
                }
            }
            Debug.WriteLine("Invalid Login Attempt");
            return View("LoginPage");
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
                if (reservation.Time != null)
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
            return View(reservation);
        }
    }
}