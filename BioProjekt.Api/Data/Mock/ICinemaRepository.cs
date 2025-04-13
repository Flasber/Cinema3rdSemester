namespace BioProjekt.Api.Data.Mockdatabase
{
    public interface ICinemaRepository
    {
        IEnumerable<Movie> GetAllMovies();
        IEnumerable<Screening> GetScreeningsForMovie(int movieId);
        IEnumerable<Seat> GetAvailableSeats();
    }
}
