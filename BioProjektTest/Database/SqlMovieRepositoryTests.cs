using BioProjekt.DataAccess.Repositories;
using BioProjekt.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using BioProjekt.DataAccess.Helpers;
using NUnit.Framework;

namespace BioProjektTest.Database;
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
        _dbCleaner = new DbCleaner(config.GetConnectionString("CinemaDb"));
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
        var result = await _repository.GetMovieByIdAsync(1);
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result?.Id);
    }

    [Test]
    public async Task GetMovieByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var result = await _repository.GetMovieByIdAsync(999);
        Assert.IsNull(result);
    }
}
