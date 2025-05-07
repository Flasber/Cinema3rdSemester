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
        private readonly Dictionary<int, List<Seat>> _selectionsByBookingId = new();

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

            return seats
                .Where(seat => seat.IsAvailable)
                .Select(seat => new SeatAvailability
                {
                    SeatNumber = seat.SeatNumber,
                    Row = seat.Row,
                    IsAvailable = seat.IsAvailable
                })
                .ToList();
        }

        public bool SelectSeatForBooking(int bookingId, int seatNumber, string row, int auditoriumId)
        {
            var seat = _repository.GetSeat(seatNumber, row, auditoriumId);
            if (seat == null || !seat.IsAvailable)
                return false;

            if (_selectionsByBookingId.Values.Any(seats =>
                seats.Any(s => s.SeatNumber == seatNumber && s.Row == row && s.AuditoriumId == auditoriumId)))
            {
                return false;
            }

            seat.IsAvailable = false;
            seat.Version++;

            if (!_selectionsByBookingId.ContainsKey(bookingId))
                _selectionsByBookingId[bookingId] = new List<Seat>();

            _selectionsByBookingId[bookingId].Add(seat);
            return true;
        }



        public IEnumerable<Seat> GetSelectedSeats(int bookingId)
        {
            return _selectionsByBookingId.ContainsKey(bookingId)
                ? _selectionsByBookingId[bookingId]
                : Enumerable.Empty<Seat>();
        }
    }
}
