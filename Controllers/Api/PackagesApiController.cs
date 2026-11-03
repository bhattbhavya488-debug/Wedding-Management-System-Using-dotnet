using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingManagementSystem.Data;
using WeddingManagementSystem.Models;

namespace WeddingManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PackagesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PackagesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeddingPackage>>> GetPackages([FromQuery] string? category)
        {
            var query = _context.WeddingPackages.Where(p => p.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                query = query.Where(p => p.Category == category);
            }

            return await query.ToListAsync();
        }

        // GET: api/PackagesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeddingPackage>> GetPackage(int id)
        {
            var package = await _context.WeddingPackages.FindAsync(id);

            if (package == null || !package.IsActive)
            {
                return NotFound(new { message = $"Wedding package with ID {id} not found." });
            }

            return package;
        }

        // POST: api/PackagesApi
        [HttpPost]
        public async Task<ActionResult<WeddingPackage>> CreatePackage([FromBody] WeddingPackage package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            package.CreatedAt = DateTime.UtcNow;
            package.IsActive = true;
            _context.WeddingPackages.Add(package);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPackage), new { id = package.Id }, package);
        }

        // PUT: api/PackagesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, [FromBody] WeddingPackage package)
        {
            if (id != package.Id)
            {
                return BadRequest(new { message = "Package ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(package).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.WeddingPackages.Any(e => e.Id == id))
                {
                    return NotFound(new { message = $"Package with ID {id} not found." });
                }
                throw;
            }

            return Ok(new { message = "Package updated successfully.", package });
        }

        // DELETE: api/PackagesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var package = await _context.WeddingPackages.FindAsync(id);
            if (package == null)
            {
                return NotFound(new { message = $"Package with ID {id} not found." });
            }

            package.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Package '{package.Name}' deactivated successfully." });
        }
    }
}
