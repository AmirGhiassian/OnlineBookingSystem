using Microsoft.AspNetCore.Mvc;
using OnlineBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

/// <summary>
/// Author: Amir Ghiassian
/// The HomeController class manages the main operations of the application, 
/// including user authentication, reservation management, and communication with external services like Twilio.
/// It uses the RestaurantContext to interact with the database, 
/// and the UserManager and SignInManager for user authentication and management.
/// </summary>

namespace OnlineBookingSystem.Controllers
{
    /// <summary>
    /// The controller class for the backbone of the application
    /// </summary>
    /// 

    [Authorize]
    public class HomeController : Controller
    {
        private string accountSid = "AC4822ed0c1bbe698e9b602ded983f0046";
        private string authToken = "f45e42925a26b1e65588038bfeb956c9";
        private readonly RestaurantContext _context; //Singleton Database Context
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private Random random = new Random();

        private static Dictionary<string, string> authData = new Dictionary<string, string>(3);
        private PasswordHasher<Customer> passwordHasher = new PasswordHasher<Customer>();

        /// <summary>
        /// Method Author: Eric Hanoun
        /// Initializes the database with a set of default restaurants if they do not already exist.
        /// This method is called when the HomeController is created.
        /// </summary>
        [AllowAnonymous]
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
                Image = "https://s7d1.scene7.com/is/image/mcdonalds/franchisinghub-homepage-hero-desktop:hero-desktop?resmode=sharp2",
                reservations = new List<Reservation>()
            },
            new Restaurant
            {
                Name = "Burger King",
                Address = "5678 Main St",
                Phone = "905-072-9075",
                Description = "Customizable burgers and sandwiches.",
                Image = "https://cdn.forumcomm.com/dims4/default/44a81cf/2147483647/strip/true/crop/2016x1512+0+0/resize/1421x1066!/quality/90/?url=https%3A%2F%2Fforum-communications-production-web.s3.us-west-2.amazonaws.com%2Fbrightspot%2Fde%2F34%2F982450d34f4184c5662d5b4df757%2Fimg-0089.jpg",
                reservations = new List<Reservation>()
            },
            new Restaurant
            {
                Name = "Wendy's",
                Address = "9101 Main St",
                Phone = "905-783-8453",
                Description = "Fresh, never frozen beef burgers.",
                Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRCaGKnO43i2s8TG7FCBhbx7OQojmi3h-GTJTjul7CpkQ&s",
                reservations = new List<Reservation>()
            },
            new Restaurant
            {
                Name = "Taco Bell",
                Address = "1122 Main St",
                Phone = "238-493-8652",
                Description = "Mexican-inspired fast food.",
                Image = "https://s3-media0.fl.yelpcdn.com/bphoto/rWo5CFW-I0VV5lQM8tkg-Q/348s.jpg",
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

        /// <summary>
        /// Author: Amir Ghiassian
        /// Constructor for the HomeController class, taking in a RestaurantContext, UserManager, and SignInManager object.
        /// </summary>

        public HomeController(RestaurantContext context, UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            ResturantDatabaseInit();
            TwilioClient.Init(accountSid, authToken);
        }



        /// <summary>
        /// Author: Eric Hanoun
        /// </summary>
        /// <returns> returns a view with the Login Page as the default page to show on startup</returns>
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View("LoginPage"); //Bring user to starting login page
        }

        /// <summary>
        /// Author: Eric Hanoun
        /// HttpGet method for Register.cshtml
        /// </summary>
        /// <returns> returns a default view for the register page </returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        /// <summary>
        /// Author: Eric Hanoun
        /// Handles the HTTP POST request for user registration. 
        /// It validates the model state, checks if the password and confirmation password match, 
        /// creates a new Customer object, and attempts to register the user with the UserManager.
        /// If the registration is successful, it redirects to the login page. 
        /// If not, it adds the errors to the ModelState and returns the view with the model.
        /// </summary>
        /// <param name="model">The RegisterViewModel instance containing the user's registration information.</param>
        /// <returns>
        /// If the model state is valid and the user registration is successful, it redirects to the login page. 
        /// If the model state is invalid or the user registration fails, it returns the view with the model.
        /// </returns>
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
                    TwoFactorEnabled = true // This changes flow for website due to 2 fac auth, change to false to get the id with just pass
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

        /// <summary>
        /// Author: Amir Ghiassian
        /// Handles the HTTP GET request for two-factor authentication. 
        /// It generates a random number, sends a verification SMS to the user's phone number, 
        /// and checks the verification code sent to the user's phone number.
        /// </summary>
        /// <param name="cust">The Customer instance containing the user's information.</param>
        /// <returns>
        /// Returns the view for two-factor authentication.
        /// </returns>

        [HttpGet]
        [AllowAnonymous]
        public IActionResult TwoFactor()
        {
            if (ModelState.IsValid)
            {
                var PhoneNumber = authData["PhoneNumber"];
                VerificationResource.Create(
                to: $"+1{PhoneNumber}",
                channel: "sms",
                pathServiceSid: "VAadb25fa50d3ef1770730417427840f75"
            );
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Phone Number");
            }

            return View();

        }

        /// <summary>
        /// Author: Amir Ghiassian
        /// Handles the HTTP POST request for two-factor authentication.
        /// It checks if the two-factor code entered by the user matches the random number generated.
        /// If the code is correct, it signs in the user and redirects to the dashboard.
        /// If the code is incorrect, it adds an error to the ModelState and returns the view.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>if code entered is correct, returns user to the Dashboard, if the code is wrong, states invalid code</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TwoFactor(TwoFactorCodeViewModel codeEntered)
        {

            if (ModelState.IsValid)
            {
                var PhoneNumber = authData["PhoneNumber"].ToString();
                var result = VerificationCheckResource.Create(
                      to: $"+1{PhoneNumber}",
                      code: codeEntered.code,
                      pathServiceSid: "VAadb25fa50d3ef1770730417427840f75"
                  );
                if (result.Status == "approved")
                {
                    var user = await _userManager.FindByNameAsync(authData["UserName"].ToString());
                    var signInResult = await _signInManager.PasswordSignInAsync(user, authData["Password"].ToString(), false, lockoutOnFailure: false);
                    if (signInResult.Succeeded)
                    {
                        authData.Clear();
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

        /// <summary>
        /// Author: Daniel O'Brien
        /// Handles the HTTP GET request for the Dashboard view.
        /// </summary>
        /// <returns> returns a view for the dashboard containing the list of restaurants </returns>

        public async Task<IActionResult> Dashboard()
        {
            return View(_context.Restaurants.ToList());
        }

        /// <summary>
        /// Author: Daniel O'Brien
        /// Handles the HTTP GET request for the Reservations view.
        /// </summary>
        /// <returns>Returns the Reservations view</returns>

        public IActionResult Reservations()
        {
            return View();
        }

        /// <summary>
        /// Author: Eric Hanoun
        /// Handles the HTTP GET request for the Login Page.
        /// </summary>
        /// <returns>Returns the Login Page view</returns>
        [AllowAnonymous]
        public IActionResult LoginPage() => View();

        /// <summary>
        /// Author: Eric Hanoun
        /// Handles the HTTP POST request for the Login Page.
        /// It validates the model state, finds the user by username, verifies the password,
        /// and signs in the user if the verification is successful.
        /// If the verification is successful, it redirects to the Dashboard.
        /// If the verification is unsuccessful, it adds an error to the ModelState and returns the view.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginPage(LoginViewModel account)
        {
            // if (HttpContext.Items.ContainsKey("Unauthorized"))
            // {
            //     ModelState.AddModelError("Unauthorized", HttpContext.Items["Unauthorized"].ToString());
            // }


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


                        authData.Add("UserName", account.UserName);
                        authData.Add("PhoneNumber", user.PhoneNumber);
                        authData.Add("Password", account.Password);


                        return RedirectToAction("TwoFactor");
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


        /// <summary>
        /// Author: Eric Hanoun
        /// Handles the HTTP GET request for the MakeNewRes view.
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns>If the restaurant is not found by id and is null, it returns NotFound.
        /// If the return is successful it returns a wrapper object of a new Reservation and the restaurant with the needed restaurant id
        /// </returns>

        public async Task<IActionResult> MakeNewRes(int? restaurantId, int? reservationId) //Get the restaurant ID
        {
            var restaurant = _context.Restaurants.Find(restaurantId); //Find the restaurant with the given ID

            if (restaurant == null)
            {
                return NotFound(); //If the restaurant is not found, return a 404 error
            }

            Reservation reservation;
            if (reservationId.HasValue)
            {
                //If a reservation id is provided, find the reservation with the given ID
                reservation = _context.Reservations.Find(reservationId);
            }
            else
            {   //If no reservation id is provided, create a new reservation object
                reservation = new Reservation();
            }

            return View(new Wrapper(reservation, restaurant, await _userManager.FindByIdAsync(_userManager.GetUserId(User)))); //Return the view
        }


        /// <summary>
        /// Author: Eric Hanoun
        /// Handles the HTTP POST request for the MakeNewRes view.
        /// It validates the model state, calculates the price based on the reservation time and party size,
        /// and adds the reservation to the database.
        /// This method also allows users to edit their previously made reservations, and determines if the 
        /// reservation has already been made, allowing editing to occur. It then saves the changes to the database.
        /// </summary>
        /// <param name="Reservation"></param>
        /// <returns>
        /// Returns a wrapper object containing: 
        ///     1. The reservation object
        ///     2. The restaurant object
        /// </returns>
        [HttpPost]
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
                // Check if a reservation with the same ID already exists
                var existingReservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == Reservation.ReservationId);
                if (existingReservation != null)
                {
                    // Update the existing reservation
                    _context.Entry(existingReservation).CurrentValues.SetValues(Reservation);
                }
                else
                {
                    // Add the new reservation
                    _context.Reservations.Add(Reservation);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View(new Wrapper(Reservation, _context.Restaurants.Find(Reservation.RestaurantId, await _userManager.FindByIdAsync(_userManager.GetUserId(User)))));
        }

        /// <summary>
        /// Author: Eric Hanoun
        /// Handles the HTTP GET request for the ViewReservations view.
        /// </summary>
        /// <returns>
        /// If the user is not found, the profile retuns an error, user not found
        /// If teh user is not a customer, the profile returns an error, user is not a customer
        /// Other wise, the list of reservations is returned to the list of reservations view
        /// </returns>
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

            return View(new Wrapper(_context.Reservations.ToList(), _context.Restaurants.ToList()));
        }

        /// <summary>
        /// Author: Eric Hanoun
        /// Handles the HTTP GET request for the EditReservation view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// If the reservation is not found by id and is null, it returns NotFound.
        /// If the return is successful it returns the reservation object
        /// </returns>
        public async Task<IActionResult> EditReservation(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        /// <summary>
        /// Author: Eric Hanoun
        /// Handles the HTTP POST request for the EditReservation view.
        /// It validates the model state, updates the reservation in the database, and redirects to the ViewReservations view.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns>
        /// If the model state is valid, the reservation is updated in the database, and the user is redirected to the ViewReservations view.
        /// If the model state is invalid, the user is returned to the EditReservation view with the reservation object.
        /// </returns>
        [HttpPost]
        public IActionResult EditReservation(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Reservations.Update(reservation);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(reservation);
        }

        /// <summary>
        /// Author:  Eric Hanoun
        /// Handles the HTTP GET request for the DeleteReservation view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// if the reservation is not found, returns NotFound, 
        /// if it is found, deletes it and returns the ViewReservation again
        /// </returns>
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


        /// <summary>
        /// Author: Eric Hanoun
        /// Handles the HTTP GET request for the Profile view.
        /// </summary>
        /// <returns>
        /// If the user is not found, the profile retuns an error, user not found
        /// If the return is successful it returns the profile view model
        /// </returns>
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

        /// <summary>
        /// Author: Daniel O'Brien
        /// Handles the HTTP GET request for the UserManagement view.
        /// Allows an admin to view all users in the database.
        /// </summary>
        /// <returns>
        /// Returns the UserManagement view with a list of all users in the database.
        /// </returns>
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