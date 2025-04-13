using BioProjektModels;
using System.Collections.Generic;

namespace BioProjekt.Api.BusinessLogic
{
    public interface ITicketService
    {
        Ticket CreateTicket(Ticket ticket);
        List<Ticket> GetAllTickets();
    }
}
