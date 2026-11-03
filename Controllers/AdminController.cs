using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingManagementSystem.Data;
using WeddingManagementSystem.Models.ViewModels;

namespace WeddingManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Package)
                .Include(b => b.User)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            var viewModel = new AdminDashboardViewModel
            {
                TotalBookings = bookings.Count,
                PendingBookings = bookings.Count(b => b.Status == "Pending"),
                ConfirmedBookings = bookings.Count(b => b.Status == "Confirmed"),
                CompletedBookings = bookings.Count(b => b.Status == "Completed"),
                TotalPackages = await _context.WeddingPackages.CountAsync(p => p.IsActive),
                TotalCustomers = await _context.Users.CountAsync(u => u.Role == "Customer"),
                TotalRevenue = bookings.Where(b => b.Status != "Cancelled").Sum(b => b.TotalAmount),
                UnreadMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead),
                RecentBookings = bookings.Take(5).ToList(),
                PopularPackages = await _context.WeddingPackages.Where(p => p.IsActive).Take(4).ToListAsync()
            };

            return View(viewModel);
        }

        // GET: /Admin/Bookings
        public async Task<IActionResult> Bookings(string? status)
        {
            var query = _context.Bookings
                .Include(b => b.Package)
                .Include(b => b.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(b => b.Status == status);
            }

            ViewBag.CurrentStatus = status ?? "All";
            var bookings = await query.OrderByDescending(b => b.BookingDate).ToListAsync();
            return View(bookings);
        }

        // GET: /Admin/Customers
        public async Task<IActionResult> Customers()
        {
            var customers = await _context.Users
                .Where(u => u.Role == "Customer")
                .Include(u => u.Bookings)
                    .ThenInclude(b => b.Package)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(customers);
        }

        // GET: /Admin/Messages
        public async Task<IActionResult> Messages()
        {
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.SubmittedAt)
                .ToListAsync();

            return View(messages);
        }

        // POST: /Admin/MarkMessageAsRead/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            var message = await _context.ContactMessages.FindAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Message marked as read.";
            }
            return RedirectToAction(nameof(Messages));
        }
    }
}
