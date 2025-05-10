using BioProjektModels.Interfaces;
using BioProjekt.DataAccess.Repositories;
using BioProjekt.Api.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

// Tilf�j controllers (API controllers)
builder.Services.AddControllers();

// Tilf�j ISqlCinemaRepository som singleton, som implementeres af SqlCinemaRepository
builder.Services.AddSingleton<ISqlCinemaRepository, SqlCinemaRepository>();

// Tilf�j services som singleton
builder.Services.AddSingleton<IMovieService, MovieService>();
builder.Services.AddSingleton<IBookingService, BookingService>();
builder.Services.AddSingleton<IScreeningService, ScreeningService>();
builder.Services.AddScoped<IAuditoriumService, AuditoriumService>();
builder.Services.AddSingleton<ISeatService, SeatService>();

// Tilf�j Endpoints API Explorer til Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aktiv�r Swagger i udviklingsmilj�
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

// K�r applikationen
app.Run();
