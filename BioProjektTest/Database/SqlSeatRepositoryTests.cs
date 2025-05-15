using NUnit.Framework;
using BioProjektModels;
using BioProjekt.DataAccess.Repositories;
using BioProjekt.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using BioProjekt.DataAccess.Repositories;
using BioProjektModels.Interfaces;
using BioProjekt.DataAccess.Helpers;


[TestFixture]
public class SqlSeatRepositoryTests
{
    private ISeatRepository _repository;
    private DbCleaner _dbCleaner;

    [SetUp]
    public void SetUp()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _repository = new SqlSeatRepository(config);
        _dbCleaner = new DbCleaner(config.GetConnectionString("CinemaDb"));
        _dbCleaner.CleanAndInsertTestData();
    }

    [Test]
    public async Task GetSeatsForAuditorium_ShouldReturnSeats()
    {
        var result = await _repository.GetSeatsForAuditorium(1);
        Assert.IsNotEmpty(result);
    }

    [Test]
    public async Task AddSeat_ShouldInsertSeat()
    {
        var seat = new Seat
        {
            SeatNumber = 5,
            Row = "B",
            SeatType = "Standard",
            AuditoriumId = 1,
            PriceModifier = 1.0m,
            IsAvailable = true
        };

        await _repository.AddSeat(seat);

        var result = await _repository.GetSeat(seat.SeatNumber, seat.Row, seat.AuditoriumId);
        Assert.IsNotNull(result);
        Assert.AreEqual(seat.SeatNumber, result?.SeatNumber);
    }

    [Test]
    public async Task GetSeat_ShouldReturnSeat_WhenFound()
    {
        var result = await _repository.GetSeat(1, "A", 1);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result?.SeatNumber);
    }

    [Test]
    public async Task TryReserveSeat_ShouldReturnTrue_WhenSeatReserved()
    {
        var seat = await _repository.GetSeat(1, "A", 1);
        var clientVersion = seat?.Version;

        var result = await _repository.TryReserveSeat(1, "A", clientVersion, 1);
        Assert.IsTrue(result);
    }

    [Test]
    public async Task TryReserveSeat_ShouldReturnFalse_WhenSeatNotAvailable()
    {
        var seat = await _repository.GetSeat(1, "A", 1);
        Assert.IsNotNull(seat);

        var clientVersion = seat?.Version;
        seat.IsAvailable = false;

        await _repository.UpdateSeat(seat);

        var result = await _repository.TryReserveSeat(1, "A", clientVersion, 1);
        Assert.IsFalse(result);
    }

    [Test]
    public async Task TryReserveSeat_ShouldFailWhenVersionMismatch()
    {
        var seat = new Seat
        {
            SeatNumber = 2,
            Row = "B",
            SeatType = "Standard",
            AuditoriumId = 1,
            PriceModifier = 1.0m,
            IsAvailable = true
        };

        await _repository.AddSeat(seat);

        var seat1 = await _repository.GetSeat(seat.SeatNumber, seat.Row, seat.AuditoriumId);
        var version1 = seat1?.Version;

        var result1 = await _repository.TryReserveSeat(seat.SeatNumber, seat.Row, version1, seat.AuditoriumId);
        Assert.IsTrue(result1);

        var result2 = await _repository.TryReserveSeat(seat.SeatNumber, seat.Row, version1, seat.AuditoriumId);
        Assert.IsFalse(result2);
    }
}
