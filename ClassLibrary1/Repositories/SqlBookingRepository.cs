using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Repositories
{
    public class SqlBookingRepository : IBookingRepository
    {
        private readonly DbHelper _dbHelper;

        public SqlBookingRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CinemaDb");
            _dbHelper = new DbHelper(connectionString);
        }

        public async Task<int> CreateBookingAsync(Booking booking)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = @"
                INSERT INTO Booking (ScreeningId, BookingDate, CustomerNumber, BookingStatus, Price, IsDiscounted)
                OUTPUT INSERTED.BookingId
                VALUES (@ScreeningId, @BookingDate, @CustomerNumber, @BookingStatus, @Price, @IsDiscounted)";
            return await connection.ExecuteScalarAsync<int>(sql, booking);
        }
        public async Task AssignSeatsToBooking(Guid sessionId, int bookingId, List<ScreeningSeat> selectedSeats)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                foreach (var screeningSeat in selectedSeats)
                {
                    var seatId = screeningSeat.SeatId;
                    var rowsAffected = await connection.ExecuteAsync(
                        @"UPDATE ScreeningSeat 
                        SET IsAvailable = 0 
                        WHERE Id = @Id AND Version = @Version",
                        new { Id = screeningSeat.Id, Version = screeningSeat.Version },
                        transaction);

                    if (rowsAffected == 0)
                        throw new InvalidOperationException($"Sædet {screeningSeat.Id} er allerede blevet reserveret af en anden.");

                    await connection.ExecuteAsync(
                        "INSERT INTO BookingSeat (BookingId, SeatId) VALUES (@BookingId, @SeatId)",
                        new { BookingId = bookingId, SeatId = seatId },
                        transaction);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync(); 
                throw;
            }

        }
    }
}
