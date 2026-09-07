using COMS.Data;
using COMS.Models;
using Google.Cloud.Firestore;

namespace COMS.Data;

public static class FirestoreDbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var firestoreDb = scope.ServiceProvider.GetRequiredService<FirestoreDb>();
        
        var userRepo = new FirestoreRepository<User>(firestoreDb, "users");
        var canalRepo = new FirestoreRepository<Canal>(firestoreDb, "canals");
        var sensorRepo = new FirestoreRepository<Sensor>(firestoreDb, "sensors");
        var announcementRepo = new FirestoreRepository<Announcement>(firestoreDb, "announcements");

        var existingUsers = await userRepo.GetAllAsync();
        if (!existingUsers.Any())
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

            await userRepo.CreateAsync(adminUser);
            await userRepo.CreateAsync(lguUser);
            await userRepo.CreateAsync(barangayUser);
            await userRepo.CreateAsync(residentUser);
        }

        var existingCanals = await canalRepo.GetAllAsync();
        if (!existingCanals.Any())
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

            await canalRepo.CreateAsync(canal1);
            await canalRepo.CreateAsync(canal2);

            var sensor1 = new Sensor
            {
                Id = Guid.NewGuid(),
                SensorCode = "SENSOR-001",
                SensorType = "WaterLevel",
                CanalId = canal1.Id,
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
                CanalId = canal1.Id,
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
                CanalId = canal1.Id,
                LocationDescription = "Downstream section",
                Latitude = 10.2925,
                Longitude = 123.8928,
                CommunicationProtocol = "LoRaWAN",
                Status = "Online",
                CreatedAt = DateTime.UtcNow
            };

            await sensorRepo.CreateAsync(sensor1);
            await sensorRepo.CreateAsync(sensor2);
            await sensorRepo.CreateAsync(sensor3);
        }

        var existingAnnouncements = await announcementRepo.GetAllAsync();
        if (!existingAnnouncements.Any())
        {
            var adminUser = (await userRepo.QueryAsync(q => q.WhereEqualTo("Email", "admin@coms.ph").Limit(1))).FirstOrDefault();
            var lguUser = (await userRepo.QueryAsync(q => q.WhereEqualTo("Email", "lgu@coms.ph").Limit(1))).FirstOrDefault();
            var barangayUser = (await userRepo.QueryAsync(q => q.WhereEqualTo("Email", "barangay@coms.ph").Limit(1))).FirstOrDefault();

            if (adminUser != null)
            {
                await announcementRepo.CreateAsync(new Announcement
                {
                    Id = Guid.NewGuid(),
                    Title = "Welcome to COMS",
                    Content = "The Canal Obstruction Monitoring System is now live. Report issues and stay updated with community announcements.",
                    ImageUrl = null,
                    PostedByUserId = adminUser.Id,
                    PostedByRole = "Admin",
                    Barangay = adminUser.Barangay,
                    Municipality = adminUser.Municipality,
                    CreatedAt = DateTime.UtcNow
                });
            }

            if (lguUser != null)
            {
                await announcementRepo.CreateAsync(new Announcement
                {
                    Id = Guid.NewGuid(),
                    Title = "Weekly Canal Inspection Schedule",
                    Content = "LGU will conduct weekly inspections of all major canals every Monday. Residents are advised to avoid disposal of waste near canal areas.",
                    ImageUrl = null,
                    PostedByUserId = lguUser.Id,
                    PostedByRole = "LGU",
                    Barangay = lguUser.Barangay,
                    Municipality = lguUser.Municipality,
                    CreatedAt = DateTime.UtcNow.AddHours(-2)
                });
            }

            if (barangayUser != null)
            {
                await announcementRepo.CreateAsync(new Announcement
                {
                    Id = Guid.NewGuid(),
                    Title = "Community Cleanup Drive",
                    Content = "Join the Barangay Guadalupe cleanup drive this Saturday at 6:00 AM. Meet at the Barangay Hall.",
                    ImageUrl = null,
                    PostedByUserId = barangayUser.Id,
                    PostedByRole = "Barangay",
                    Barangay = barangayUser.Barangay,
                    Municipality = barangayUser.Municipality,
                    CreatedAt = DateTime.UtcNow.AddHours(-5)
                });
            }
        }
    }
}
