USE CinemaDB;
GO

DELETE FROM BookingSeat;
DELETE FROM Booking;
DELETE FROM ScreeningSeat;
DELETE FROM Screening;
DELETE FROM Seat;
DELETE FROM Auditorium;
DELETE FROM Movie;
GO

INSERT INTO Customer (Name, Email, MobileNumber, Address, CustomerType)
VALUES ('Test Bruger', 'test@example.com', '12345678', 'Testvej 1', 'Standard');
GO

SET IDENTITY_INSERT Movie ON;
INSERT INTO Movie (Id, Title, Genre, Duration, Description, Language, AgeRating, PosterUrl)
VALUES
(1, 'Inception', 'Sci-Fi', 148, 'A mind-bending thriller about dreams within dreams.', 'English', 'PG-13', '/images/Inception.jpg'),
(2, 'Mac and Devin Go To High School', 'Comedy', 75, 'Two students bond over cannabis and friendship.', 'English', 'PG-18', '/images/MacAndDevin.jpg'),
(3, 'Minecraft Filmen', 'Adventure', 100, 'Et eventyr i en blokverden, hvor alt er muligt!', 'Danish', 'PG', '/images/Minecraft.jpg');
SET IDENTITY_INSERT Movie OFF;
GO

DBCC CHECKIDENT ('Movie', RESEED, 3);
GO

INSERT INTO Auditorium (Id, Name, Capacity, Has3D, SoundSystem, ScreenSize)
VALUES
(1, 'Auditorium 1', 200, 1, 'Dolby Atmos', 'Large'),
(2, 'Auditorium 2', 150, 0, 'Stereo', 'Medium');
GO

INSERT INTO Screening (MovieId, Date, Time, LanguageVersion, Is3D, IsSoldOut, SoundSystem, AuditoriumId)
VALUES
(2, CAST(GETDATE() AS DATE), '16:00:00', 'English', 0, 0, 'Stereo', 2),
(2, CAST(GETDATE() AS DATE), '20:00:00', 'English', 0, 1, 'Stereo', 2),
(1, CAST(DATEADD(day, 1, GETDATE()) AS DATE), '14:00:00', 'English', 1, 0, 'Dolby Atmos', 1),
(1, CAST(DATEADD(day, 1, GETDATE()) AS DATE), '17:00:00', 'English', 0, 0, 'Dolby Atmos', 1),
(3, CAST(DATEADD(day, 2, GETDATE()) AS DATE), '15:00:00', 'Danish', 0, 0, 'Stereo', 2),
(3, CAST(DATEADD(day, 2, GETDATE()) AS DATE), '18:30:00', 'Danish', 0, 0, 'Stereo', 1);
GO

DECLARE @audId INT = 1;
WHILE @audId <= 2
BEGIN
    DECLARE @currentRow CHAR(1) = 'A';
    WHILE ASCII(@currentRow) <= ASCII('F')
    BEGIN
        DECLARE @currentSeat INT = 1;
        WHILE @currentSeat <= 10
        BEGIN
            INSERT INTO Seat (SeatNumber, Row, SeatType, IsAvailable, PriceModifier, AuditoriumId)
            VALUES (
                @currentSeat,
                @currentRow,
                CASE WHEN @currentSeat % 5 = 0 THEN 'VIP' ELSE 'Standard' END,
                1,
                CASE WHEN @currentSeat % 5 = 0 THEN 1.5 ELSE 1.0 END,
                @audId
            );
            SET @currentSeat += 1;
        END
        SET @currentRow = CHAR(ASCII(@currentRow) + 1);
    END
    SET @audId += 1;
END
GO

INSERT INTO ScreeningSeat (ScreeningId, SeatId, IsAvailable)
SELECT s.Id, st.Id, 1
FROM Screening s
JOIN Seat st ON s.AuditoriumId = st.AuditoriumId;
GO
