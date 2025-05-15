using BioProjekt.DataAccess.Interfaces;
using BioProjektModels;
using Dapper;
using BioProjekt.DataAccess.Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Repositories
{
    public class SqlSeatRepository : ISeatRepository
    {
        private readonly DbHelper _dbHelper;

        public SqlSeatRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CinemaDb");
            _dbHelper = new DbHelper(connectionString);
        }

        public async Task<IEnumerable<Seat>> GetSeatsForAuditorium(int auditoriumId)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = "SELECT * FROM Seat WHERE AuditoriumId = @AuditoriumId";
            return await connection.QueryAsync<Seat>(sql, new { AuditoriumId = auditoriumId });
        }

        public async Task<Seat?> GetSeat(int seatNumber, string row, int auditoriumId)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = "SELECT * FROM Seat WHERE SeatNumber = @SeatNumber AND Row = @Row AND AuditoriumId = @AuditoriumId";
            return await connection.QueryFirstOrDefaultAsync<Seat>(sql, new { SeatNumber = seatNumber, Row = row, AuditoriumId = auditoriumId });
        }

        public async Task AddSeat(Seat seat)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = @"
                INSERT INTO Seat (SeatNumber, Row, SeatType, IsAvailable, PriceModifier, AuditoriumId)
                VALUES (@SeatNumber, @Row, @SeatType, @IsAvailable, @PriceModifier, @AuditoriumId)";
            await connection.ExecuteAsync(sql, seat);
        }

        public async Task<bool> TryReserveSeat(int seatNumber, string row, byte[] clientVersion, int auditoriumId)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var seat = await connection.QueryFirstOrDefaultAsync<Seat>(
                "SELECT * FROM Seat WHERE SeatNumber = @SeatNumber AND Row = @Row AND AuditoriumId = @AuditoriumId",
                new { SeatNumber = seatNumber, Row = row, AuditoriumId = auditoriumId }, transaction);

            if (seat == null || !seat.IsAvailable || !seat.Version.SequenceEqual(clientVersion))
            {
                return false;
            }

            seat.IsAvailable = false;

            var updateSql = @"
                UPDATE Seat 
                SET IsAvailable = @IsAvailable 
                WHERE SeatNumber = @SeatNumber AND Row = @Row AND AuditoriumId = @AuditoriumId AND Version = @ClientVersion";

            var rowsAffected = await connection.ExecuteAsync(updateSql, new
            {
                IsAvailable = seat.IsAvailable,
                SeatNumber = seat.SeatNumber,
                Row = seat.Row,
                AuditoriumId = seat.AuditoriumId,
                ClientVersion = clientVersion
            }, transaction);

            if (rowsAffected == 0)
            {
                return false;
            }

            await transaction.CommitAsync();
            return true;
        }

        public async Task UpdateSeat(Seat seat)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = "UPDATE Seat SET IsAvailable = @IsAvailable WHERE Id = @Id";
            await connection.ExecuteAsync(sql, seat);
        }

        public async Task AssignSeatsToBooking(Guid sessionId, int bookingId, List<Seat> selectedSeats)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            using var transaction = connection.BeginTransaction();

            foreach (var seat in selectedSeats)
            {
                await connection.ExecuteAsync(
                    "INSERT INTO BookingSeat (BookingId, SeatId) VALUES (@BookingId, @SeatId)",
                    new { BookingId = bookingId, SeatId = seat.Id },
                    transaction);

                await connection.ExecuteAsync(
                    "UPDATE Seat SET IsAvailable = 0 WHERE Id = @SeatId",
                    new { SeatId = seat.Id },
                    transaction);
            }

            await transaction.CommitAsync();
        }
        public async Task<IEnumerable<Seat>> GetSeatsForBookingAsync(int bookingId)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = @"SELECT s.* 
                FROM Seat s
                INNER JOIN BookingSeat bs ON s.Id = bs.SeatId
                WHERE bs.BookingId = @BookingId";

            return await connection.QueryAsync<Seat>(sql, new { BookingId = bookingId });
        }

    }
}
