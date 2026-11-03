using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingManagementSystem.Models
{
    public class ServiceItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Service Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // Catering, Decor, Photography, Entertainment, Venue, Bridal

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Starting Price (₹)")]
        public decimal StartingPrice { get; set; }

        [StringLength(50)]
        [Display(Name = "Icon Class")]
        public string IconClass { get; set; } = "bi-gem";

        public bool IsActive { get; set; } = true;
    }
}
