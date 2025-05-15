using BioProjekt.Shared.WebDtos;

namespace BioProjekt.Api.Storage
{
    public class SeatSelectionStore
    {
        private readonly Dictionary<Guid, List<SeatSelectionDTO>> _store = new();

        public void AddSelection(Guid sessionId, SeatSelectionDTO dto)
        {
            if (!_store.ContainsKey(sessionId))
                _store[sessionId] = new List<SeatSelectionDTO>();

            _store[sessionId].Add(dto);
        }

        public List<SeatSelectionDTO> GetSelections(Guid sessionId)
        {
            return _store.TryGetValue(sessionId, out var list) ? list : new List<SeatSelectionDTO>();
        }
    }
}
