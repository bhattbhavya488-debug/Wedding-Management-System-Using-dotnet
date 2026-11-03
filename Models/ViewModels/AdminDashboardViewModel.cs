namespace WeddingManagementSystem.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalBookings { get; set; }
        public int PendingBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int TotalPackages { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int UnreadMessages { get; set; }

        public List<Booking> RecentBookings { get; set; } = new List<Booking>();
        public List<WeddingPackage> PopularPackages { get; set; } = new List<WeddingPackage>();
    }
}
