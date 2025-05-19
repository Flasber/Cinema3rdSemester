using BioProjekt.DataAccess.Helpers;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using BioProjekt.DataAccess.Helpers;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BioProjektTest.Database
{
    [TestFixture]
    public class SqlMovieRepositoryTests
    {
        private IMovieRepository _repository;
        private DbCleaner _dbCleaner;


        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _repository = new SqlMovieRepository(config);

            var connString = config.GetConnectionString("CinemaDb");
            _dbCleaner = new DbCleaner(connString);
            _dbCleaner.CleanAndInsertTestData();
        }


        [Test]
        public async Task GetAllMoviesAsync_ShouldReturnMovies()
        {
            var result = await _repository.GetAllMoviesAsync();
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task GetMovieByIdAsync_ShouldReturnMovie_WhenFound()
        {
            var allMovies = await _repository.GetAllMoviesAsync();
            var firstMovie = allMovies.FirstOrDefault();
            Assert.IsNotNull(firstMovie);

            var result = await _repository.GetMovieByIdAsync(firstMovie.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(firstMovie.Id, result.Id);
        }







        [Test]
        public async Task GetMovieByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _repository.GetMovieByIdAsync(999999); 
            Assert.IsNull(result);
        }
    }
}
