using Microsoft.AspNetCore.Mvc;
using BioProjektModels;
using BioProjekt.Api.BusinessLogic;

namespace BioProjekt.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {

        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [HttpGet]
        public ActionResult<List<Ticket>> GetAllTickets()
        {
            return _ticketService.GetAllTickets();
        }

        [HttpPost]
        public ActionResult<Ticket> CreateTicket([FromBody] Ticket ticket)
        {
            var created = _ticketService.CreateTicket(ticket);
            return Created("", created);
        }
    }
}
