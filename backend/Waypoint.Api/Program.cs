using Microsoft.EntityFrameworkCore;
using Waypoint.Infrastructure;
using Waypoint.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure connection string from environment variables if available
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
if (!string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine($"Using connection string from environment variable");
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}
else
{
    Console.WriteLine("No connection string found in environment variables. Using configuration file.");
}

Console.WriteLine($"Connection string: {builder.Configuration.GetConnectionString("DefaultConnection")}");

// Add services to the container.
builder.Services.AddControllers();

// Add our infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Add direct DB context for more control when needed
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowLocalhost");

app.UseAuthorization();

app.MapControllers();

app.Run();
