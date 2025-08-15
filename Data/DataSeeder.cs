using Microsoft.EntityFrameworkCore;
using ParkingSystem.Models;

namespace ParkingSystem.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            if (await context.ParkingLots.AnyAsync())
            {
                return;
            }

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

            user1.Cars.Add(car1);
            user2.Cars.Add(car2);
            await context.SaveChangesAsync();

            var parkingLot1 = new ParkingLot
            {
                Name = "Downtown Parking",
                Address = "123 Main St, Downtown",
                Description = "Central parking facility with multiple levels. Close to shops and restaurants.",
                OperatingHours = "Mon-Sat 7am-11pm",
                Latitude = 40.7128m,
                Longitude = -74.0060m
            };

            var parkingLot2 = new ParkingLot
            {
                Name = "Airport Parking",
                Address = "456 Airport Rd, Airport Area",
                Description = "Long-term and short-term airport parking with shuttle service.",
                OperatingHours = "24/7",
                Latitude = 34.0522m,
                Longitude = -118.2437m
            };

            await context.ParkingLots.AddRangeAsync(parkingLot1, parkingLot2);
            await context.SaveChangesAsync();


            var standardType = new SpotType { Name = "Standard", Description = "Vaga de tamanho padrão para a maioria dos veículos." };
            var compactType = new SpotType { Name = "Compact", Description = "Vaga econômica para veículos compactos." };
            var handicapType = new SpotType { Name = "Handicap", Description = "Vaga acessível para pessoas com deficiência." };
            var electricType = new SpotType { Name = "Electric Vehicle", Description = "Vaga com ponto de recarga para veículos elétricos." };

            await context.SpotTypes.AddRangeAsync(standardType, compactType, handicapType, electricType);
            await context.SaveChangesAsync();


            var standardFee = new Fee { Name = "Standard Fee", DailyMaxCap = 25.00m, SpotTypeId = standardType.Id };
            var compactFee = new Fee { Name = "Compact Fee", DailyMaxCap = 20.00m, SpotTypeId = compactType.Id };
            var handicapFee = new Fee { Name = "Handicap Fee", DailyMaxCap = 15.00m, SpotTypeId = handicapType.Id };
            var electricFee = new Fee { Name = "Electric Fee", DailyMaxCap = 30.00m, SpotTypeId = electricType.Id };

            await context.Fees.AddRangeAsync(standardFee, compactFee, handicapFee, electricFee);
            await context.SaveChangesAsync();


            var rules = new List<FeeRule>
    {
        new FeeRule { Priority = 1, ChargeType = ChargeType.Hourly, ChargeAmount = 3.00m, FeeId = standardFee.Id },
        new FeeRule { Priority = 1, ChargeType = ChargeType.Hourly, ChargeAmount = 2.50m, FeeId = compactFee.Id },
        new FeeRule { Priority = 1, ChargeType = ChargeType.Hourly, ChargeAmount = 2.00m, FeeId = handicapFee.Id },
        new FeeRule { Priority = 1, ChargeType = ChargeType.Hourly, ChargeAmount = 3.50m, FeeId = electricFee.Id }
    };
            await context.FeeRules.AddRangeAsync(rules);
            await context.SaveChangesAsync();


            var spots = new List<ParkingSpot>
            {
                new ParkingSpot { SpotName = "A1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot1.Id, SpotTypeId = standardType.Id },
                new ParkingSpot { SpotName = "A2", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot1.Id, SpotTypeId = standardType.Id },
                new ParkingSpot { SpotName = "B1", FloorLevel = 1, Status = SpotStatus.Occupied, ParkingLotId = parkingLot1.Id, SpotTypeId = compactType.Id },
                new ParkingSpot { SpotName = "B2", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot1.Id, SpotTypeId = compactType.Id },
                new ParkingSpot { SpotName = "C1", FloorLevel = 1, Status = SpotStatus.Reserved, ParkingLotId = parkingLot1.Id, SpotTypeId = handicapType.Id },
                new ParkingSpot { SpotName = "D1", FloorLevel = 1, Status = SpotStatus.Occupied, ParkingLotId = parkingLot1.Id, SpotTypeId = electricType.Id },

                new ParkingSpot { SpotName = "A1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot2.Id, SpotTypeId = standardType.Id },
                new ParkingSpot { SpotName = "A2", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot2.Id, SpotTypeId = standardType.Id },
                new ParkingSpot { SpotName = "B1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot2.Id, SpotTypeId = compactType.Id },
                new ParkingSpot { SpotName = "C1", FloorLevel = 1, Status = SpotStatus.Available, ParkingLotId = parkingLot2.Id, SpotTypeId = handicapType.Id }
            };

            await context.ParkingSpots.AddRangeAsync(spots);
            await context.SaveChangesAsync();


            var reservation = new Reservation
            {
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(1).AddHours(2),
                Status = ReservationStatus.Active,
                UserId = user1.Id,
                CarId = car1.Id,
                ParkingSpotId = spots[0].Id
            };

            await context.Reservations.AddAsync(reservation);


            spots[0].Status = SpotStatus.Reserved;

            await context.SaveChangesAsync();
        }
    }
}