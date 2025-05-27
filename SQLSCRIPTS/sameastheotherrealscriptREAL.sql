USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'CinemaDB')
BEGIN
    ALTER DATABASE CinemaDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CinemaDB;
END
GO

CREATE DATABASE CinemaDB;
GO

USE CinemaDB;
GO

CREATE TABLE Customer (
    CustomerNumber INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    MobileNumber NVARCHAR(20),
    Address NVARCHAR(255),
    CustomerType NVARCHAR(50)
);

CREATE TABLE Movie (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Genre NVARCHAR(50),
    Duration INT,
    Description NVARCHAR(MAX),
    Language NVARCHAR(50),
    AgeRating NVARCHAR(20),
    PosterUrl NVARCHAR(255)
);

CREATE TABLE Auditorium (
    Id INT PRIMARY KEY,
    Name NVARCHAR(100),
    Capacity INT,
    Has3D BIT,
    SoundSystem NVARCHAR(50),
    ScreenSize NVARCHAR(50)
);

CREATE TABLE Seat (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SeatNumber INT NOT NULL,
    Row CHAR(1) NOT NULL,
    SeatType NVARCHAR(20),
    IsAvailable BIT,
    PriceModifier FLOAT,
    AuditoriumId INT NOT NULL,
    FOREIGN KEY (AuditoriumId) REFERENCES Auditorium(Id)
);

CREATE TABLE Screening (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MovieId INT NOT NULL,
    Date DATE NOT NULL,
    Time TIME NOT NULL,
    LanguageVersion NVARCHAR(50),
    Is3D BIT,
    IsSoldOut BIT,
    SoundSystem NVARCHAR(50),
    AuditoriumId INT NOT NULL,
    FOREIGN KEY (MovieId) REFERENCES Movie(Id),
    FOREIGN KEY (AuditoriumId) REFERENCES Auditorium(Id)
);

CREATE TABLE ScreeningSeat (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ScreeningId INT NOT NULL,
    SeatId INT NOT NULL,
    IsAvailable BIT NOT NULL,
    Version ROWVERSION,
    FOREIGN KEY (ScreeningId) REFERENCES Screening(Id),
    FOREIGN KEY (SeatId) REFERENCES Seat(Id)
);

CREATE TABLE Booking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    ScreeningId INT NOT NULL,
    BookingDate DATETIME NOT NULL,
    CustomerNumber INT NOT NULL,
    BookingStatus NVARCHAR(50),
    Price FLOAT,
    IsDiscounted BIT,
    FOREIGN KEY (ScreeningId) REFERENCES Screening(Id),
    FOREIGN KEY (CustomerNumber) REFERENCES Customer(CustomerNumber)
);

CREATE TABLE BookingSeat (
    BookingId INT NOT NULL,
    SeatId INT NOT NULL,
    PRIMARY KEY (BookingId, SeatId),
    FOREIGN KEY (BookingId) REFERENCES Booking(BookingId),
    FOREIGN KEY (SeatId) REFERENCES Seat(Id)
);