using BioProjekt.Api.BusinessLogic;
using BioProjekt.DataAccess.Repositories;
using BioProjektModels.Interfaces;
using DataAccess.Helpers;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Tilføj konfigurationen (appsettings.json) til DI-containeren
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        // Tilføj controllers til DI-containeren
        builder.Services.AddControllers();

        // Registrer ISqlCinemaRepository med SqlCinemaRepository som implementering
        builder.Services.AddScoped<ISqlCinemaRepository, SqlCinemaRepository>();

        // Registrer DbHelper med den nødvendige forbindelsesstreng
        builder.Services.AddScoped<DbHelper>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("CinemaDb");
            return new DbHelper(connectionString); // Opretter en ny DbHelper-instans med forbindelsesstrengen
        });

        // Registrering af øvrige services
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddScoped<IScreeningService, ScreeningService>();
        builder.Services.AddScoped<ISeatService, SeatService>();
        builder.Services.AddScoped<IAuditoriumService, AuditoriumService>();
        builder.Services.AddScoped<IBookingService, BookingService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
