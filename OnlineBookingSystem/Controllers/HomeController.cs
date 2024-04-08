using Microsoft.AspNetCore.Mvc;
using OnlineBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using Microsoft.EntityFrameworkCore;
namespace OnlineBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private string accountSid = "AC4822ed0c1bbe698e9b602ded983f0046";
        private string authToken = "f45e42925a26b1e65588038bfeb956c9";
        private readonly ResturantContext _context; //Singleton Database Context
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private Random random = new Random();
        private int randomNumber;
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
                reservations = new List<Reservation>()
            },
            new Restaurant
            {
                Name = "Burger King",
                Address = "5678 Main St",
                Phone = "905-072-9075",
                Description = "Customizable burgers and sandwiches.",
                Image = "https://www.bk.com/sites/default/files/03202020_BK_Web_LTO_Whopper_0.png",
                reservations = new List<Reservation>()
            },
            new Restaurant
            {
                Name = "Wendy's",
                Address = "9101 Main St",
                Phone = "905-783-8453",
                Description = "Fresh, never frozen beef burgers.",
                Image = "https://www.wendys.com/en-us/assets/menu/product/cheeseburger-2x.png",
                reservations = new List<Reservation>()
            },
            new Restaurant
            {
                Name = "Taco Bell",
                Address = "1122 Main St",
                Phone = "238-493-8652",
                Description = "Mexican-inspired fast food.",
                Image = "https://www.tacobell.com/images/21499_cheesy-gordita-crunch.png",
                reservations = new List<Reservation>()
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
            TwilioClient.Init(accountSid, authToken);
        }



        //ViewResult for Index.cshtml, to be called when a button is clicked
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View("LoginPage"); //Bring user to starting login page
        }

        //HttpGet for Register.cshtml
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        //HttpPost for Register.cshtml
        [HttpPost]
        [AllowAnonymous]
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
                var result = await _userManager.CreateAsync(new Customer()
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Reservations = new List<int>(),
                    PasswordHash = passwordHasher.HashPassword(new Customer(), model.Password),
                    LockoutEnabled = false,
                    TwoFactorEnabled = false // This changes flow for website due to 2 fac auth, change to false to get the id with just pass
                });

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


        [AllowAnonymous]
        [HttpGet]
        public IActionResult TwoFactor(Customer cust)
        {

            randomNumber = random.Next(100000, 999999);

            VerificationResource.Create(
            to: $"+1{cust.PhoneNumber}",
            channel: "sms",
            pathServiceSid: "VAadb25fa50d3ef1770730417427840f75"
        );

            VerificationCheckResource.Create(
                 to: $"+1{cust.PhoneNumber}",
                 code: randomNumber.ToString(),
                 pathServiceSid: "VAadb25fa50d3ef1770730417427840f75"
             );

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TwoFactor(TwoFactorCodeViewModel code)
        {
            if (ModelState.IsValid)
            {
                if (code.code == randomNumber.ToString())
                {
                    var result = await _signInManager.TwoFactorSignInAsync("Phone", code.code, isPersistent: false, rememberClient: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Code");
                        return View();
                    }
                }

            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            return View(_context.Restaurants.ToList());
        }
        [Authorize]
        public IActionResult Reservations()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult LoginPage() => View(); //Get request for LoginPage.cshtml

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginPage(LoginViewModel account)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(account.UserName);

                var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, account.Password);
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    // Check if the user has two-factor authentication enabled
                    if (await _userManager.GetTwoFactorEnabledAsync(user))
                    {
                        // Generate and send the two-factor code, then redirect the user to the TwoFactor action
                        // ...
                        return RedirectToAction("TwoFactor", user);
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, account.Password, false, lockoutOnFailure: false);
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
        [Authorize]
        public IActionResult MakeNewRes(int restaurantId) //Get the restaurant ID
        {
            var restaurant = _context.Restaurants.Find(restaurantId); //Find the restaurant with the given ID

            if (restaurant == null)
            {
                return NotFound(); //If the restaurant is not found, return a 404 error
            }


            return View(new Wrapper(new Reservation(), _context.Restaurants.Find(restaurantId))); //Return the view
        }

        //HttpPost for MakeNewRes.cshtml
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MakeNewRes(Reservation Reservation)
        {
            if (ModelState.IsValid)
            {
                // Check if a time has been inputted
                if (Reservation.Time != TimeSpan.Zero)
                {
                    // Calculate the price based on the reservation time
                    if (Reservation.Time.Hours >= 6 && Reservation.Time.Hours < 12)
                    {
                        Reservation.Price = 10; // Breakfast price
                    }
                    else if (Reservation.Time.Hours >= 12 && Reservation.Time.Hours < 18)
                    {
                        Reservation.Price = 20; // Lunch price
                    }
                    else if (Reservation.Time.Hours >= 18 && Reservation.Time.Hours < 23)
                    {
                        Reservation.Price = 30; // Dinner price
                    }
                    else
                    {
                        Reservation.Price = 0; // Default price
                    }
                }


                // Add the price for guests
                Reservation.Price += Reservation.PartySize * 1.50;


                _context.Reservations.Add(Reservation);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(new Wrapper(Reservation, _context.Restaurants.Find(Reservation.RestaurantId), await _userManager.FindByIdAsync(_userManager.GetUserId(User))));
        }

        public async Task<IActionResult> ViewReservation()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ;
            if (user == null)
            {
                return NotFound("The user is not a customer.");
            }

            return View(_context.Reservations.ToList());
        }


        public async Task<IActionResult> EditReservation(string id)
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

        public IActionResult DeleteReservation(string id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            return RedirectToAction("ViewReservation");
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

        [Authorize(Roles = "Admin")]
        public IActionResult UserManagement()
        {

            var users = _userManager.Users.ToList();
            return View(users);
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