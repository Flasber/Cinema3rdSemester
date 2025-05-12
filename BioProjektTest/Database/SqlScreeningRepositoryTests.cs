using BioProjekt.DataAccess.Repositories;
using BioProjektModels.Interfaces;
using Microsoft.Extensions.Configuration;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.DataAccess.Helpers;


[TestFixture]
public class SqlScreeningRepositoryTests
{
    private IScreeningRepository _repository;
    private DbCleaner _dbCleaner;

    [SetUp]
    public void SetUp()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _repository = new SqlScreeningRepository(config);
        _dbCleaner = new DbCleaner(config.GetConnectionString("CinemaDb"));
        _dbCleaner.CleanAndInsertTestData();
    }

    [Test]
    public async Task GetAllScreeningsAsync_ShouldReturnScreenings()
    {
        var result = await _repository.GetAllScreeningsAsync();
        Assert.IsNotEmpty(result);
    }
}
