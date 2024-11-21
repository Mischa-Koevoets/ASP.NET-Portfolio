using Microsoft.AspNetCore.Mvc;
using aspnetPortfolio.dbContext;

namespace aspnetPortfolio.Controllers
{
    public class TagController : Controller
    {
        private readonly PortofolioDbContext _context;

        public TagController(PortofolioDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Tags.Add(tag);
                _context.SaveChanges();
                return RedirectToAction("Index", "Project");
            }
            return View(tag);
        }
    }
}
