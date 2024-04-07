using Microsoft.AspNetCore.Mvc;
using OnlineBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
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
                    Reservations = new List<Reservation>(),
                    PasswordHash = passwordHasher.HashPassword(new Customer(), model.Password),
                    LockoutEnabled = false,
                    TwoFactorEnabled = true
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

            ViewBag.Restaurant = restaurant; //Pass the restaurant to the view
            return View(); //Return the view
        }

        //HttpPost for MakeNewRes.cshtml
        [HttpPost]
        [Authorize]
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
    }
}