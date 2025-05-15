//using System;
//using System.Collections.Generic;
//using System.Linq;
//using BioProjektModels;
//using BioProjekt.Api.Data.Mockdatabase;
//using BioProjekt.Api.Dto.BookingDTO;

//namespace BioProjekt.Api.Data.Mock
//{
//    public class MockCinemaRepository : ICinemaRepository
//    {
//        private readonly List<Movie> _movies;
//        private readonly List<Screening> _screenings;
//        private readonly List<Auditorium> _auditoriums;
//        private readonly List<Seat> _seats;
//        private readonly Dictionary<int, List<Seat>> _selectedSeatsByBookingId;

//        public MockCinemaRepository()
//        {
         

//            _movies = new List<Movie>
//            {
//                new Movie
//                {
//                    Id = 1,
//                    Title = "Inception",
//                    Genre = "Sci-Fi",
//                    Duration = TimeSpan.FromMinutes(148),
//                    Description = "A mind-bending thriller about dreams within dreams.",
//                    Language = "English",
//                    AgeRating = "PG-13",
//                    PosterUrl = "/images/Inception.jpg"
//                },
//                new Movie
//                {
//                    Id = 2,
//                    Title = "Mac and Devin Go To High School",
//                    Genre = "Comedy",
//                    Duration = TimeSpan.FromMinutes(75),
//                    Description = "Two students bond over cannabis and friendship.",
//                    Language = "English",
//                    AgeRating = "PG-18",
//                    PosterUrl = "/images/MacAndDevin.jpg"
//                },
//                new Movie
//                {
//                    Id = 3,
//                    Title = "Minecraft Filmen",
//                    Genre = "Adventure",
//                    Duration = TimeSpan.FromMinutes(100),
//                    Description = "Et eventyr i en blokverden, hvor alt er muligt!",
//                    Language = "English",
//                    AgeRating = "PG",
//                    PosterUrl = "/images/Minecraft.jpg"
//                }
//            };

//            _auditoriums = new List<Auditorium>
//            {
//                new Auditorium { Id = 1, Name = "Sal 1", Capacity = 100, Has3D = true, SoundSystem = "Dolby Atmos", ScreenSize = "16x9 meter" },
//                new Auditorium { Id = 2, Name = "Sal 2", Capacity = 80, Has3D = false, SoundSystem = "Stereo", ScreenSize = "12x7 meter" },
//                new Auditorium { Id = 3, Name = "Sal 3", Capacity = 40, Has3D = true, SoundSystem = "Dolby 7.1", ScreenSize = "10x5 meter" }
//            };

//            _screenings = new List<Screening>
//            {
//                new Screening
//                {
//                    Id = 1,
//                    MovieId = 1,
//                    Date = DateTime.Today,
//                    Time = DateTime.Today.AddHours(18),
//                    LanguageVersion = "English",
//                    Is3D = true,
//                    IsSoldOut = false,
//                    SoundSystem = "Dolby Atmos",
//                    AuditoriumId = 1
//                },
//                new Screening
//                {
//                    Id = 2,
//                    MovieId = 2,
//                    Date = DateTime.Today,
//                    Time = DateTime.Today.AddHours(20),
//                    LanguageVersion = "English",
//                    Is3D = false,
//                    IsSoldOut = false,
//                    SoundSystem = "Stereo",
//                    AuditoriumId = 2
//                },
//                new Screening
//                {
//                    Id = 3,
//                    MovieId = 3,
//                    Date = DateTime.Today,
//                    Time = DateTime.Today.AddHours(21),
//                    LanguageVersion = "English",
//                    Is3D = true,
//                    IsSoldOut = false,
//                    SoundSystem = "Dolby 7.1",
//                    AuditoriumId = 3
//                }
//            };

//            _seats = new List<Seat>
//            {
//                new Seat
//                {
//                    SeatNumber = 1,
//                    Row = "A",
//                    SeatType = "Standard",
//                    IsAvailable = true,
//                    PriceModifier = 1.0m,
//                    Version = 1,
//                    AuditoriumId = 1
//                },
//                new Seat
//                {
//                    SeatNumber = 2,
//                    Row = "A",
//                    SeatType = "VIP",
//                    IsAvailable = true,
//                    PriceModifier = 1.5m,
//                    Version = 1,
//                    AuditoriumId = 1
//                },
//                new Seat
//                {
//                    SeatNumber = 1,
//                    Row = "B",
//                    SeatType = "Standard",
//                    IsAvailable = true,
//                    PriceModifier = 1.0m,
//                    Version = 1,
//                    AuditoriumId = 2
//                }
//            };
//        }

//        public IEnumerable<Movie> GetAllMovies()
//        {
//            return _movies;
//        }

//        public Movie GetMovieById(int id)
//        {
//            return _movies.FirstOrDefault(m => m.Id == id);
//        }

//        public IEnumerable<Screening> GetAllScreenings()
//        {
//            return _screenings;
//        }

//        public IEnumerable<Screening> GetScreeningsForMovie(int movieId)
//        {
//            return _screenings.Where(s => s.MovieId == movieId);
//        }

//        public IEnumerable<Auditorium> GetAllAuditoriums()
//        {
//            return _auditoriums;
//        }

//        public IEnumerable<Seat> GetSeatsForAuditorium(int auditoriumId)
//        {
//            return _seats.Where(s => s.AuditoriumId == auditoriumId);
//        }

//        public void AddSeat(Seat seat)
//        {
//            _seats.Add(seat);
//        }

//        public Seat GetSeat(int seatNumber, string row, int auditoriumId)
//        {
//            return _seats.FirstOrDefault(s =>
//                s.SeatNumber == seatNumber &&
//                s.Row == row &&
//                s.AuditoriumId == auditoriumId);
//        }
//        public void SelectSeatForBooking(int bookingId, int seatNumber, string row, int auditoriumId)
//        {
//            var seat = GetSeat(seatNumber, row, auditoriumId);
//            if (seat != null)
//            {
//                if (!_selectedSeatsByBookingId.ContainsKey(bookingId))
//                {
//                    _selectedSeatsByBookingId[bookingId] = new List<Seat>();
//                }

//                _selectedSeatsByBookingId[bookingId].Add(seat);
//                seat.IsAvailable = false;
//            }
//        }

//        public IEnumerable<Seat> GetSelectedSeats(int bookingId)
//        {
//            return _selectedSeatsByBookingId.ContainsKey(bookingId)
//                ? _selectedSeatsByBookingId[bookingId]
//                : Enumerable.Empty<Seat>();
//        }

//        public IEnumerable<Seat> GetAvailableSeats(int auditoriumId)
//        {
//            return _seats.Where(s => s.AuditoriumId == auditoriumId && s.IsAvailable);
//        }
      


//    }
//}