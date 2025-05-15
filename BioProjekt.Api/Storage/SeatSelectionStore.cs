using BioProjektModels;

namespace BioProjekt.Api.Storage
{
    public class SeatSelectionStore
    {
        private readonly Dictionary<Guid, List<Seat>> _selections = new();

        public void AddSeat(Guid sessionId, Seat seat)
        {
            if (!_selections.ContainsKey(sessionId))
                _selections[sessionId] = new List<Seat>();

            _selections[sessionId].Add(seat);
        }

        public List<Seat> GetSeats(Guid sessionId)
        {
            return _selections.TryGetValue(sessionId, out var list)
                ? list
                : new List<Seat>();
        }

        public void Clear(Guid sessionId)
        {
            _selections.Remove(sessionId);
        }
    }
}
