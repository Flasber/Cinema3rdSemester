using NUnit.Framework;
using BioProjektModels;
using BioProjekt.DataAccess.Repositories;
using BioProjekt.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using BioProjekt.DataAccess.Helpers;

namespace BioProjektTest.Database
{
    [TestFixture]
    public class SqlMovieRepositoryTest
    {
        private IMovieRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _repository = new SqlMovieRepository(config);
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

            Assert.IsNotNull(firstMovie, "Der er ingen film i databasen at teste med.");

            var result = await _repository.GetMovieByIdAsync(firstMovie.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(firstMovie.Id, result?.Id);
        }

        [Test]
        public async Task GetMovieByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var result = await _repository.GetMovieByIdAsync(999);
            Assert.IsNull(result);
        }
    }
}
