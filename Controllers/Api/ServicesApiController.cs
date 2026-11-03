using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingManagementSystem.Data;
using WeddingManagementSystem.Models;

namespace WeddingManagementSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServicesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ServicesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceItem>>> GetServices()
        {
            return await _context.ServiceItems.Where(s => s.IsActive).ToListAsync();
        }

        // GET: api/ServicesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceItem>> GetService(int id)
        {
            var service = await _context.ServiceItems.FindAsync(id);

            if (service == null || !service.IsActive)
            {
                return NotFound(new { message = $"Service with ID {id} not found." });
            }

            return service;
        }
    }
}
