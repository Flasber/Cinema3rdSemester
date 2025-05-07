
namespace BioProjekt.Api.Dto.SeatDTO
{
    public class SeatReservationRequestDTO
    {
        public int SeatNumber { get; set; }
        public string Row { get; set; }
        public int ClientVersion { get; set; }
        public int AuditoriumId { get; set; }
    }
}
