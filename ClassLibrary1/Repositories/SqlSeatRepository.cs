using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Repositories
{
    public class SqlSeatRepository : ISeatRepository
    {
        private readonly DbHelper _dbHelper;
        private readonly Dictionary<Guid, List<ScreeningSeat>> _sessionSeats = new();

        public SqlSeatRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CinemaDb");
            _dbHelper = new DbHelper(connectionString);
        }

        public async Task<IEnumerable<ScreeningSeat>> GetAvailableSeatsForScreeningAsync(int screeningId)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = @"
                SELECT ss.*, s.SeatNumber, s.Row, s.SeatType, s.AuditoriumId, s.PriceModifier
                FROM ScreeningSeat ss
                JOIN Seat s ON ss.SeatId = s.Id
                WHERE ss.ScreeningId = @ScreeningId AND ss.IsAvailable = 1";

            return await connection.QueryAsync<ScreeningSeat, Seat, ScreeningSeat>(
                sql,
                (ss, seat) =>
                {
                    ss.Seat = seat;
                    return ss;
                },
                new { ScreeningId = screeningId },
                splitOn: "SeatNumber");
        }

        public async Task<ScreeningSeat?> GetScreeningSeatByIdAsync(int screeningSeatId)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = @"
                SELECT ss.*, s.SeatNumber, s.Row, s.SeatType, s.AuditoriumId, s.PriceModifier
                FROM ScreeningSeat ss
                JOIN Seat s ON ss.SeatId = s.Id
                WHERE ss.Id = @Id";

            return (await connection.QueryAsync<ScreeningSeat, Seat, ScreeningSeat>(
                sql,
                (ss, seat) => { ss.Seat = seat; return ss; },
                new { Id = screeningSeatId },
                splitOn: "SeatNumber")).FirstOrDefault();
        }

        public async Task<bool> TryReserveScreeningSeatAsync(int screeningSeatId, byte[] clientVersion)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var seat = await connection.QueryFirstOrDefaultAsync<ScreeningSeat>(
                "SELECT * FROM ScreeningSeat WHERE Id = @Id",
                new { Id = screeningSeatId }, transaction);

            if (seat == null || !seat.IsAvailable || !seat.Version.SequenceEqual(clientVersion))
                return false;

            var rowsAffected = await connection.ExecuteAsync(
                "UPDATE ScreeningSeat SET IsAvailable = 0 WHERE Id = @Id AND Version = @Version",
                new { Id = screeningSeatId, Version = clientVersion }, transaction);

            if (rowsAffected == 0)
                return false;

            await transaction.CommitAsync();
            return true;
        }

        public async Task AssignSeatsToBooking(Guid sessionId, int bookingId, List<ScreeningSeat> selectedSeats)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            using var transaction = connection.BeginTransaction();

            foreach (var screeningSeat in selectedSeats)
            {
                await connection.ExecuteAsync(
                    "INSERT INTO BookingSeat (BookingId, SeatId) VALUES (@BookingId, @SeatId)",
                    new { BookingId = bookingId, SeatId = screeningSeat.SeatId }, transaction);

                await connection.ExecuteAsync(
                    "UPDATE ScreeningSeat SET IsAvailable = 0 WHERE Id = @Id",
                    new { Id = screeningSeat.Id }, transaction);
            }

            await transaction.CommitAsync();
        }

        public async Task CreateScreeningSeatsAsync(int screeningId, int auditoriumId)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var seatIds = await connection.QueryAsync<int>(
                "SELECT Id FROM Seat WHERE AuditoriumId = @AuditoriumId", new { AuditoriumId = auditoriumId });

            foreach (var seatId in seatIds)
            {
                await connection.ExecuteAsync(
                    "INSERT INTO ScreeningSeat (ScreeningId, SeatId, IsAvailable) VALUES (@ScreeningId, @SeatId, 1)",
                    new { ScreeningId = screeningId, SeatId = seatId });
            }
        }

        public Task<IEnumerable<ScreeningSeat>> GetSelectedSeatsAsync(Guid sessionId)
        {
            if (_sessionSeats.TryGetValue(sessionId, out var seats))
                return Task.FromResult(seats.AsEnumerable());

            return Task.FromResult(Enumerable.Empty<ScreeningSeat>());
        }

        public void StoreSeatSelection(Guid sessionId, ScreeningSeat screeningSeat)
        {
            if (!_sessionSeats.ContainsKey(sessionId))
                _sessionSeats[sessionId] = new List<ScreeningSeat>();

            if (!_sessionSeats[sessionId].Any(s => s.Id == screeningSeat.Id))
                _sessionSeats[sessionId].Add(screeningSeat);
        }

        public void ClearSeatSelection(Guid sessionId)
        {
            _sessionSeats.Remove(sessionId);
        }
    }
}
