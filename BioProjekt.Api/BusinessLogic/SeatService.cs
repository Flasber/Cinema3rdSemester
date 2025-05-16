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

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<IEnumerable<SeatAvailability>> GetAvailableSeats(int screeningId)
        {
            var screeningSeats = await _seatRepository.GetAvailableSeatsForScreeningAsync(screeningId);

            return screeningSeats.Select(ss => new SeatAvailability
            {
                ScreeningSeatId = ss.Id,
                SeatNumber = ss.Seat.SeatNumber,
                Row = ss.Seat.Row,
                IsAvailable = ss.IsAvailable,
                Version = ss.Version
            });
        }

        public async Task<bool> TryReserveScreeningSeat(int screeningSeatId, byte[] clientVersion)
        {
            return await _seatRepository.TryReserveScreeningSeatAsync(screeningSeatId, clientVersion);
        }

        public async Task<bool> SelectSeats(Guid sessionId, List<int> screeningSeatIds)
        {
            var allSucceeded = true;

            foreach (var id in screeningSeatIds)
            {
                var seat = await _seatRepository.GetScreeningSeatByIdAsync(id);
                if (seat == null || !seat.IsAvailable)
                {
                    allSucceeded = false;
                    continue;
                }

                _seatRepository.StoreSeatSelection(sessionId, seat);
            }

            return allSucceeded;
        }

        public IEnumerable<ScreeningSeat> GetSelectedSeats(Guid sessionId)
        {
            return _seatRepository.GetSelectedSeatsAsync(sessionId).Result;
        }

        public async Task AssignSeatsToBooking(Guid sessionId, int bookingId)
        {
            var selected = _seatRepository.GetSelectedSeatsAsync(sessionId).Result.ToList();
            if (!selected.Any())
                return;

            await _seatRepository.AssignSeatsToBooking(sessionId, bookingId, selected);
            _seatRepository.ClearSeatSelection(sessionId);
        }
    }
}
