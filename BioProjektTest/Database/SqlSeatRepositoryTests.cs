﻿using BioProjektModels;
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
        private DbCleaner.TestIds _testIds;

        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _repository = new SqlSeatRepository(config);
            _dbCleaner = new DbCleaner(config.GetConnectionString("CinemaDb"));
            _testIds = _dbCleaner.CleanAndInsertTestData();
        }

        [Test]
        public async Task GetAvailableSeatsForScreeningAsync_ShouldReturnSeats()
        {
            var result = await _repository.GetAvailableSeatsForScreeningAsync(_testIds.Screening1Id);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task GetScreeningSeatByIdAsync_ShouldReturnCorrectSeat()
        {
            var all = await _repository.GetAvailableSeatsForScreeningAsync(_testIds.Screening1Id);
            var one = all.FirstOrDefault();
            Assert.IsNotNull(one, "Forventede mindst ét seat i testdata.");

            var seat = await _repository.GetScreeningSeatByIdAsync(one.Id);
            Assert.IsNotNull(seat);
            Assert.AreEqual(one.Id, seat.Id);
        }

        [Test]
        public async Task TryReserveScreeningSeatAsync_ShouldReturnTrue_WhenVersionMatches()
        {
            var seats = await _repository.GetAvailableSeatsForScreeningAsync(_testIds.Screening1Id);
            var seat = seats.FirstOrDefault();
            Assert.IsNotNull(seat);

            var version = seat.Version;
            var success = await _repository.TryReserveScreeningSeatAsync(seat.Id, version);
            Assert.IsTrue(success);
        }

        [Test]
        public async Task TryReserveScreeningSeatAsync_ShouldReturnFalse_WhenAlreadyReserved()
        {
            var seats = await _repository.GetAvailableSeatsForScreeningAsync(_testIds.Screening1Id);
            var seat = seats.FirstOrDefault();
            Assert.IsNotNull(seat);

            var version = seat.Version;
            var reservedFirst = await _repository.TryReserveScreeningSeatAsync(seat.Id, version);
            var reservedSecond = await _repository.TryReserveScreeningSeatAsync(seat.Id, version);

            Assert.IsTrue(reservedFirst);
            Assert.IsFalse(reservedSecond);
        }

        [Test]
        public async Task TryReserveScreeningSeatAsync_ShouldReturnFalse_WhenVersionMismatch()
        {
            var seats = await _repository.GetAvailableSeatsForScreeningAsync(_testIds.Screening1Id);
            var seat = seats.FirstOrDefault();
            Assert.IsNotNull(seat);

            var fakeVersion = new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 };
            var result = await _repository.TryReserveScreeningSeatAsync(seat.Id, fakeVersion);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task StoreAndGetSelectedSeats_ShouldReturnStoredSeats()
        {
            var sessionId = Guid.NewGuid();
            var seats = (await _repository.GetAvailableSeatsForScreeningAsync(_testIds.Screening1Id)).Take(2).ToList();
            Assert.IsNotEmpty(seats);

            foreach (var s in seats)
                _repository.StoreSeatSelection(sessionId, s);

            var selected = await _repository.GetSelectedSeatsAsync(sessionId);
            Assert.AreEqual(2, selected.Count());
        }

        [Test]
        public async Task ClearSeatSelection_ShouldRemoveSeatsFromSession()
        {
            var sessionId = Guid.NewGuid();
            var seats = (await _repository.GetAvailableSeatsForScreeningAsync(_testIds.Screening1Id)).Take(2).ToList();
            Assert.IsNotEmpty(seats);

            foreach (var s in seats)
                _repository.StoreSeatSelection(sessionId, s);

            _repository.ClearSeatSelection(sessionId);
            var selected = await _repository.GetSelectedSeatsAsync(sessionId);
            Assert.IsEmpty(selected);
        }
    }
}
