using NUnit.Framework;
using BioProjekt.Api.BusinessLogic;
using BioProjekt.Api.Data.Mock;
using BioProjekt.Api.Data.Mockdatabase;
using BioProjektModels;
using System;
using System.Linq;

namespace BioProjekt.Tests
{
    [TestFixture]
    public class ScreeningServiceTests
    {
        private IScreeningService _screeningService;

        [SetUp]
        public void Setup()
        {
            ICinemaRepository mockRepo = new MockCinemaRepository();
            _screeningService = new ScreeningService(mockRepo);
        }

        [Test]
        public void GetAllScreenings_ShouldReturnDefaultMockScreenings()
        {
            var screenings = _screeningService.GetAllScreenings();
            Assert.IsNotNull(screenings);
            Assert.That(screenings.Count, Is.GreaterThanOrEqualTo(3));
        }

        [Test]
        public void AddScreening_ShouldIncreaseScreeningCount()
        {
            var beforeCount = _screeningService.GetAllScreenings().Count;

            var newScreening = new Screening
            {
                MovieId = 1,
                Date = DateTime.Today.AddDays(1),
                Time = DateTime.Today.AddDays(1).AddHours(17),
                LanguageVersion = "Danish",
                Is3D = false,
                IsSoldOut = false,
                SoundSystem = "Stereo",
                AuditoriumId = 2
            };

            _screeningService.AddScreening(newScreening);

            var afterCount = _screeningService.GetAllScreenings().Count;

            Assert.That(afterCount, Is.EqualTo(beforeCount + 1));
        }
    }
}
