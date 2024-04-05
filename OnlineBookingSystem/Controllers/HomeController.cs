using Microsoft.AspNetCore.Mvc;
using OnlineBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
namespace OnlineBookingSystem.Controllers
{
    public class HomeController : Controller
    {

        private readonly DBContext _context; //Singleton Database Context
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        //ViewResult for Index.cshtml, to be called when a button is clicked
        public ViewResult Index()
        {
            return View("LoginPage"); //Bring user to starting login page
        }

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
                _ = new IdentityUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(new Customer() { Reservations = new List<Reservation>() }, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(new Customer() { Reservations = new List<Reservation>() }, isPersistent: false);
                    return RedirectToAction("Login");
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
            return View();
        }

        public IActionResult Reservations()
        {
            return View();
        }

        public IActionResult LoginPage() => View(); //Get request for LoginPage.cshtml

        [HttpPost]
        public IActionResult Login(LoginViewModel account)
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(account.Email, account.Password, account.RememberMe, false).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return View("LoginPage");
                }
            }
            return View("LoginPage");
        }

        public IActionResult MakeNewRes() => View(); //Get request for MakeNewRes.cshtml

        [HttpPost]
        public IActionResult MakeNewRes(Reservation reservation) //Post request for MakeNewRes.cshtml
        {
            if (ModelState.IsValid)
            {
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(reservation);
        }
    }
}