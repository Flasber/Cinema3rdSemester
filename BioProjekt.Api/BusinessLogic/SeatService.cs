using System.Collections.Generic;
using System.Linq;
using BioProjektModels;

namespace BioProjekt.Api.BusinessLogic
{
    public class SeatService
    {
        private readonly List<Seat> _seats = new();

        public void AddSeat(Seat seat)
        {
            _seats.Add(seat);
        }


        public bool TryReserveSeat(int seatNumber, string row, int clientVersion, int auditoriumId)
        {
            var seat = _seats.FirstOrDefault(s =>
                s.SeatNumber == seatNumber &&
                s.Row == row &&
                s.AuditoriumId == auditoriumId);

            if (seat == null || !seat.IsAvailable || seat.Version != clientVersion)
                return false;

            seat.IsAvailable = false;
            seat.Version++;
            return true;
        }
    }
}
