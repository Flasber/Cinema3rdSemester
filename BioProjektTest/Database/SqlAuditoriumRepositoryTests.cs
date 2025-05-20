using BioProjekt.DataAccess.Repositories;
using BioProjekt.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using BioProjekt.DataAccess.Helpers;
using NUnit.Framework;
namespace BioProjektTest.Database;
[TestFixture]
public class SqlAuditoriumRepositoryTests
{
    private IAuditoriumRepository _repository;
    private DbCleaner _dbCleaner;

    [SetUp]
    public void SetUp()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _repository = new SqlAuditoriumRepository(config);
        _dbCleaner = new DbCleaner(config.GetConnectionString("CinemaDb"));
        _dbCleaner.CleanAndInsertTestData();
    }

    [Test]
    public async Task GetAllAuditoriumsAsync_ShouldReturnAuditoriums()
    {
        var result = await _repository.GetAllAuditoriumsAsync();
        Assert.IsNotEmpty(result);
    }
}
