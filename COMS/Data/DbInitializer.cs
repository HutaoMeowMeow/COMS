using COMS.Models;

namespace COMS.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.Users.Any())
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "System",
                LastName = "Administrator",
                Email = "admin@coms.ph",
                PhoneNumber = "+639171234567",
                Role = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Barangay = "Cebu City",
                Municipality = "Cebu City",
                Province = "Cebu",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var lguUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Juan",
                LastName = "Dela Cruz",
                Email = "lgu@coms.ph",
                PhoneNumber = "+639171234568",
                Role = "LGU",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Lgu@123"),
                Barangay = "Cebu City",
                Municipality = "Cebu City",
                Province = "Cebu",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var barangayUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Maria",
                LastName = "Santos",
                Email = "barangay@coms.ph",
                PhoneNumber = "+639171234569",
                Role = "Barangay",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Brgy@123"),
                Barangay = "Guadalupe",
                Municipality = "Cebu City",
                Province = "Cebu",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var residentUser = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Pedro",
                LastName = "Reyes",
                Email = "resident@coms.ph",
                PhoneNumber = "+639171234570",
                Role = "Resident",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Res@123"),
                Barangay = "Guadalupe",
                Municipality = "Cebu City",
                Province = "Cebu",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(adminUser, lguUser, barangayUser, residentUser);
            await context.SaveChangesAsync();
        }

        if (!context.Canals.Any())
        {
            var canal1 = new Canal
            {
                Id = Guid.NewGuid(),
                Name = "Guadalupe Canal",
                Location = "Along V. Rama Avenue, Guadalupe, Cebu City",
                Barangay = "Guadalupe",
                Municipality = "Cebu City",
                Province = "Cebu",
                Status = "Normal",
                LengthMeters = 850,
                WidthMeters = 4.5,
                DepthMeters = 2.0,
                NormalWaterLevel = 0.8,
                WarningWaterLevel = 1.5,
                CriticalWaterLevel = 2.5,
                CreatedAt = DateTime.UtcNow
            };

            var canal2 = new Canal
            {
                Id = Guid.NewGuid(),
                Name = "Pahina Canal",
                Location = "Pahina District, Cebu City",
                Barangay = "Pahina",
                Municipality = "Cebu City",
                Province = "Cebu",
                Status = "Normal",
                LengthMeters = 620,
                WidthMeters = 3.8,
                DepthMeters = 1.8,
                NormalWaterLevel = 0.7,
                WarningWaterLevel = 1.3,
                CriticalWaterLevel = 2.2,
                CreatedAt = DateTime.UtcNow
            };

            context.Canals.AddRange(canal1, canal2);
            await context.SaveChangesAsync();
        }

        if (!context.Sensors.Any())
        {
            var canal = context.Canals.First();
            var sensor1 = new Sensor
            {
                Id = Guid.NewGuid(),
                SensorCode = "SENSOR-001",
                SensorType = "WaterLevel",
                CanalId = canal.Id,
                LocationDescription = "Upstream section",
                Latitude = 10.2915,
                Longitude = 123.8912,
                CommunicationProtocol = "MQTT",
                Status = "Online",
                CreatedAt = DateTime.UtcNow
            };

            var sensor2 = new Sensor
            {
                Id = Guid.NewGuid(),
                SensorCode = "SENSOR-002",
                SensorType = "FlowRate",
                CanalId = canal.Id,
                LocationDescription = "Midstream section",
                Latitude = 10.2920,
                Longitude = 123.8920,
                CommunicationProtocol = "MQTT",
                Status = "Online",
                CreatedAt = DateTime.UtcNow
            };

            var sensor3 = new Sensor
            {
                Id = Guid.NewGuid(),
                SensorCode = "SENSOR-003",
                SensorType = "Debris",
                CanalId = canal.Id,
                LocationDescription = "Downstream section",
                Latitude = 10.2925,
                Longitude = 123.8928,
                CommunicationProtocol = "LoRaWAN",
                Status = "Online",
                CreatedAt = DateTime.UtcNow
            };

            context.Sensors.AddRange(sensor1, sensor2, sensor3);
            await context.SaveChangesAsync();
        }
    }
}
