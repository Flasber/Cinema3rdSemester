using BioProjektModels;
using BioProjekt.DataAccess.Repositories;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Helpers;
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
        public async Task GetAvailableSeatsForScreeningAsync_ShouldReturnSeats()
        {
            var result = await _repository.GetAvailableSeatsForScreeningAsync(1);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task GetScreeningSeatByIdAsync_ShouldReturnCorrectSeat()
        {
            var all = await _repository.GetAvailableSeatsForScreeningAsync(1);
            var one = all.First();

            var seat = await _repository.GetScreeningSeatByIdAsync(one.Id);
            Assert.IsNotNull(seat);
            Assert.AreEqual(one.Id, seat.Id);
        }

        [Test]
        public async Task TryReserveScreeningSeatAsync_ShouldReturnTrue_WhenVersionMatches()
        {
            var seats = await _repository.GetAvailableSeatsForScreeningAsync(1);
            var seat = seats.First();
            var version = seat.Version;

            var success = await _repository.TryReserveScreeningSeatAsync(seat.Id, version);
            Assert.IsTrue(success);
        }

        [Test]
        public async Task TryReserveScreeningSeatAsync_ShouldReturnFalse_WhenAlreadyReserved()
        {
            var seats = await _repository.GetAvailableSeatsForScreeningAsync(1);
            var seat = seats.First();
            var version = seat.Version;

            var reservedFirst = await _repository.TryReserveScreeningSeatAsync(seat.Id, version);
            var reservedSecond = await _repository.TryReserveScreeningSeatAsync(seat.Id, version);

            Assert.IsTrue(reservedFirst);
            Assert.IsFalse(reservedSecond);
        }

        [Test]
        public async Task TryReserveScreeningSeatAsync_ShouldReturnFalse_WhenVersionMismatch()
        {
            var seats = await _repository.GetAvailableSeatsForScreeningAsync(1);
            var seat = seats.First();
            var fakeVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }; // Simuleret forkert version

            var result = await _repository.TryReserveScreeningSeatAsync(seat.Id, fakeVersion);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task StoreAndGetSelectedSeats_ShouldReturnStoredSeats()
        {
            var sessionId = Guid.NewGuid();
            var seats = (await _repository.GetAvailableSeatsForScreeningAsync(1)).Take(2).ToList();

            foreach (var s in seats)
                _repository.StoreSeatSelection(sessionId, s);

            var selected = await _repository.GetSelectedSeatsAsync(sessionId);
            Assert.AreEqual(2, selected.Count());
        }

        [Test]
        public async Task ClearSeatSelection_ShouldRemoveSeatsFromSession()
        {
            var sessionId = Guid.NewGuid();
            var seats = (await _repository.GetAvailableSeatsForScreeningAsync(1)).Take(2).ToList();

            foreach (var s in seats)
                _repository.StoreSeatSelection(sessionId, s);

            _repository.ClearSeatSelection(sessionId);
            var selected = await _repository.GetSelectedSeatsAsync(sessionId);
            Assert.IsEmpty(selected);
        }

    }
}
