using Microsoft.EntityFrameworkCore;
using ParkingSystem.Models;

namespace ParkingSystem.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            // Only seed if the database is empty
            if (await context.ParkingLots.AnyAsync())
            {
                return;
            }

            // Add users
            var user1 = new User
            {
                FullName = "John Doe",
                Email = "john@example.com",
                PasswordHash = "hashed_password_1"
            };

            var user2 = new User
            {
                FullName = "Jane Smith",
                Email = "jane@example.com",
                PasswordHash = "hashed_password_2"
            };

            await context.Users.AddRangeAsync(user1, user2);
            await context.SaveChangesAsync();

            // Add cars
            var car1 = new Car
            {
                LicensePlate = "ABC123",
                Model = "Toyota Camry",
                Color = "Blue"
            };

            var car2 = new Car
            {
                LicensePlate = "XYZ789",
                Model = "Honda Civic",
                Color = "Red"
            };

            await context.Cars.AddRangeAsync(car1, car2);
            await context.SaveChangesAsync();

            // Associate cars with users (many-to-many)
            user1.Cars.Add(car1);
            user2.Cars.Add(car2);
            await context.SaveChangesAsync();

            // Add parking lots
            var parkingLot1 = new ParkingLot
            {
                Name = "Downtown Parking",
                Address = "123 Main St, Downtown",
                Description = "Central parking facility"
            };

            var parkingLot2 = new ParkingLot
            {
                Name = "Airport Parking",
                Address = "456 Airport Rd, Airport Area",
                Description = "Long-term airport parking"
            };

            await context.ParkingLots.AddRangeAsync(parkingLot1, parkingLot2);
            await context.SaveChangesAsync();

            // Add spot types
            var standardType = new SpotType { Name = "Standard" };
            var compactType = new SpotType { Name = "Compact" };
            var handicapType = new SpotType { Name = "Handicap" };
            var electricType = new SpotType { Name = "Electric Vehicle" };

            await context.SpotTypes.AddRangeAsync(standardType, compactType, handicapType, electricType);
            await context.SaveChangesAsync();

            // Add fees for spot types
            var standardFee = new Fee { Name = "Standard Fee", DailyMaxCap = 20.00m, SpotTypeId = standardType.Id };
            var compactFee = new Fee { Name = "Compact Fee", DailyMaxCap = 15.00m, SpotTypeId = compactType.Id };
            var handicapFee = new Fee { Name = "Handicap Fee", DailyMaxCap = 10.00m, SpotTypeId = handicapType.Id };
            var electricFee = new Fee { Name = "Electric Fee", DailyMaxCap = 25.00m, SpotTypeId = electricType.Id };

            await context.Fees.AddRangeAsync(standardFee, compactFee, handicapFee, electricFee);
            await context.SaveChangesAsync();

            // Add fee rules
            var standardHourlyRule = new FeeRule
            {
                Priority = 1,
                ChargeType = ChargeType.Hourly,
                ChargeAmount = 2.00m,
                FeeId = standardFee.Id
            };

            var compactHourlyRule = new FeeRule
            {
                Priority = 1,
                ChargeType = ChargeType.Hourly,
                ChargeAmount = 1.50m,
                FeeId = compactFee.Id
            };

            await context.FeeRules.AddRangeAsync(standardHourlyRule, compactHourlyRule);
            await context.SaveChangesAsync();

            // Add parking spots
            var spots = new List<ParkingSpot>
            {
                new ParkingSpot { SpotName = "A1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot1.Id, SpotTypeId = standardType.Id },
                new ParkingSpot { SpotName = "A2", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot1.Id, SpotTypeId = standardType.Id },
                new ParkingSpot { SpotName = "B1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot1.Id, SpotTypeId = compactType.Id },
                new ParkingSpot { SpotName = "B2", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot1.Id, SpotTypeId = compactType.Id },
                new ParkingSpot { SpotName = "C1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot1.Id, SpotTypeId = handicapType.Id },
                new ParkingSpot { SpotName = "D1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot1.Id, SpotTypeId = electricType.Id },

                new ParkingSpot { SpotName = "A1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot2.Id, SpotTypeId = standardType.Id },
                new ParkingSpot { SpotName = "A2", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot2.Id, SpotTypeId = standardType.Id },
                new ParkingSpot { SpotName = "B1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot2.Id, SpotTypeId = compactType.Id },
                new ParkingSpot { SpotName = "C1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot2.Id, SpotTypeId = handicapType.Id }
            };

            await context.ParkingSpots.AddRangeAsync(spots);
            await context.SaveChangesAsync();

            // Add a sample reservation
            var reservation = new Reservation
            {
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(2),
                Status = ReservationStatus.Active,
                UserId = user1.Id,
                CarId = car1.Id,
                ParkingSpotId = spots[0].Id // A1 in Downtown Parking
            };

            await context.Reservations.AddAsync(reservation);

            // Update spot status to Reserved
            spots[0].Status = SpotStatus.Reserved;

            await context.SaveChangesAsync();
        }
    }
}