//using NUnit.Framework;
//using BioProjekt.Api.BusinessLogic;
//using BioProjekt.Api.Data.Mock;
//using BioProjekt.Api.Data.Mockdatabase;
//using BioProjektModels;

//namespace BioProjektTest.Mock
//{
//    [TestFixture]
//    public class MovieServiceTests
//    {
//        //private IMovieService _movieService;

//        [SetUp]
//        public void Setup()
//        {
//            ICinemaRepository mockRepo = new MockCinemaRepository();
//            _movieService = new MovieService(mockRepo);
//        }

//        [Test]
//        public void GetMovieById_ShouldReturnCorrectMovie()
//        {
//            var movie = _movieService.GetMovieById(1);
//            Assert.NotNull(movie);
//            Assert.AreEqual("Inception", movie.Title);
//        }

//        [Test]
//        public void GetMoviesByGenre_ShouldReturnFilteredMovies()
//        {
//            var comedyMovies = _movieService.GetMoviesByGenre("Comedy");
//            Assert.That(comedyMovies, Has.Exactly(1).Items);
//            Assert.AreEqual("Mac and Devin Go To High School", comedyMovies[0].Title);
//        }

//        [Test]
//        public void GetMovieDescription_ShouldReturnCorrectDescription()
//        {
//            var description = _movieService.GetMovieDescription(3);
//            Assert.AreEqual("Et eventyr i en blokverden, hvor alt er muligt!", description);
//        }

//        [Test]
//        public void GetMovieDuration_ShouldReturnCorrectDuration()
//        {
//            var duration = _movieService.GetMovieDuration(2);
//            Assert.AreEqual(TimeSpan.FromMinutes(75), duration);
//        }
//    }
//}
