using System.ComponentModel.DataAnnotations;

namespace WeddingManagementSystem.Models.ViewModels
{
    public class BookingCreateViewModel
    {
        [Required]
        public int PackageId { get; set; }

        public string? PackageName { get; set; }
        public decimal PackagePrice { get; set; }
        public int MaxGuests { get; set; }
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Please select your wedding date")]
        [DataType(DataType.Date)]
        [Display(Name = "Wedding Date")]
        public DateTime EventDate { get; set; } = DateTime.Today.AddMonths(1);

        [Required(ErrorMessage = "Please specify expected number of guests")]
        [Range(10, 5000, ErrorMessage = "Guest count must be between 10 and 5,000")]
        [Display(Name = "Estimated Guest Count")]
        public int GuestCount { get; set; } = 150;

        [StringLength(500)]
        [Display(Name = "Special Requests / Theme / Venue Preferences")]
        public string? SpecialRequests { get; set; }
    }
}
