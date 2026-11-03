using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WeddingManagementSystem.Data;
using WeddingManagementSystem.Models;
using WeddingManagementSystem.Models.ViewModels;

namespace WeddingManagementSystem.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Bookings
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var isAdmin = User.IsInRole("Admin");

            var bookingsQuery = _context.Bookings
                .Include(b => b.Package)
                .Include(b => b.User)
                .AsQueryable();

            if (!isAdmin)
            {
                bookingsQuery = bookingsQuery.Where(b => b.UserId == userId);
            }

            var bookings = await bookingsQuery
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            return View(bookings);
        }

        // GET: /Bookings/Create?packageId=1
        public async Task<IActionResult> Create(int packageId)
        {
            if (User.IsInRole("Admin"))
            {
                TempData["InfoMessage"] = "Administrators manage client reservations from the Admin Panel.";
                return RedirectToAction("Bookings", "Admin");
            }

            var package = await _context.WeddingPackages.FindAsync(packageId);
            if (package == null || !package.IsActive)
            {
                TempData["ErrorMessage"] = "Selected package is not available.";
                return RedirectToAction("Index", "Packages");
            }

            var viewModel = new BookingCreateViewModel
            {
                PackageId = package.Id,
                PackageName = package.Name,
                PackagePrice = package.Price,
                MaxGuests = package.GuestCapacity,
                ImageUrl = package.ImageUrl,
                GuestCount = Math.Min(150, package.GuestCapacity),
                EventDate = DateTime.Today.AddMonths(2)
            };

            return View(viewModel);
        }

        // POST: /Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel model)
        {
            if (User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Administrators cannot submit personal wedding reservations.";
                return RedirectToAction("Bookings", "Admin");
            }

            var package = await _context.WeddingPackages.FindAsync(model.PackageId);
            if (package == null)
            {
                return NotFound();
            }

            if (model.EventDate.Date <= DateTime.Today)
            {
                ModelState.AddModelError("EventDate", "Wedding date must be a future date.");
            }

            if (model.GuestCount > package.GuestCapacity)
            {
                ModelState.AddModelError("GuestCount", $"Guest count cannot exceed the package capacity of {package.GuestCapacity} guests.");
            }

            if (ModelState.IsValid)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdStr, out int userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var random = new Random();
                var bookingNumber = $"WED-{DateTime.UtcNow.Year}-{random.Next(1000, 9999)}";

                var booking = new Booking
                {
                    BookingNumber = bookingNumber,
                    UserId = userId,
                    PackageId = package.Id,
                    EventDate = model.EventDate,
                    GuestCount = model.GuestCount,
                    SpecialRequests = model.SpecialRequests,
                    TotalAmount = package.Price,
                    Status = "Pending",
                    BookingDate = DateTime.UtcNow
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Congratulations! Your wedding booking #{booking.BookingNumber} has been submitted successfully!";
                return RedirectToAction(nameof(Details), new { id = booking.Id });
            }

            model.PackageName = package.Name;
            model.PackagePrice = package.Price;
            model.MaxGuests = package.GuestCapacity;
            model.ImageUrl = package.ImageUrl;
            return View(model);
        }

        // GET: /Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Package)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && (string.IsNullOrEmpty(userIdStr) || booking.UserId != int.Parse(userIdStr)))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View(booking);
        }

        // POST: /Bookings/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && (string.IsNullOrEmpty(userIdStr) || booking.UserId != int.Parse(userIdStr)))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            if (booking.Status == "Completed")
            {
                TempData["ErrorMessage"] = "Cannot cancel an already completed wedding booking.";
                return RedirectToAction(nameof(Details), new { id });
            }

            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Booking #{booking.BookingNumber} has been cancelled.";

            return RedirectToAction(isAdmin ? "Bookings" : "Index", isAdmin ? "Admin" : "Bookings");
        }

        // POST: /Bookings/UpdateStatus (Admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = status;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Booking #{booking.BookingNumber} status updated to '{status}'.";
            }
            return RedirectToAction("Bookings", "Admin");
        }
    }
}
