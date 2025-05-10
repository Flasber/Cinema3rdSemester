using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using BioProjekt.DataAccess.Repositories;
using BioProjektModels.Interfaces;
using BioProjektModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjektTest.Database
{
    [TestFixture]
    public class SqlCinemaRepositoryTest
    {
        private string _connectionString;
        private ISqlCinemaRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = config.GetConnectionString("CinemaDb");

            _repository = new SqlCinemaRepository(config);
        }

        [Test]
        public async Task GetAllMoviesAsync_ShouldReturnMovies()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await _repository.GetAllMoviesAsync();
                Assert.IsNotEmpty(result);
            }
        }

        [Test]
        public async Task GetMovieByIdAsync_ShouldReturnMovie_WhenFound()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await _repository.GetMovieByIdAsync(1);
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result?.Id);
            }
        }

        [Test]
        public async Task GetMovieByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await _repository.GetMovieByIdAsync(999);
                Assert.IsNull(result);
            }
        }
    }
}
