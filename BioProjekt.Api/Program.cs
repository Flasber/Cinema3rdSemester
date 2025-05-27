using BioProjekt.Api.BusinessLogic;
using BioProjekt.Api.Storage;
using BioProjekt.DataAccess.Repositories;
using BioProjektModels.Interfaces;
using BioProjekt.DataAccess.Helpers;
using BioProjekt.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using DataAccess.Repositories;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddControllers();

        builder.Services.AddScoped<DbHelper>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("CinemaDb");
            return new DbHelper(connectionString);
        });

        // Repository-registreringer
        builder.Services.AddScoped<IMovieRepository, SqlMovieRepository>();
        builder.Services.AddScoped<IScreeningRepository, SqlScreeningRepository>();
        builder.Services.AddScoped<IAuditoriumRepository, SqlAuditoriumRepository>();
        builder.Services.AddScoped<ISeatRepository, SqlSeatRepository>();
        builder.Services.AddScoped<IBookingRepository, SqlBookingRepository>();

        // Service-registreringer
        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddScoped<IScreeningService, ScreeningService>();
        builder.Services.AddScoped<IAuditoriumService, AuditoriumService>();
        builder.Services.AddScoped<ISeatService, SeatService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<ICustomerRepository, SqlCustomerRepository>();
        builder.Services.AddSingleton<SeatSelectionStore>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
