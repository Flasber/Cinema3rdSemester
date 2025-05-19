using BioProjekt.DataAccess.Helpers;
using System.Data;
using Dapper;
public class DbCleaner
{
    private readonly DbHelper _dbHelper;

    public DbCleaner(string connectionString)
    {
        _dbHelper = new DbHelper(connectionString);
    }

    public record TestIds(
        int Screening1Id,
        int Screening2Id,
        int Seat1Id,
        int Seat2Id,
        int Seat3Id,
        int CustomerNumber,
        int Movie1Id,
        int Movie2Id
    );

    public TestIds CleanAndInsertTestData()
    {
        using var connection = _dbHelper.CreateConnection();
        connection.Open();

        EnsureScreeningSeatTableExists(connection);

        connection.Execute("DELETE FROM BookingSeat");
        connection.Execute("DELETE FROM Booking");
        connection.Execute("DELETE FROM ScreeningSeat");
        connection.Execute("DELETE FROM Seat");
        connection.Execute("DELETE FROM Screening");
        connection.Execute("DELETE FROM Auditorium");
        connection.Execute("DELETE FROM Movie");
        connection.Execute("DELETE FROM Customer");

        connection.Execute(@"
                INSERT INTO Customer (Name, Email, MobileNumber, Address, CustomerType)
                VALUES ('Test Bruger', 'test@example.com', '12345678', 'Testvej 1', 'Standard')");

        int customerNumber = connection.QuerySingle<int>("SELECT TOP 1 CustomerNumber FROM Customer");
        Console.WriteLine("CustomerNumber inserted = " + customerNumber);

        var movie1Id = connection.QuerySingle<int>(@"
                INSERT INTO Movie (Title, Genre, Duration, Description, Language, AgeRating, PosterUrl)
                OUTPUT INSERTED.Id
                VALUES ('Inception', 'Sci-Fi', 148, 'A mind-bending thriller about dreams within dreams.', 'English', 'PG-13', '/images/Inception.jpg')");
        Console.WriteLine("Movie1 ID = " + movie1Id);

        var movie2Id = connection.QuerySingle<int>(@"
                INSERT INTO Movie (Title, Genre, Duration, Description, Language, AgeRating, PosterUrl)
                OUTPUT INSERTED.Id
                VALUES ('Mac and Devin Go To High School', 'Comedy', 75, 'Two students bond over cannabis and friendship.', 'English', 'PG-18', '/images/MacAndDevin.jpg')");
        Console.WriteLine("Movie2 ID = " + movie2Id);

        connection.Execute(@"
                INSERT INTO Auditorium (Id, Name, Capacity, Has3D, SoundSystem, ScreenSize)
                VALUES 
                    (1, 'Auditorium 1', 200, 1, 'Dolby Atmos', 'Large'),
                    (2, 'Auditorium 2', 150, 0, 'Stereo', 'Medium')");

        var screening1Id = connection.QuerySingle<int>(@"
                INSERT INTO Screening (MovieId, Date, Time, LanguageVersion, Is3D, IsSoldOut, SoundSystem, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (@MovieId, '2023-08-15', '19:00:00', 'English', 1, 0, 'Dolby Atmos', 1)",
            new { MovieId = movie1Id });
        Console.WriteLine("Screening1 ID = " + screening1Id);

        var screening2Id = connection.QuerySingle<int>(@"
                INSERT INTO Screening (MovieId, Date, Time, LanguageVersion, Is3D, IsSoldOut, SoundSystem, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (@MovieId, '2023-08-16', '18:00:00', 'English', 0, 0, 'Stereo', 2)",
            new { MovieId = movie2Id });
        Console.WriteLine("Screening2 ID = " + screening2Id);

        var seat1Id = connection.QuerySingle<int>(@"
                INSERT INTO Seat (SeatNumber, Row, SeatType, IsAvailable, PriceModifier, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (1, 'A', 'Standard', 1, 1.00, 1)");
        var seat2Id = connection.QuerySingle<int>(@"
                INSERT INTO Seat (SeatNumber, Row, SeatType, IsAvailable, PriceModifier, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (2, 'A', 'VIP', 1, 1.50, 1)");
        var seat3Id = connection.QuerySingle<int>(@"
                INSERT INTO Seat (SeatNumber, Row, SeatType, IsAvailable, PriceModifier, AuditoriumId)
                OUTPUT INSERTED.Id
                VALUES (3, 'B', 'Standard', 1, 1.00, 2)");

        Console.WriteLine($"Seats inserted: {seat1Id}, {seat2Id}, {seat3Id}");

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

        return new TestIds(
            screening1Id, screening2Id,
            seat1Id, seat2Id, seat3Id,
            customerNumber, movie1Id, movie2Id
        );
    }

    private void EnsureScreeningSeatTableExists(IDbConnection connection)
    {
        var exists = connection.QuerySingle<int>(@"
                SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_NAME = 'ScreeningSeat'") > 0;

        if (!exists)
        {
            connection.Execute(@"
                    CREATE TABLE ScreeningSeat (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        ScreeningId INT NOT NULL,
                        SeatId INT NOT NULL,
                        IsAvailable BIT NOT NULL,
                        Version ROWVERSION,
                        FOREIGN KEY (ScreeningId) REFERENCES Screening(Id),
                        FOREIGN KEY (SeatId) REFERENCES Seat(Id)
                    )");
        }
    }
}
