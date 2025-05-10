using BioProjektModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioProjektModels.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();  
        Task<Movie?> GetMovieByIdAsync(int id);  
    }
}
