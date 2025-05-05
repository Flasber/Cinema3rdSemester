using System.Collections.Generic;
using System.Linq;
using BioProjekt.Api.Data.Mockdatabase;
using BioProjektModels;
using BioProjekt.Api.Dto.SeatDTO;


namespace BioProjekt.Api.BusinessLogic
{
    public class SeatService : ISeatService
    {
        private readonly ICinemaRepository _repository;

        public SeatService(ICinemaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Seat> GetSeatsForAuditorium(int auditoriumId)
        {
            return _repository.GetSeatsForAuditorium(auditoriumId);
        }

        public void AddSeat(Seat seat)
        {
            _repository.AddSeat(seat);
        }

        public bool TryReserveSeat(int seatNumber, string row, int clientVersion, int auditoriumId)
        {
            var seat = _repository.GetSeat(seatNumber, row, auditoriumId);

            if (seat == null || !seat.IsAvailable || seat.Version != clientVersion)
                return false;

            seat.IsAvailable = false;
            seat.Version++;
            return true;
        }

        public IEnumerable<SeatAvailability> GetAvailableSeats(int auditoriumId)
        {
            var seats = _repository.GetSeatsForAuditorium(auditoriumId);

            if (seats == null || !seats.Any())
                return Enumerable.Empty<SeatAvailability>();

            var availableSeats = seats
                .Where(seat => seat.IsAvailable)
                .Select(seat => new SeatAvailability
                {
                    SeatNumber = seat.SeatNumber,
                    Row = seat.Row,
                    IsAvailable = seat.IsAvailable
                })
                .ToList();

            return availableSeats;
        }

    }
}
