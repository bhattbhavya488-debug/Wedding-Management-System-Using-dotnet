namespace WeddingManagementSystem.Models.ViewModels
{
    public class PackageFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public string? SelectedCategory { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }

        public List<string> Categories { get; set; } = new List<string> { "All", "Royal", "Beach", "Destination", "Traditional", "Minimalist" };
        public List<WeddingPackage> Packages { get; set; } = new List<WeddingPackage>();
    }
}
