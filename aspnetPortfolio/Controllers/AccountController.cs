using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using aspnetPortfolio.Models;
using aspnetPortfolio.dbContext;

namespace aspnetPortfolio.Controllers
{
    public class AccountController : Controller
    {
        private readonly PortofolioDbContext _context;

        public AccountController()
        {
            _context = new PortofolioDbContext(); // Assuming you have a DbContext setup
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check user credentials
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Authentication successful, redirect to the home page or dashboard
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Invalid login attempt
                    ViewBag.ErrorMessage = "Invalid username or password.";                    
                    
                }
                
            }
            return View(model);
            
        }
    }
}
