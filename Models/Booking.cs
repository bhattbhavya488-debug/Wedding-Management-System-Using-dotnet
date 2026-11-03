using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingManagementSystem.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Booking #")]
        public string BookingNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Customer")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [Required]
        [Display(Name = "Wedding Package")]
        public int PackageId { get; set; }

        [ForeignKey("PackageId")]
        public virtual WeddingPackage? Package { get; set; }

        [Required(ErrorMessage = "Event Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Wedding Date")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Estimated guests count is required")]
        [Range(10, 5000, ErrorMessage = "Guest count must be between 10 and 5,000")]
        [Display(Name = "Number of Guests")]
        public int GuestCount { get; set; }

        [StringLength(500)]
        [Display(Name = "Special Requests / Notes")]
        public string? SpecialRequests { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Amount (₹)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Completed, Cancelled

        [Display(Name = "Booking Placed On")]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
    }
}
