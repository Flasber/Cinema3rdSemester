using BioProjektModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjektModels.Interfaces
{
    public interface ISqlCinemaRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<IEnumerable<Auditorium>> GetAllAuditoriums();
        Task<IEnumerable<Screening>> GetAllScreenings();
        Task<IEnumerable<Seat>> GetSeatsForAuditorium(int auditoriumId);
        Task AddSeat(Seat seat);
        Task<Seat?> GetSeat(int seatNumber, string row, int auditoriumId);
        Task<bool> TryReserveSeat(int seatNumber, string row, byte[] clientVersion, int auditoriumId);
    }
}
