using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using DataAccess.Helpers;
using BioProjektModels.Interfaces;

namespace BioProjektTest.Database
{
    [TestFixture]
    public class DatabaseConnectionTest
    {
        private string _connectionString;

        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = config.GetConnectionString("CinemaDb");
        }

        [Test]
        public void TestDatabaseConnection()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Assert.AreEqual(System.Data.ConnectionState.Open, connection.State);
            }
        }
    }
}
