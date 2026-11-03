using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingManagementSystem.Data;
using WeddingManagementSystem.Models;

namespace WeddingManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BookingsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBookings([FromQuery] string? status)
        {
            var query = _context.Bookings
                .Include(b => b.Package)
                .Include(b => b.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(b => b.Status == status);
            }

            var bookings = await query
                .OrderByDescending(b => b.BookingDate)
                .Select(b => new
                {
                    b.Id,
                    b.BookingNumber,
                    CustomerName = b.User != null ? b.User.FullName : "Unknown",
                    CustomerEmail = b.User != null ? b.User.Email : "Unknown",
                    PackageName = b.Package != null ? b.Package.Name : "Unknown",
                    b.EventDate,
                    b.GuestCount,
                    b.TotalAmount,
                    b.Status,
                    b.BookingDate
                })
                .ToListAsync();

            return Ok(bookings);
        }

        // GET: api/BookingsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Package)
                .Include(b => b.User)
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    b.Id,
                    b.BookingNumber,
                    Customer = new
                    {
                        b.User!.Id,
                        b.User.FullName,
                        b.User.Email,
                        b.User.PhoneNumber
                    },
                    Package = new
                    {
                        b.Package!.Id,
                        b.Package.Name,
                        b.Package.Category,
                        b.Package.Price,
                        b.Package.GuestCapacity
                    },
                    b.EventDate,
                    b.GuestCount,
                    b.SpecialRequests,
                    b.TotalAmount,
                    b.Status,
                    b.BookingDate
                })
                .FirstOrDefaultAsync();

            if (booking == null)
            {
                return NotFound(new { message = $"Booking with ID {id} not found." });
            }

            return Ok(booking);
        }

        // PATCH: api/BookingsApi/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] StatusUpdateDto dto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = $"Booking with ID {id} not found." });
            }

            var allowedStatuses = new[] { "Pending", "Confirmed", "Completed", "Cancelled" };
            if (!allowedStatuses.Contains(dto.Status))
            {
                return BadRequest(new { message = "Invalid status. Allowed values: Pending, Confirmed, Completed, Cancelled." });
            }

            booking.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Status updated successfully.", bookingId = booking.Id, newStatus = booking.Status });
        }
    }

    public class StatusUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
