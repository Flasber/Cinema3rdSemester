using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace BioProjekt.DataAccess.Helpers;


public class DbHelper
{
    private readonly string _connectionString;

    public DbHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task OpenConnectionAsync(SqlConnection connection)
    {
        await connection.OpenAsync();
    }

    public async Task<SqlConnection> CreateAndOpenConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}
