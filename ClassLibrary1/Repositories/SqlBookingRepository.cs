using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
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


        public async Task AddSeatToBookingAsync(int bookingId, int seatId)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = "INSERT INTO BookingSeat (BookingId, SeatId) VALUES (@BookingId, @SeatId)";
            await connection.ExecuteAsync(sql, new { BookingId = bookingId, SeatId = seatId });
        }
    }
}
