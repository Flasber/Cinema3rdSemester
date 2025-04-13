using BioProjektModels;
using System;
using System.Collections.Generic;

namespace BioProjekt.Api.BusinessLogic
{
    public class MockTicketService : ITicketService
    {
        private static readonly List<Ticket> Tickets = new();
        private static int _id = 1;

        public Ticket CreateTicket(Ticket ticket)
        {
            ticket.Id = _id++;
            ticket.IssueDate = DateTime.Now;
            Tickets.Add(ticket);
            return ticket;
        }

        public List<Ticket> GetAllTickets()
        {
            return Tickets;
        }
    }
}
