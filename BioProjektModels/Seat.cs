using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioProjektModels
{
    public class Seat
    {
        public int Id { get; set; }       
        public int SeatNumber { get; set; }
        public string SeatType { get; set; }
        public string Row { get; set; }
        public int AuditoriumId { get; set; }
        public decimal PriceModifier { get; set; }
        public bool IsAvailable { get; set; }
        public byte[] Version { get; set; }  
    }

}