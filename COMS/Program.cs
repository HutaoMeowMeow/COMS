using COMS.Data;
using COMS.Hubs;
<<<<<<< HEAD
using COMS.Models;
using COMS.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
=======
using COMS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();

<<<<<<< HEAD
// Firebase
var firebaseProjectId = builder.Configuration["Firebase:ProjectId"] ?? "coms-eldnet";
var firebaseServiceAccountPath = builder.Configuration["Firebase:ServiceAccountPath"] ?? "firebase-service-account.json";

if (File.Exists(firebaseServiceAccountPath))
{
    var credential = GoogleCredential.FromFile(firebaseServiceAccountPath);
    var firestoreDb = new FirestoreDbBuilder
    {
        ProjectId = firebaseProjectId,
        Credential = credential
    }.Build();

    builder.Services.AddSingleton(firestoreDb);

    builder.Services.AddScoped<IFirestoreRepository<User>>(sp =>
        new FirestoreRepository<User>(sp.GetRequiredService<FirestoreDb>(), "users"));
    builder.Services.AddScoped<IFirestoreRepository<Canal>>(sp =>
        new FirestoreRepository<Canal>(sp.GetRequiredService<FirestoreDb>(), "canals"));
    builder.Services.AddScoped<IFirestoreRepository<Sensor>>(sp =>
        new FirestoreRepository<Sensor>(sp.GetRequiredService<FirestoreDb>(), "sensors"));
    builder.Services.AddScoped<IFirestoreRepository<SensorReading>>(sp =>
        new FirestoreRepository<SensorReading>(sp.GetRequiredService<FirestoreDb>(), "sensor_readings"));
    builder.Services.AddScoped<IFirestoreRepository<ObstructionAlert>>(sp =>
        new FirestoreRepository<ObstructionAlert>(sp.GetRequiredService<FirestoreDb>(), "obstruction_alerts"));
    builder.Services.AddScoped<IFirestoreRepository<CommunityReport>>(sp =>
        new FirestoreRepository<CommunityReport>(sp.GetRequiredService<FirestoreDb>(), "community_reports"));
    builder.Services.AddScoped<IFirestoreRepository<FloodRiskAssessment>>(sp =>
        new FirestoreRepository<FloodRiskAssessment>(sp.GetRequiredService<FirestoreDb>(), "flood_risk_assessments"));
    builder.Services.AddScoped<IFirestoreRepository<Notification>>(sp =>
        new FirestoreRepository<Notification>(sp.GetRequiredService<FirestoreDb>(), "notifications"));
    builder.Services.AddScoped<IFirestoreRepository<Announcement>>(sp =>
        new FirestoreRepository<Announcement>(sp.GetRequiredService<FirestoreDb>(), "announcements"));
}
=======
// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=coms.db"));
>>>>>>> db46004de7d488abfb831db9c6cd307518689719

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICanalService, CanalService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<ICommunityReportService, CommunityReportService>();
<<<<<<< HEAD
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
=======
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
builder.Services.AddScoped<IFloodRiskService, FloodRiskService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Authentication
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"] ?? "COMS-SUPER-SECRET-KEY-CHANGE-ME-2024";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "coms";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "coms-users";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// SignalR
builder.Services.AddSignalR();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "COMS API",
        Version = "v1",
        Description = "Canal Obstruction Monitoring System API",
        Contact = new OpenApiContact
        {
            Name = "COMS Team",
            Email = "support@coms.ph"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

<<<<<<< HEAD
// Initialize Firestore and seed data
if (File.Exists(firebaseServiceAccountPath))
{
    using (var scope = app.Services.CreateScope())
    {
        await FirestoreDbInitializer.InitializeAsync(scope.ServiceProvider);
    }
=======
// Initialize database and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbInitializer.InitializeAsync(context);
>>>>>>> db46004de7d488abfb831db9c6cd307518689719
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "COMS API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapHub<MonitoringHub>("/hubs/monitoring");

app.MapGet("/", async context =>
{
    if (context.User.Identity?.IsAuthenticated == true)
        context.Response.Redirect("/Dashboard");
    else
        context.Response.Redirect("/Auth/Login");
});

app.Run();
