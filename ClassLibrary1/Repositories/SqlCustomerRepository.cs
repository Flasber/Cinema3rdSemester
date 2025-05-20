using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioProjekt.DataAccess.Helpers;
using BioProjektModels;
using Microsoft.Extensions.Configuration;
using Dapper;
using BioProjekt.DataAccess.Interfaces;


namespace DataAccess.Repositories;
public class SqlCustomerRepository : ICustomerRepository
{
    private readonly DbHelper _dbHelper;

    public SqlCustomerRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CinemaDb");
        _dbHelper = new DbHelper(connectionString);
    }

    public async Task<int> CreateCustomerAsync(Customer customer)
    {
        using var connection = await _dbHelper.CreateAndOpenConnectionAsync();
        var sql = @"
        INSERT INTO Customer (Name, Email, MobileNumber, Address, CustomerType)
        OUTPUT INSERTED.CustomerNumber
        VALUES (@Name, @Email, @MobileNumber, @Address, @CustomerType)";

        return await connection.ExecuteScalarAsync<int>(sql, customer);
    }

}
