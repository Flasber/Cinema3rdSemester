using BioProjektModels;
using BioProjekt.DataAccess.Interfaces;
using BioProjekt.Api.Storage;
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
        private readonly SeatSelectionStore _store;

        public SeatService(ISeatRepository seatRepository, SeatSelectionStore store)
        {
            _seatRepository = seatRepository;
            _store = store;
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

            var existing = _store.GetSeats(sessionId);
            if (existing.Any(s => s.SeatNumber == seatNumber && s.Row == row && s.AuditoriumId == auditoriumId))
                return false;

            seat.IsAvailable = false;
            _store.AddSeat(sessionId, seat);
            return true;
        }

        public IEnumerable<Seat> GetSelectedSeats(Guid sessionId)
        {
            return _store.GetSeats(sessionId);
        }

        public async Task AssignSeatsToBooking(Guid sessionId, int bookingId)
        {
            var selectedSeats = _store.GetSeats(sessionId);
            if (!selectedSeats.Any())
                return;

            await _seatRepository.AssignSeatsToBooking(sessionId, bookingId, selectedSeats);
            _store.Clear(sessionId);
        }
    }
}
