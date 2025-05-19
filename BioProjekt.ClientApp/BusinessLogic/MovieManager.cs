using System;
using System.IO;
using System.Threading.Tasks;
using BioProjekt.ClientApp.Forms.Services;
using BioProjekt.Shared.ClientDtos;

namespace BioProjekt.ClientApp.BusinessLogic
{
    public class MovieManager
    {
        private readonly MovieApiClient _apiClient;
        private readonly string _posterStoragePath;

        public MovieManager()
        {
            _apiClient = new MovieApiClient();
            _posterStoragePath = ResolveMvcWwwrootImagesPath();
        }

        public async Task<bool> CreateMovieWithPosterAsync(MovieCreateDto movie, string posterFilePath)
        {
            var copiedPosterUrl = CopyPosterToMvcFolder(posterFilePath);
            if (copiedPosterUrl == null)
                return false;

            movie.PosterUrl = copiedPosterUrl;
            return await _apiClient.CreateMovieAsync(movie);
        }

        private string? CopyPosterToMvcFolder(string originalFilePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(originalFilePath) || !File.Exists(originalFilePath))
                    return null;

                var fileName = Guid.NewGuid() + Path.GetExtension(originalFilePath);
                Directory.CreateDirectory(_posterStoragePath);

                var destinationPath = Path.Combine(_posterStoragePath, fileName);
                File.Copy(originalFilePath, destinationPath, true);

                return "/Images/" + fileName;
            }
            catch
            {
                return null;
            }
        }

        private string ResolveMvcWwwrootImagesPath()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var relativePath = Path.Combine(baseDir, @"..\..\..\..\..\BioProjekt.Web\wwwroot\Images");
            var fullPath = Path.GetFullPath(relativePath);
            return fullPath;
        }
    }
}
