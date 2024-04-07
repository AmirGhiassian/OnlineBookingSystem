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


        private void ResturantDatabaseInit()
        {
            var restaurants = new List<Restaurant>
            {
            new Restaurant
            {
                Name = "McDonalds",
                Address = "1234 Main St",
                Phone = "134-386-9753",
                Description = "Best and most popular fast food restaurant in the world.",
                Image = "https://www.mcdonalds.com/is/image/content/dam/usa/nfl/nutrition/items/hero/desktop/t-mcdonalds-Big-Mac.jpg?$Product_Desktop$",
                reservedTimes = Array.Empty<Reservation>()
            },
            new Restaurant
            {
                Name = "Burger King",
                Address = "5678 Main St",
                Phone = "905-072-9075",
                Description = "Customizable burgers and sandwiches.",
                Image = "https://www.bk.com/sites/default/files/03202020_BK_Web_LTO_Whopper_0.png",
                reservedTimes = Array.Empty<Reservation>()
            },
            new Restaurant
            {
                Name = "Wendy's",
                Address = "9101 Main St",
                Phone = "905-783-8453",
                Description = "Fresh, never frozen beef burgers.",
                Image = "https://www.wendys.com/en-us/assets/menu/product/cheeseburger-2x.png",
                reservedTimes = Array.Empty<Reservation>()
            },
            new Restaurant
            {
                Name = "Taco Bell",
                Address = "1122 Main St",
                Phone = "238-493-8652",
                Description = "Mexican-inspired fast food.",
                Image = "https://www.tacobell.com/images/21499_cheesy-gordita-crunch.png",
                reservedTimes = Array.Empty<Reservation>()
            }
            };

            foreach (var restaurant in restaurants)
            {
                var existingRestaurant = _context.Restaurants.FirstOrDefault(r => r.Name == restaurant.Name);
                if (existingRestaurant == null)
                {
                    _context.Restaurants.Add(restaurant);
                }
                else
                {
                    existingRestaurant.Address = restaurant.Address;
                    existingRestaurant.Phone = restaurant.Phone;
                    existingRestaurant.Description = restaurant.Description;
                    existingRestaurant.Image = restaurant.Image;
                }
            }

            _context.SaveChanges();
        }

        public HomeController(ResturantContext context, UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            ResturantDatabaseInit();
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
            return View(reservation);
        }

        public async Task<IActionResult> ViewReservations()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var reservations = _context.Reservations.Where(r => r.CustId == Convert.ToInt32(user.Id)).ToList();

            return View(reservations);
        }

        public IActionResult EditReservation(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        public IActionResult EditReservation(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Reservations.Update(reservation);
                _context.SaveChanges();
                return RedirectToAction("ViewReservations");
            }

            return View("MakeNewRes", reservation);
        }

        public IActionResult DeleteReservation(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            return RedirectToAction("ViewReservations");
        }


        //HttpGet for Profile.cshtml
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email
            };

            return View(model);
        }



        // [HttpPost]
        // public async Task<IActionResult> UploadPhoto(IFormFile photo)
        // {
        //     var user = await _userManager.GetUserAsync(User);
        //     if (user == null)
        //     {
        //         return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //     }

        //     // Save the photo and update the user's PhotoPath property

        //     return RedirectToAction("Profile");
        // }
    }
}