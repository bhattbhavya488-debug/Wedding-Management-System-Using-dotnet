using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingManagementSystem.Models
{
    public class WeddingPackage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Package name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Package name must be between 3 and 100 characters")]
        [Display(Name = "Package Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [StringLength(50)]
        public string Category { get; set; } = "Traditional"; // Royal, Beach, Destination, Traditional, Minimalist

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be at least 10 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(1000, 10000000, ErrorMessage = "Price must be between ₹1,000 and ₹1,00,00,000")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price (₹)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Guest capacity is required")]
        [Range(10, 5000, ErrorMessage = "Guest capacity must be between 10 and 5,000")]
        [Display(Name = "Guest Capacity")]
        public int GuestCapacity { get; set; }

        [Display(Name = "Image URL")]
        [StringLength(500)]
        public string ImageUrl { get; set; } = "https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=800&q=80";

        [Display(Name = "Features (Semicolon separated)")]
        [StringLength(1000)]
        public string Features { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
