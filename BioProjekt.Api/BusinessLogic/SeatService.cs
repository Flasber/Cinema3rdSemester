using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.Shared.WebDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BioProjekt.Api.BusinessLogic
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly Dictionary<Guid, List<Seat>> _selectionsBySessionId = new();

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<IEnumerable<SeatAvailability>> GetAvailableSeats(int auditoriumId)
        {
            var seats = await _seatRepository.GetSeatsForAuditorium(auditoriumId);

            return seats
                .Where(seat => seat.IsAvailable)
                .Select(seat => new SeatAvailability
                {
                    SeatNumber = seat.SeatNumber,
                    Row = seat.Row,
                    IsAvailable = seat.IsAvailable,
                    Version = seat.Version,
                    AuditoriumId = seat.AuditoriumId
                });
        }

        public async Task<IEnumerable<Seat>> GetSeatsForAuditorium(int auditoriumId)
        {
            return await _seatRepository.GetSeatsForAuditorium(auditoriumId);
        }

        public async Task AddSeat(Seat seat)
        {
            await _seatRepository.AddSeat(seat);
        }

        public async Task<bool> TryReserveSeat(int seatNumber, string row, byte[] clientVersion, int auditoriumId)
        {
            return await _seatRepository.TryReserveSeat(seatNumber, row, clientVersion, auditoriumId);
        }

        public bool SelectSeat(Guid sessionId, int seatNumber, string row, int auditoriumId)
        {
            var seat = _seatRepository.GetSeat(seatNumber, row, auditoriumId).Result;
            if (seat == null || !seat.IsAvailable)
                return false;

            if (!_selectionsBySessionId.ContainsKey(sessionId))
                _selectionsBySessionId[sessionId] = new List<Seat>();

            if (_selectionsBySessionId[sessionId].Any(s =>
                s.SeatNumber == seatNumber &&
                s.Row == row &&
                s.AuditoriumId == auditoriumId))
            {
                return false;
            }

            seat.IsAvailable = false;
            _selectionsBySessionId[sessionId].Add(seat);
            return true;
        }

        public IEnumerable<Seat> GetSelectedSeats(Guid sessionId)
        {
            return _selectionsBySessionId.ContainsKey(sessionId)
                ? _selectionsBySessionId[sessionId]
                : Enumerable.Empty<Seat>();
        }

        public async Task AssignSeatsToBooking(Guid sessionId, int bookingId)
        {
            if (!_selectionsBySessionId.TryGetValue(sessionId, out var selectedSeats))
                return;

            await _seatRepository.AssignSeatsToBooking(sessionId, bookingId, selectedSeats);
            _selectionsBySessionId.Remove(sessionId);
        }
    }
}
