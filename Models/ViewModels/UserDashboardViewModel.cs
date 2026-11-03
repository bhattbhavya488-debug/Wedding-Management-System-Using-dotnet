namespace WeddingManagementSystem.Models.ViewModels
{
    public class UserDashboardViewModel
    {
        public User User { get; set; } = null!;
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public int TotalBookings => Bookings.Count;
        public int PendingBookings => Bookings.Count(b => b.Status == "Pending");
        public int ConfirmedBookings => Bookings.Count(b => b.Status == "Confirmed");
        public decimal TotalSpent => Bookings.Where(b => b.Status != "Cancelled").Sum(b => b.TotalAmount);
    }
}
