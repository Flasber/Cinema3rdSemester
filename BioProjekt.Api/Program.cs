using BioProjekt.Api.BusinessLogic;
using BioProjekt.Api.Data.Mock;
using BioProjekt.Api.Data.Mockdatabase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ICinemaRepository, MockCinemaRepository>();
builder.Services.AddSingleton<IMovieService, MovieService>();
builder.Services.AddSingleton<IBookingService, BookingService>();
builder.Services.AddSingleton<IScreeningService, ScreeningService>();
builder.Services.AddScoped<IAuditoriumService, AuditoriumService>();
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
