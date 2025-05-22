using BioProjekt.DataAccess.Helpers;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Repositories;
using BioProjektModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BioProjektTest.Database
{
    [TestFixture]
    public class SqlBookingRepositoryTests
    {
        private IBookingRepository _bookingRepository;
        private DbCleaner _dbCleaner;
        private DbCleaner.TestIds _testIds;
        private IConfiguration _config;

        [SetUp]
        public void SetUp()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _bookingRepository = new SqlBookingRepository(_config);
            _dbCleaner = new DbCleaner(_config.GetConnectionString("CinemaDb"));
            _testIds = _dbCleaner.CleanAndInsertTestData();
        }

        [Test]
        public async Task CreateBookingAsync_ShouldInsertBookingAndReturnId()
        {
            var booking = new Booking
            {
                ScreeningId = _testIds.Screening1Id,
                BookingDate = DateTime.Now,
                CustomerNumber = _testIds.CustomerNumber,
                BookingStatus = "Pending",
                Price = 120,
                IsDiscounted = false
            };

            var bookingId = await _bookingRepository.CreateBookingAsync(booking);
            Assert.Greater(bookingId, 0);
        }

        [Test]
        public async Task AssignSeatsToBooking_ShouldHandleConcurrencyConflict()
        {
            var booking1 = new Booking
            {
                ScreeningId = _testIds.Screening1Id,
                BookingDate = DateTime.Now,
                CustomerNumber = _testIds.CustomerNumber,
                BookingStatus = "Pending",
                Price = 100,
                IsDiscounted = false
            };

            var booking2 = new Booking
            {
                ScreeningId = _testIds.Screening1Id,
                BookingDate = DateTime.Now,
                CustomerNumber = _testIds.CustomerNumber,
                BookingStatus = "Pending",
                Price = 100,
                IsDiscounted = false
            };

            var bookingId1 = await _bookingRepository.CreateBookingAsync(booking1);
            var bookingId2 = await _bookingRepository.CreateBookingAsync(booking2);

            using var connection = await new DbHelper(_config.GetConnectionString("CinemaDb")).CreateAndOpenConnectionAsync();

            var version = await connection.QuerySingleAsync<byte[]>(
                "SELECT Version FROM ScreeningSeat WHERE ScreeningId = @ScreeningId AND SeatId = @SeatId",
                new { ScreeningId = _testIds.Screening1Id, SeatId = _testIds.Seat1Id });

            var screeningSeat = new ScreeningSeat
            {
                ScreeningId = _testIds.Screening1Id,
                SeatId = _testIds.Seat1Id,
                Id = await connection.QuerySingleAsync<int>(
                    "SELECT Id FROM ScreeningSeat WHERE ScreeningId = @ScreeningId AND SeatId = @SeatId",
                    new { ScreeningId = _testIds.Screening1Id, SeatId = _testIds.Seat1Id }),
                IsAvailable = true,
                Version = version
            };

            var selectedSeats = new List<ScreeningSeat> { screeningSeat };

            var task1 = Task.Run(async () =>
            {
                await _bookingRepository.AssignSeatsToBooking(Guid.NewGuid(), bookingId1, selectedSeats);
            });

            var task2 = Task.Run(async () =>
            {
                await _bookingRepository.AssignSeatsToBooking(Guid.NewGuid(), bookingId2, selectedSeats);
            });

            try
            {
                await Task.WhenAll(task1, task2);
            }
            catch { }

            var failedTasks = new[] { task1, task2 }.Count(t => t.IsFaulted);
            Assert.AreEqual(1, failedTasks, "Præcis én transaktion bør fejle pga. concurrency-konflikt.");
        }
    }
}
