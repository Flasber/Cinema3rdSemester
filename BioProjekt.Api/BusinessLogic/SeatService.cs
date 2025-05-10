using BioProjekt.Api.Dto.SeatDTO;
using BioProjektModels;
using BioProjektModels.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BioProjekt.Api.BusinessLogic
{
    public class SeatService : ISeatService
    {
        private readonly ISqlCinemaRepository _repository;
        private readonly Dictionary<int, List<Seat>> _selectionsByBookingId = new();

        public SeatService(ISqlCinemaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Seat> GetSeatsForAuditorium(int auditoriumId)
        {
            return _repository.GetSeatsForAuditorium(auditoriumId).Result;
        }

        public void AddSeat(Seat seat)
        {
            _repository.AddSeat(seat).Wait();
        }

        public bool TryReserveSeat(int seatNumber, string row, byte[] clientVersion, int auditoriumId)
        {
            return _repository.TryReserveSeat(seatNumber, row, clientVersion, auditoriumId).Result;
        }

        public IEnumerable<SeatAvailability> GetAvailableSeats(int auditoriumId)
        {
            var seats = _repository.GetSeatsForAuditorium(auditoriumId).Result;

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
            var seat = _repository.GetSeat(seatNumber, row, auditoriumId).Result;
            if (seat == null || !seat.IsAvailable)
                return false;

            if (_selectionsByBookingId.Values.Any(seats =>
                seats.Any(s => s.SeatNumber == seatNumber && s.Row == row && s.AuditoriumId == auditoriumId)))
            {
                return false;
            }

            seat.IsAvailable = false;

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
