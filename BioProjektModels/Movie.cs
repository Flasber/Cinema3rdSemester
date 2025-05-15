using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioProjektModels
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }  // Duration gemmes som int (antal minutter)
        public string Description { get; set; }
        public string Language { get; set; }
        public string AgeRating { get; set; }
        public string PosterUrl { get; set; }

        // Beregnet egenskab for at konvertere int (minutter) til TimeSpan
        public TimeSpan DurationAsTimeSpan => TimeSpan.FromMinutes(Duration);
    }

}

