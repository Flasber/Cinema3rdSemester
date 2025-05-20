USE CinemaDB;
GO

-- Ryd eksisterende data i den rigtige r�kkef�lge
DELETE FROM BookingSeat;
DELETE FROM Booking;
DELETE FROM ScreeningSeat;
DELETE FROM Screening;     
DELETE FROM Seat;
DELETE FROM Auditorium;
DELETE FROM Movie;
GO

-- Tilf�j en testkunde
INSERT INTO Customer (Name, Email, MobileNumber, Address, CustomerType)
VALUES ('Test Bruger', 'test@example.com', '12345678', 'Testvej 1', 'Standard');
GO

-- Brug IDENTITY_INSERT for Movie
SET IDENTITY_INSERT Movie ON;
INSERT INTO Movie (Id, Title, Genre, Duration, Description, Language, AgeRating, PosterUrl)
VALUES
(1, 'Inception', 'Sci-Fi', 148, 'A mind-bending thriller about dreams within dreams.', 'English', 'PG-13', '/images/Inception.jpg'),
(2, 'Mac and Devin Go To High School', 'Comedy', 75, 'Two students bond over cannabis and friendship.', 'English', 'PG-18', '/images/MacAndDevin.jpg'),
(3, 'Minecraft Filmen', 'Adventure', 100, 'Et eventyr i en blokverden, hvor alt er muligt!', 'Danish', 'PG', '/images/Minecraft.jpg');
SET IDENTITY_INSERT Movie OFF;
GO

-- Res�t Movie IDENTITY
DBCC CHECKIDENT ('Movie', RESEED, 3);
GO

-- Tilf�j auditorier
INSERT INTO Auditorium (Id, Name, Capacity, Has3D, SoundSystem, ScreenSize)
VALUES
(1, 'Auditorium 1', 200, 1, 'Dolby Atmos', 'Large'),
(2, 'Auditorium 2', 150, 0, 'Stereo', 'Medium');
GO

-- Tilf�j forestillinger (uden Id - IDENTITY klarer det selv)
INSERT INTO Screening (MovieId, Date, Time, LanguageVersion, Is3D, IsSoldOut, SoundSystem, AuditoriumId)
VALUES
(2, CAST(GETDATE() AS DATE), '16:00:00', 'English', 0, 0, 'Stereo', 2),
(2, CAST(GETDATE() AS DATE), '20:00:00', 'English', 0, 1, 'Stereo', 2),
(1, CAST(DATEADD(day, 1, GETDATE()) AS DATE), '14:00:00', 'English', 1, 0, 'Dolby Atmos', 1),
(1, CAST(DATEADD(day, 1, GETDATE()) AS DATE), '17:00:00', 'English', 0, 0, 'Dolby Atmos', 1),
(3, CAST(DATEADD(day, 2, GETDATE()) AS DATE), '15:00:00', 'Danish', 0, 0, 'Stereo', 2),
(3, CAST(DATEADD(day, 2, GETDATE()) AS DATE), '18:30:00', 'Danish', 0, 0, 'Stereo', 1);
GO

-- Tilf�j s�der til begge auditorier
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

-- Opret ScreeningSeat records for alle screenings og s�der i tilh�rende auditorium
INSERT INTO ScreeningSeat (ScreeningId, SeatId, IsAvailable)
SELECT s.Id AS ScreeningId, st.Id AS SeatId, 1 AS IsAvailable
FROM Screening s
JOIN Seat st ON s.AuditoriumId = st.AuditoriumId;
GO
