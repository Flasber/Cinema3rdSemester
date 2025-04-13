using System;
namespace BioProjektModels
{
    public class Ticket
    {
        public int Id { get; set; }
        public string TicketType { get; set; }
        public decimal  Price { get; set; }
        public bool IsDiscounted { get; set; }
        public DateTime IssueDate { get; set; }
        }
    }

