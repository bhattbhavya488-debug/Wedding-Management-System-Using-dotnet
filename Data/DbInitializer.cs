using WeddingManagementSystem.Models;
using WeddingManagementSystem.Services;

namespace WeddingManagementSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Seed Users if empty
            if (!context.Users.Any())
            {
                var admin = new User
                {
                    FullName = "System Administrator",
                    Email = "admin@wedding.com",
                    PasswordHash = PasswordHasher.HashPassword("Admin@123"),
                    PhoneNumber = "9876543210",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                };

                var demoCustomer = new User
                {
                    FullName = "Riya Sharma",
                    Email = "user@wedding.com",
                    PasswordHash = PasswordHasher.HashPassword("User@123"),
                    PhoneNumber = "9123456780",
                    Role = "Customer",
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                };

                var customer2 = new User
                {
                    FullName = "Arjun Patel",
                    Email = "arjun@example.com",
                    PasswordHash = PasswordHasher.HashPassword("User@123"),
                    PhoneNumber = "9825012345",
                    Role = "Customer",
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                };

                context.Users.AddRange(admin, demoCustomer, customer2);
                context.SaveChanges();
            }

            // Seed Packages if empty
            if (!context.WeddingPackages.Any())
            {
                var packages = new List<WeddingPackage>
                {
                    new WeddingPackage
                    {
                        Name = "Royal Heritage Palace Wedding",
                        Category = "Royal",
                        Description = "Experience a fairytale wedding inspired by royal Indian traditions. Includes majestic floral arches, crystal chandeliers, royal procession, and 5-star royal banquet spread.",
                        Price = 850000m,
                        GuestCapacity = 500,
                        ImageUrl = "https://images.unsplash.com/photo-1519741497674-611481863552?auto=format&fit=crop&w=1200&q=80",
                        Features = "Royal Mandap with Exotic Fresh Flowers; 5-Star Multi-Cuisine Royal Buffet (60+ Items); Cinematic Drone & 4K Wedding Film; Luxury Bridal & Groom Vanity Lounge; Live Shehnai, Dhol & Premium DJ Night; Dedicated Royal Butler & Guest Concierge",
                        IsActive = true
                    },
                    new WeddingPackage
                    {
                        Name = "Sunset Beachfront Serenade",
                        Category = "Beach",
                        Description = "Breathtaking seaside wedding ceremony with golden sand aisle, breezy pastel floral canopies, sunset vow exchange, and fresh seafood & continental dining under fairy lights.",
                        Price = 525000m,
                        GuestCapacity = 250,
                        ImageUrl = "https://images.unsplash.com/photo-1544078751-58fee2d8a03b?auto=format&fit=crop&w=1200&q=80",
                        Features = "Beachside Bohemian Canopy & Floral Arch; Oceanfront Sunset Ceremony Seating; Fresh Coastal & International Dining; Acoustic Live Band & Beach Bonfire Afterparty; Candid Beach Photography & Golden Hour Shoot; Customized Tropical Welcome Cocktails",
                        IsActive = true
                    },
                    new WeddingPackage
                    {
                        Name = "Destination Extravaganza Udaipur",
                        Category = "Destination",
                        Description = "An unforgettable 2-day destination celebration at an iconic lake palace. Complete with Mehendi by the poolside, Sangeet in a mirrored ballroom, and grand Varmala on a heritage boat.",
                        Price = 1200000m,
                        GuestCapacity = 600,
                        ImageUrl = "https://images.unsplash.com/photo-1583939003579-730e3918a45a?auto=format&fit=crop&w=1200&q=80",
                        Features = "2-Day Exclusive Venue Access & Hospitality; Lakefront Boat Arrival for Groom & Bride; Grand Sangeet Stage with LED Video Walls; Michelin-Style Global Buffet Stations; Celebrity DJ & Choreographed Sound/Light Show; Complete Luxury Transport & Airport Shuttle",
                        IsActive = true
                    },
                    new WeddingPackage
                    {
                        Name = "Traditional Vedic Vivah",
                        Category = "Traditional",
                        Description = "Sacred and soulful traditional wedding ceremony following ancient Vedic rituals. Featuring marigold & jasmine floral theme, temple bells, pure vegetarian traditional feast, and authentic brass decor.",
                        Price = 350000m,
                        GuestCapacity = 350,
                        ImageUrl = "https://images.unsplash.com/photo-1606800052052-a08af7148866?auto=format&fit=crop&w=1200&q=80",
                        Features = "Sacred Havan Kund & Authentic Brass Temple Mandap; Fresh Marigold, Jasmine & Lotus Floral Decor; Traditional Pure-Ghee Satvik Feast on Banana Leaf; Classical Carnatic / Shehnai Instrumentalist Group; Complete Puja Samagri & Head Priest Coordination; Traditional Photography & Family Album",
                        IsActive = true
                    },
                    new WeddingPackage
                    {
                        Name = "Bohemian Garden Blossom",
                        Category = "Minimalist",
                        Description = "An intimate, eco-conscious celebration surrounded by lush greenery, fairy lights, wooden rustic tables, acoustic music, and curated farm-to-table culinary experiences.",
                        Price = 220000m,
                        GuestCapacity = 150,
                        ImageUrl = "https://images.unsplash.com/photo-1511285560929-80b456fea0bc?auto=format&fit=crop&w=1200&q=80",
                        Features = "Rustic Wooden Arches & Wildflower Bouquets; Fairy-Lit Canopy over Open Garden Lawn; Organic Farm-to-Table Artisanal Buffet; Solo Acoustic Guitarist & Vocalist; Polaroids & Candid Film Photography; Eco-friendly Seed Paper Invitations & Favors",
                        IsActive = true
                    },
                    new WeddingPackage
                    {
                        Name = "Grand Golden Banquet Gala",
                        Category = "Royal",
                        Description = "A glamorous indoor celebration inside an opulent ballroom adorned with gold leaf pillars, cascading orchids, mirror dance floor, and an extravagant international buffet.",
                        Price = 675000m,
                        GuestCapacity = 450,
                        ImageUrl = "https://images.unsplash.com/photo-1465495976277-4387d4b0b4c6?auto=format&fit=crop&w=1200&q=80",
                        Features = "Mirror Walkway with 3D Holographic Entry; Crystal & Golden Candelabra Table Settings; Gourmet 5-Course Plated & Live Food Counters; High-Energy Bollywood DJ & Laser Lighting; Pre-wedding Video Projection on Giant Curved Screen; Dedicated Bride & Groom Concierge",
                        IsActive = true
                    }
                };

                context.WeddingPackages.AddRange(packages);
                context.SaveChanges();
            }

            // Seed Services if empty
            if (!context.ServiceItems.Any())
            {
                var services = new List<ServiceItem>
                {
                    new ServiceItem
                    {
                        Name = "Floral & Mandap Styling",
                        Category = "Decoration",
                        Description = "Exquisite floral installations, royal mandap designs, ambient fairy lights, and themed stage backdrops tailored to your vision.",
                        StartingPrice = 45000m,
                        IconClass = "bi-flower1",
                        IsActive = true
                    },
                    new ServiceItem
                    {
                        Name = "Gourmet Catering & Live Counters",
                        Category = "Catering",
                        Description = "Multi-cuisine culinary feasts spanning North Indian, Gujarati, Pan-Asian, Italian, and live dessert counters prepared by master chefs.",
                        StartingPrice = 85000m,
                        IconClass = "bi-cup-hot",
                        IsActive = true
                    },
                    new ServiceItem
                    {
                        Name = "Cinematic Photography & Drone",
                        Category = "Photography",
                        Description = "4K cinematic wedding teaser films, candid portraits, aerial drone captures, and handcrafted luxury leather-bound albums.",
                        StartingPrice = 60000m,
                        IconClass = "bi-camera-reels",
                        IsActive = true
                    },
                    new ServiceItem
                    {
                        Name = "Live Band, Shehnai & Celebrity DJ",
                        Category = "Entertainment",
                        Description = "Soulful traditional Shehnai for morning rituals, acoustic sunset melodies, and high-energy club DJ sets for Sangeet & Reception.",
                        StartingPrice = 35000m,
                        IconClass = "bi-music-note-beamed",
                        IsActive = true
                    },
                    new ServiceItem
                    {
                        Name = "Bridal & Groom Makeover",
                        Category = "Bridal",
                        Description = "Celebrity bridal makeup artists, HD airbrush makeover, hairstyling, saree draping, and grooming packages.",
                        StartingPrice = 25000m,
                        IconClass = "bi-stars",
                        IsActive = true
                    },
                    new ServiceItem
                    {
                        Name = "Heritage Venues & Destination Lawns",
                        Category = "Venue",
                        Description = "Exclusive tie-ups with premium 5-star heritage hotels, royal palaces, beachfront resorts, and open-air luxury lawns.",
                        StartingPrice = 150000m,
                        IconClass = "bi-building",
                        IsActive = true
                    }
                };

                context.ServiceItems.AddRange(services);
                context.SaveChanges();
            }

            // Seed Bookings if empty
            if (!context.Bookings.Any())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == "user@wedding.com");
                var pkg1 = context.WeddingPackages.FirstOrDefault(p => p.Category == "Royal");
                var pkg2 = context.WeddingPackages.FirstOrDefault(p => p.Category == "Beach");

                if (user != null && pkg1 != null && pkg2 != null)
                {
                    var bookings = new List<Booking>
                    {
                        new Booking
                        {
                            BookingNumber = "WED-2026-1001",
                            UserId = user.Id,
                            PackageId = pkg1.Id,
                            EventDate = DateTime.Today.AddDays(45),
                            GuestCount = 400,
                            SpecialRequests = "Red & Gold traditional theme with Rajasthani welcome folk dancers.",
                            TotalAmount = pkg1.Price,
                            Status = "Confirmed",
                            BookingDate = DateTime.UtcNow.AddDays(-10)
                        },
                        new Booking
                        {
                            BookingNumber = "WED-2026-1002",
                            UserId = user.Id,
                            PackageId = pkg2.Id,
                            EventDate = DateTime.Today.AddDays(90),
                            GuestCount = 200,
                            SpecialRequests = "Beach sunset vows with acoustic guitar and vegan live food station.",
                            TotalAmount = pkg2.Price,
                            Status = "Pending",
                            BookingDate = DateTime.UtcNow.AddDays(-2)
                        }
                    };

                    context.Bookings.AddRange(bookings);
                    context.SaveChanges();
                }
            }
        }
    }
}
