using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Parking System API", Version = "v1" });
});

// Register DbContext
builder.Services.AddDbContext<ParkingSystem.Data.ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("ParkingSystemDb"));

// Register Repositories (Hexagonal Architecture - Infrastructure Layer)
builder.Services.AddScoped(typeof(ParkingSystem.Repositories.Interfaces.IRepository<>), typeof(ParkingSystem.Repositories.Implementations.Repository<>));
builder.Services.AddScoped<ParkingSystem.Repositories.Interfaces.IParkingLotRepository, ParkingSystem.Repositories.Implementations.ParkingLotRepository>();
builder.Services.AddScoped<ParkingSystem.Repositories.Interfaces.IParkingSpotRepository, ParkingSystem.Repositories.Implementations.ParkingSpotRepository>();
builder.Services.AddScoped<ParkingSystem.Repositories.Interfaces.IReservationRepository, ParkingSystem.Repositories.Implementations.ReservationRepository>();
builder.Services.AddScoped<ParkingSystem.Repositories.Interfaces.ICarRepository, ParkingSystem.Repositories.Implementations.CarRepository>();

// Register Services (Hexagonal Architecture - Application Layer)
builder.Services.AddScoped<ParkingSystem.Services.Interfaces.IParkingLotService, ParkingSystem.Services.Implementations.ParkingLotService>();
builder.Services.AddScoped<ParkingSystem.Services.Interfaces.IParkingSpotService, ParkingSystem.Services.Implementations.ParkingSpotService>();
builder.Services.AddScoped<ParkingSystem.Services.Interfaces.IReservationService, ParkingSystem.Services.Implementations.ReservationService>();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ParkingSystem.Data.ApplicationDbContext>();
    await ParkingSystem.Data.DataSeeder.SeedDataAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();
