using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingManagementSystem.Data;
using WeddingManagementSystem.Models;
using WeddingManagementSystem.Models.ViewModels;

namespace WeddingManagementSystem.Controllers
{
    public class PackagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PackagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Packages
        public async Task<IActionResult> Index(string? searchTerm, string? selectedCategory, decimal? maxPrice, string? sortBy)
        {
            var query = _context.WeddingPackages.Where(p => p.IsActive).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term) || p.Description.ToLower().Contains(term) || p.Features.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(selectedCategory) && selectedCategory != "All")
            {
                query = query.Where(p => p.Category == selectedCategory);
            }

            if (maxPrice.HasValue && maxPrice.Value > 0)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "capacity_desc" => query.OrderByDescending(p => p.GuestCapacity),
                _ => query.OrderBy(p => p.Name)
            };

            var viewModel = new PackageFilterViewModel
            {
                SearchTerm = searchTerm,
                SelectedCategory = selectedCategory ?? "All",
                MaxPrice = maxPrice,
                SortBy = sortBy,
                Packages = await query.ToListAsync()
            };

            return View(viewModel);
        }

        // GET: /Packages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var package = await _context.WeddingPackages
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (package == null) return NotFound();

            return View(package);
        }

        // GET: /Packages/Create (Admin Only)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new WeddingPackage());
        }

        // POST: /Packages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(WeddingPackage package)
        {
            if (ModelState.IsValid)
            {
                package.CreatedAt = DateTime.UtcNow;
                _context.WeddingPackages.Add(package);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Package '{package.Name}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(package);
        }

        // GET: /Packages/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var package = await _context.WeddingPackages.FindAsync(id);
            if (package == null) return NotFound();

            return View(package);
        }

        // POST: /Packages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, WeddingPackage package)
        {
            if (id != package.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingPackage = await _context.WeddingPackages.FindAsync(id);
                if (existingPackage == null)
                {
                    return NotFound();
                }

                existingPackage.Name = package.Name.Trim();
                existingPackage.Category = package.Category;
                existingPackage.Price = package.Price;
                existingPackage.GuestCapacity = package.GuestCapacity;
                existingPackage.ImageUrl = package.ImageUrl;
                existingPackage.Description = package.Description;
                existingPackage.Features = package.Features;
                existingPackage.IsActive = package.IsActive;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Package '{existingPackage.Name}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(package);
        }

        // POST: /Packages/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var package = await _context.WeddingPackages.FindAsync(id);
            if (package != null)
            {
                // Soft delete to preserve booking references
                package.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Package '{package.Name}' removed from catalog.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
