using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Repositories
{
    public class SqlAuditoriumRepository : IAuditoriumRepository
    {
        private readonly DbHelper _dbHelper;

        public SqlAuditoriumRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CinemaDb");
            _dbHelper = new DbHelper(connectionString);
        }

        public async Task<IEnumerable<Auditorium>> GetAllAuditoriumsAsync()
        {
            using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
            return await connection.QueryAsync<Auditorium>("SELECT * FROM Auditorium");
        }
    }
}
