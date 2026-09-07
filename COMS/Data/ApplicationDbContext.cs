using COMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace COMS.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Canal> Canals { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorReading> SensorReadings { get; set; }
    public DbSet<ObstructionAlert> ObstructionAlerts { get; set; }
    public DbSet<CommunityReport> CommunityReports { get; set; }
    public DbSet<FloodRiskAssessment> FloodRiskAssessments { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                    property.SetValueConverter(dateTimeConverter);
                if (property.ClrType == typeof(DateTime?))
                    property.SetValueConverter(nullableDateTimeConverter);
            }
        }

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<Sensor>()
            .HasIndex(s => s.SensorCode)
            .IsUnique();

        modelBuilder.Entity<Sensor>()
            .HasOne(s => s.Canal)
            .WithMany(c => c.Sensors)
            .HasForeignKey(s => s.CanalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SensorReading>()
            .HasOne(sr => sr.Sensor)
            .WithMany(s => s.SensorReadings)
            .HasForeignKey(sr => sr.SensorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SensorReading>()
            .HasOne(sr => sr.Canal)
            .WithMany(c => c.SensorReadings)
            .HasForeignKey(sr => sr.CanalId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ObstructionAlert>()
            .HasOne(oa => oa.Canal)
            .WithMany(c => c.ObstructionAlerts)
            .HasForeignKey(oa => oa.CanalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CommunityReport>()
            .HasOne(cr => cr.Canal)
            .WithMany(c => c.CommunityReports)
            .HasForeignKey(cr => cr.CanalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CommunityReport>()
            .HasOne(cr => cr.ReportedByUser)
            .WithMany(u => u.CommunityReports)
            .HasForeignKey(cr => cr.ReportedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FloodRiskAssessment>()
            .HasOne(fra => fra.Canal)
            .WithMany(c => c.FloodRiskAssessments)
            .HasForeignKey(fra => fra.CanalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.ObstructionAlert)
            .WithMany(oa => oa.Notifications)
            .HasForeignKey(n => n.ObstructionAlertId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
