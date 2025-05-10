using DataAccess.Helpers;
using BioProjektModels.Interfaces;
using BioProjektModels;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;

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
    }
}
