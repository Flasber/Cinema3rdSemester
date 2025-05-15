using System.Data;
using Dapper;
using BioProjekt.DataAccess.Helpers;

namespace BioProjekt.DataAccess.Helpers;

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
        connection.Execute("DELETE FROM Seat");
        connection.Execute("DELETE FROM Screening");
        connection.Execute("DELETE FROM Auditorium");
        connection.Execute("DELETE FROM Movie");
        connection.Execute("DELETE FROM Customer");

        connection.Execute(@"
            INSERT INTO Customer (CustomerNumber, Name, Email, MobileNumber, Address, CustomerType)
            VALUES (1, 'Test Bruger', 'test@example.com', '12345678', 'Testvej 1', 'Standard')
        ");

        connection.Execute(@"
            INSERT INTO Movie (Id, Title, Genre, Duration, Description, Language, AgeRating, PosterUrl)
            VALUES
            (1, 'Inception', 'Sci-Fi', 148, 'A mind-bending thriller about dreams within dreams.', 'English', 'PG-13', '/images/Inception.jpg'),
            (2, 'Mac and Devin Go To High School', 'Comedy', 75, 'Two students bond over cannabis and friendship.', 'English', 'PG-18', '/images/MacAndDevin.jpg'),
            (3, 'Minecraft Filmen', 'Adventure', 100, 'Et eventyr i en blokverden, hvor alt er muligt!', 'English', 'PG', '/images/Minecraft.jpg')
        ");

        connection.Execute(@"
            INSERT INTO Auditorium (Id, Name, Capacity, Has3D, SoundSystem, ScreenSize)
            VALUES
            (1, 'Auditorium 1', 200, 1, 'Dolby Atmos', 'Large'),
            (2, 'Auditorium 2', 150, 0, 'Stereo', 'Medium')
        ");

        connection.Execute(@"
            INSERT INTO Screening (Id, MovieId, Date, Time, LanguageVersion, Is3D, IsSoldOut, SoundSystem, AuditoriumId)
            VALUES
            (1, 1, '2023-08-15', '19:00:00', 'English', 1, 0, 'Dolby Atmos', 1),
            (2, 2, '2023-08-16', '18:00:00', 'English', 0, 1, 'Stereo', 2)
        ");

        connection.Execute(@"
            INSERT INTO Seat (SeatNumber, Row, SeatType, IsAvailable, PriceModifier, AuditoriumId)
            VALUES
            (1, 'A', 'Standard', 1, 1.00, 1),
            (2, 'A', 'VIP', 1, 1.50, 1),
            (3, 'B', 'Standard', 1, 1.00, 2)
        ");
    }
}
