using BioProjektModels.Interfaces;
using BioProjekt.DataAccess.Repositories;
using BioProjekt.Api.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

// Tilføj controllers (API controllers)
builder.Services.AddControllers();

// Tilføj ISqlCinemaRepository som singleton, som implementeres af SqlCinemaRepository
builder.Services.AddSingleton<ISqlCinemaRepository, SqlCinemaRepository>();

// Tilføj services som singleton
builder.Services.AddSingleton<IMovieService, MovieService>();
builder.Services.AddSingleton<IBookingService, BookingService>();
builder.Services.AddSingleton<IScreeningService, ScreeningService>();
builder.Services.AddScoped<IAuditoriumService, AuditoriumService>();
builder.Services.AddSingleton<ISeatService, SeatService>();

// Tilføj Endpoints API Explorer til Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aktivér Swagger i udviklingsmiljø
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aktiver HTTPS og autorisation
app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers til ruter
app.MapControllers();

// Kør applikationen
app.Run();
