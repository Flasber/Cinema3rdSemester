using System.Data;
using Dapper;

namespace BioProjekt.DataAccess.Helpers
{
    public class DbCleaner
    {
        private readonly DbHelper _dbHelper;

        public DbCleaner(string connectionString)
        {
            _dbHelper = new DbHelper(connectionString);
        }

        public void CleanAndInsertTestData()
        {
            using var connection = _dbHelper.CreateConnection();
            connection.Open();

            connection.Execute("DELETE FROM BookingSeat");
            connection.Execute("DELETE FROM Booking");
            connection.Execute("DELETE FROM ScreeningSeat");
            connection.Execute("DELETE FROM Seat");
            connection.Execute("DELETE FROM Screening");
            connection.Execute("DELETE FROM Auditorium");
            connection.Execute("DELETE FROM Movie");
            connection.Execute("DELETE FROM Customer");

            // Customer kræver CustomerNumber, derfor SET IDENTITY_INSERT ON
           
            connection.Execute(@"
                INSERT INTO Customer (CustomerNumber, Name, Email, MobileNumber, Address, CustomerType)
                VALUES (1, 'Test Bruger', 'test@example.com', '12345678', 'Testvej 1', 'Standard')");
      

            var movie1Id = connection.QuerySingle<int>(@"
                INSERT INTO Movie (Title, Genre, Duration, Description, Language, AgeRating, PosterUrl)
                OUTPUT INSERTED.Id
                VALUES ('Inception', 'Sci-Fi', 148, 'A mind-bending thriller about dreams within dreams.', 'English', 'PG-13', '/images/Inception.jpg')");

            var movie2Id = connection.QuerySingle<int>(@"
                INSERT INTO Movie (Title, Genre, Duration, Description, Language, AgeRating, PosterUrl)
                OUTPUT INSERTED.Id
                VALUES ('Mac and Devin Go To High School', 'Comedy', 75, 'Two students bond over cannabis and friendship.', 'English', 'PG-18', '/images/MacAndDevin.jpg')");

            var aud1Id = connection.QuerySingle<int>(@"
                INSERT INTO Auditorium (Name, Capacity, Has3D, SoundSystem, ScreenSize)
                OUTPUT INSERTED.Id
                VALUES ('Auditorium 1', 200, 1, 'Dolby Atmos', 'Large')");

            var aud2Id = connection.QuerySingle<int>(@"
                INSERT INTO Auditorium (Name, Capacity, Has3D, SoundSystem, ScreenSize)
                OUTPUT INSERTED.Id
                VALUES ('Auditorium 2', 150, 0, 'Stereo', 'Medium')");

            var screening1Id = connection.QuerySingle<int>(@"
                INSERT INTO Screening (MovieId, Date, Time, LanguageVersion, Is3D, IsSoldOut, SoundSystem, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (@MovieId, '2023-08-15', '19:00:00', 'English', 1, 0, 'Dolby Atmos', @AudId)",
                new { MovieId = movie1Id, AudId = aud1Id });

            var screening2Id = connection.QuerySingle<int>(@"
                INSERT INTO Screening (MovieId, Date, Time, LanguageVersion, Is3D, IsSoldOut, SoundSystem, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (@MovieId, '2023-08-16', '18:00:00', 'English', 0, 0, 'Stereo', @AudId)",
                new { MovieId = movie2Id, AudId = aud2Id });

            var seat1Id = connection.QuerySingle<int>(@"
                INSERT INTO Seat (SeatNumber, Row, SeatType, IsAvailable, PriceModifier, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (1, 'A', 'Standard', 1, 1.00, @AudId)", new { AudId = aud1Id });

            var seat2Id = connection.QuerySingle<int>(@"
                INSERT INTO Seat (SeatNumber, Row, SeatType, IsAvailable, PriceModifier, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (2, 'A', 'VIP', 1, 1.50, @AudId)", new { AudId = aud1Id });

            var seat3Id = connection.QuerySingle<int>(@"
                INSERT INTO Seat (SeatNumber, Row, SeatType, IsAvailable, PriceModifier, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (3, 'B', 'Standard', 1, 1.00, @AudId)", new { AudId = aud2Id });

            connection.Execute(@"
                INSERT INTO ScreeningSeat (ScreeningId, SeatId, IsAvailable)
                VALUES 
                    (@Screening1, @Seat1, 1),
                    (@Screening1, @Seat2, 1),
                    (@Screening2, @Seat3, 1)",
                new
                {
                    Screening1 = screening1Id,
                    Screening2 = screening2Id,
                    Seat1 = seat1Id,
                    Seat2 = seat2Id,
                    Seat3 = seat3Id
                });
        }
    }
}
