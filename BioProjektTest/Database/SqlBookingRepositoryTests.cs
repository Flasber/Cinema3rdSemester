using BioProjekt.DataAccess.Helpers;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Repositories;
using BioProjektModels;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BioProjektTest.Database
{
    [TestFixture]
    public class SqlBookingRepositoryTests
    {
        private IBookingRepository _bookingRepository;
        private DbCleaner _dbCleaner;
        private DbCleaner.TestIds _testIds;

        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _bookingRepository = new SqlBookingRepository(config);
            _dbCleaner = new DbCleaner(config.GetConnectionString("CinemaDb"));
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
    }
}
