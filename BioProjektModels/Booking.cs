using System;
namespace BioProjektModels
{
    public class Booking
    {
        public int Id { get; set; }
        public string BookingType { get; set; }
        public decimal  Price { get; set; }
        public bool IsDiscounted { get; set; }
        public DateTime BookingDate { get; set; }
        }
    }

