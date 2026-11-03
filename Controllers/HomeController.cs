using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WeddingManagementSystem.Data;
using WeddingManagementSystem.Models;

namespace WeddingManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.FeaturedPackages = await _context.WeddingPackages
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.Price)
                .Take(3)
                .ToListAsync();

            ViewBag.Services = await _context.ServiceItems
                .Where(s => s.IsActive)
                .Take(6)
                .ToListAsync();

            ViewBag.TotalWeddingsCount = await _context.Bookings.CountAsync(b => b.Status == "Completed" || b.Status == "Confirmed") + 250;
            ViewBag.TotalPackagesCount = await _context.WeddingPackages.CountAsync(p => p.IsActive);
            ViewBag.TotalHappyCouples = await _context.Users.CountAsync(u => u.Role == "Customer") + 500;

            return View();
        }

        public async Task<IActionResult> Services()
        {
            var services = await _context.ServiceItems
                .Where(s => s.IsActive)
                .ToListAsync();
            return View(services);
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                model.SubmittedAt = DateTime.UtcNow;
                model.IsRead = false;
                _context.ContactMessages.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you! Your message has been received. Our wedding concierge will contact you shortly.";
                return RedirectToAction(nameof(Contact));
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
