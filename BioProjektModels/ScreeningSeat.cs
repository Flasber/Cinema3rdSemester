using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioProjektModels
{
    public class ScreeningSeat
    {
        public int Id { get; set; }
        public int ScreeningId { get; set; }
        public int SeatId { get; set; }
        public bool IsAvailable { get; set; }
        public byte[] Version { get; set; }

        public Seat? Seat { get; set; }
    }
}
