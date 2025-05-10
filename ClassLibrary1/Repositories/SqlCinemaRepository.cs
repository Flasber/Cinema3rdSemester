using BioProjektModels.Interfaces;
using BioProjektModels;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Helpers;
using Microsoft.Extensions.Configuration;

namespace BioProjekt.DataAccess.Repositories
{
    public class SqlCinemaRepository : ISqlCinemaRepository
    {
        private readonly DbHelper _dbHelper;

        public SqlCinemaRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CinemaDb");
            _dbHelper = new DbHelper(connectionString);
        }

        private IDbConnection CreateConnection()
        {
            return _dbHelper.CreateConnection();
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Movie";
            return await connection.QueryAsync<Movie>(sql);
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Movie WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Movie>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Auditorium>> GetAllAuditoriums()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Auditorium";
            return await connection.QueryAsync<Auditorium>(sql); 
        }

        public async Task<IEnumerable<Screening>> GetAllScreenings()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Screening";
            return await connection.QueryAsync<Screening>(sql);
        }

        public async Task<IEnumerable<Seat>> GetSeatsForAuditorium(int auditoriumId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Seat WHERE AuditoriumId = @AuditoriumId";
            return await connection.QueryAsync<Seat>(sql, new { AuditoriumId = auditoriumId });
        }

        public async Task AddSeat(Seat seat)
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO Seat (SeatNumber, Row, AuditoriumId, IsAvailable, Version) VALUES (@SeatNumber, @Row, @AuditoriumId, @IsAvailable, @Version)";
            await connection.ExecuteAsync(sql, seat);
        }

        public async Task<Seat?> GetSeat(int seatNumber, string row, int auditoriumId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Seat WHERE SeatNumber = @SeatNumber AND Row = @Row AND AuditoriumId = @AuditoriumId";
            return await connection.QueryFirstOrDefaultAsync<Seat>(sql, new { SeatNumber = seatNumber, Row = row, AuditoriumId = auditoriumId });
        }

        public async Task<bool> TryReserveSeat(int seatNumber, string row, byte[] clientVersion, int auditoriumId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Seat WHERE SeatNumber = @SeatNumber AND Row = @Row AND AuditoriumId = @AuditoriumId";
            var seat = await connection.QueryFirstOrDefaultAsync<Seat>(sql, new { SeatNumber = seatNumber, Row = row, AuditoriumId = auditoriumId });

            if (seat == null || !seat.IsAvailable || !seat.Version.SequenceEqual(clientVersion))
                return false;

            seat.IsAvailable = false;

            var updateSql = "UPDATE Seat SET IsAvailable = @IsAvailable, Version = @Version WHERE Id = @Id AND Version = @ClientVersion";
            var rowsAffected = await connection.ExecuteAsync(updateSql, new
            {
                IsAvailable = seat.IsAvailable,
                Version = seat.Version,
                Id = seat.Id,
                ClientVersion = clientVersion
            });

            return rowsAffected > 0;
        }
    }
}
