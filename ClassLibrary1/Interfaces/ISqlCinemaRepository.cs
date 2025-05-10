namespace BioProjektModels.Interfaces
{
    public interface ISqlCinemaRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(int id);
        // Flere metoder her til Screening, Booking osv.
    }
}
