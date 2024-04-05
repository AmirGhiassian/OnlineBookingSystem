using Microsoft.AspNetCore.Mvc;
using OnlineBookingSystem.Models;
using Microsoft.AspNetCore.Identity;
namespace OnlineBookingSystem.Controllers
{
    public class HomeController : Controller
    {

        private readonly DBContext _context; //Singleton Database Context

        //ViewResult for Index.cshtml, to be called when a button is clicked
        public ViewResult Index()
        {
            return View("LoginPage"); //Bring user to starting login page
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Reservations()
        {
            return View();
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