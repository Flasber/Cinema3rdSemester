using BioProjektModels.Interfaces;
using BioProjektModels;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Moq;
using BioProjekt.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;

[TestFixture]
public class SqlCinemaRepositoryTests
{
    private ISqlCinemaRepository _repository;
    private DbCleaner _dbCleaner;

    [SetUp]
    public void Setup()
    {
        // Opret forbindelse til databasen og ryd den først
        _dbCleaner = new DbCleaner("Server=localhost,1433;Database=CinemaDB;User Id=sa;Password=NyStærkAdgangskode123;TrustServerCertificate=True;");
        _dbCleaner.CleanAndInsertTestData(); // Renser og indsætter testdata

        var mockConfiguration = new Mock<IConfiguration>();
        _repository = new SqlCinemaRepository(mockConfiguration.Object);
    }

    [Test]
    public async Task GetAllMoviesAsync_ShouldReturnCorrectMovies()
    {
        var movies = await _repository.GetAllMoviesAsync();

        Assert.AreEqual(3, movies.Count());
        Assert.AreEqual("Inception", movies.First().Title);
    }

    [Test]
    public async Task GetMovieByIdAsync_ShouldReturnCorrectMovie_WhenFound()
    {
        var movie = await _repository.GetMovieByIdAsync(1);

        Assert.IsNotNull(movie);
        Assert.AreEqual("Inception", movie?.Title);
    }

    [Test]
    public async Task GetMovieByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var movie = await _repository.GetMovieByIdAsync(999);

        Assert.IsNull(movie);
    }
}
