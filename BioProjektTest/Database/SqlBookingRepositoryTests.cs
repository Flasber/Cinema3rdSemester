using BioProjekt.DataAccess.Helpers;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Repositories;
using BioProjektModels;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
namespace BioProjektTest.Database;
[TestFixture]
public class SqlBookingRepositoryTests
{
    private IBookingRepository _bookingRepository;
    private ISeatRepository _seatRepository;
    private DbCleaner _dbCleaner;

    [SetUp]
    public void SetUp()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _bookingRepository = new SqlBookingRepository(config);
        _seatRepository = new SqlSeatRepository(config);
        _dbCleaner = new DbCleaner(config.GetConnectionString("CinemaDb"));
        _dbCleaner.CleanAndInsertTestData();
    }

    [Test]
    public async Task CreateBookingAsync_ShouldInsertBookingAndReturnId()
    {
        var booking = new Booking
        {
            ScreeningId = 1,
            BookingDate = DateTime.Now,
            CustomerNumber = 1,
            BookingStatus = "Pending",
            Price = 120,
            IsDiscounted = false
        };

        var bookingId = await _bookingRepository.CreateBookingAsync(booking);
        Assert.Greater(bookingId, 0);
    }

}
