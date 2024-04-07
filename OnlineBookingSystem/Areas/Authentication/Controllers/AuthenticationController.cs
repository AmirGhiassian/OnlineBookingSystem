using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBookingSystem.Models;
namespace OnlineBookingSystem.Controllers.Authentication
{
    [Area("Authentication")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class AuthenticationController(UserManager<Customer> userManager, SignInManager<Customer> signInManager) : Controller
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        private readonly UserManager<Customer> _userManager = userManager;
        private readonly SignInManager<Customer> _signInManager = signInManager;

        private readonly ResturantContext _context; //Singleton Database Context
        private PasswordHasher<Customer> passwordHasher = new PasswordHasher<Customer>();

        //ViewResult for Index.cshtml, to be called when a button is clicked
        public IActionResult Index() => View();



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
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // return the view with the model
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel account)
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
                    return View("Index");
                }
            }
            Debug.WriteLine("Invalid Login Attempt");
            return View("Index");
        }
    }
}
