using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Repositories
{
    public class SqlScreeningRepository : IScreeningRepository
    {
        private readonly DbHelper _dbHelper;

        public SqlScreeningRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CinemaDb");
            _dbHelper = new DbHelper(connectionString);
        }

        public async Task<IEnumerable<Screening>> GetAllScreeningsAsync()
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            return await connection.QueryAsync<Screening>("SELECT * FROM Screening");
        }

        public async Task AddScreeningAsync(Screening screening)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();

            var sql = @"
                INSERT INTO Screening (MovieId, Date, Time, LanguageVersion, Is3D, IsSoldOut, SoundSystem, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (@MovieId, @Date, @Time, @LanguageVersion, @Is3D, @IsSoldOut, @SoundSystem, @AuditoriumId)";

            screening.Id = await connection.ExecuteScalarAsync<int>(sql, screening);
        }

        public async Task<Screening?> GetScreeningByIdAsync(int id)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = "SELECT * FROM Screening WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Screening>(sql, new { Id = id });
        }
    }
}
