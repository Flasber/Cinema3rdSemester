using BioProjektModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BioProjekt.Api.Storage
{
    public class SeatSelectionStore
    {
        private readonly Dictionary<Guid, List<SeatSelectionEntry>> _selections = new();
        private const int SelectionTimeoutMinutes = 1;
        private readonly Timer _cleanupTimer;

        private class SeatSelectionEntry
        {
            public ScreeningSeat ScreeningSeat { get; set; }
            public DateTime SelectedAt { get; set; }
        }

        public SeatSelectionStore()
        {
            _cleanupTimer = new Timer(CleanupAll, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }

        public void AddSeat(Guid sessionId, ScreeningSeat screeningSeat)
        {
            CleanupOldSelections(sessionId);

            if (!_selections.ContainsKey(sessionId))
                _selections[sessionId] = new List<SeatSelectionEntry>();

            if (_selections[sessionId].Any(entry => entry.ScreeningSeat.Id == screeningSeat.Id))
            {
                throw new InvalidOperationException("Dette sæde er allerede valgt.");
            }

            _selections[sessionId].Add(new SeatSelectionEntry
            {
                ScreeningSeat = screeningSeat,
                SelectedAt = DateTime.UtcNow
            });
        }


        public List<ScreeningSeat> GetSeats(Guid sessionId)
        {
            CleanupOldSelections(sessionId);

            if (_selections.TryGetValue(sessionId, out var list))
                return list.Select(entry => entry.ScreeningSeat).ToList();

            return new List<ScreeningSeat>();
        }

        public void Clear(Guid sessionId)
        {
            _selections.Remove(sessionId);
        }

        private void CleanupOldSelections(Guid sessionId)
        {
            if (!_selections.ContainsKey(sessionId))
                return;

            var cutoff = DateTime.UtcNow.AddMinutes(-SelectionTimeoutMinutes);
            _selections[sessionId] = _selections[sessionId]
                .Where(entry => entry.SelectedAt >= cutoff)
                .ToList();

            if (_selections[sessionId].Count == 0)
                _selections.Remove(sessionId);
        }

        private void CleanupAll(object? state)
        {
            var now = DateTime.UtcNow;
            var cutoff = now.AddMinutes(-SelectionTimeoutMinutes);

            var expiredSessions = _selections
                .Where(kv => kv.Value.All(entry => entry.SelectedAt < cutoff))
                .Select(kv => kv.Key)
                .ToList();

            foreach (var sessionId in expiredSessions)
            {
                _selections.Remove(sessionId);
            }
        }
    }
}
