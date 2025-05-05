using NUnit.Framework;
using BioProjekt.Api.BusinessLogic;
using BioProjektModels;

namespace BioProjekt.Tests
{
    [TestFixture]
    public class SeatServiceTests
    {
        private SeatService _seatService;

        [SetUp]
        public void Setup()
        {
            _seatService = new SeatService();
            _seatService.AddSeat(new Seat
            {
                SeatNumber = 1,
                Row = "A",
                SeatType = "Standard",
                IsAvailable = true,
                PriceModifier = 1.0m,
                Version = 1,
                AuditoriumId = 1
            });
        }

        [Test]
        public void TryReserveSeat_ShouldReserveSeat_WhenAvailableAndVersionMatches()
        {
            var result = _seatService.TryReserveSeat(1, "A", 1, 1);
            Assert.IsTrue(result);
        }

        [Test]
        public void TryReserveSeat_ShouldFail_WhenVersionIsIncorrect()
        {
            var result = _seatService.TryReserveSeat(1, "A", clientVersion: 2, auditoriumId: 1);
            Assert.IsFalse(result);
        }
        [Test]
        public void TryReserveSeat_ShouldFail_WhenSeatIsAlreadyReserved()
        {
            var firstAttempt = _seatService.TryReserveSeat(1, "A", 1, 1);
            Assert.IsTrue(firstAttempt);

            var secondAttempt = _seatService.TryReserveSeat(1, "A", 1, 1);
            Assert.IsFalse(secondAttempt);
        }
    }
}
