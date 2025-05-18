using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BioProjekt.DataAccess.Repositories
{
    public class SqlMovieRepository : IMovieRepository
    {
        private readonly DbHelper _dbHelper;

        public SqlMovieRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CinemaDb");
            _dbHelper = new DbHelper(connectionString);
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            return await connection.QueryAsync<Movie>("SELECT * FROM Movie");
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            var sql = "SELECT * FROM Movie WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Movie>(sql, new { Id = id });
        }
        public async Task CreateMovieAsync(Movie movie)
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();

            var sql = @"
        INSERT INTO Movie (Title, Genre, Duration, Description, Language, AgeRating, PosterUrl)
        VALUES (@Title, @Genre, @Duration, @Description, @Language, @AgeRating, @PosterUrl)";

            await connection.ExecuteAsync(sql, movie);
        }

    }
}
