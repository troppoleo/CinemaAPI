USE [master]
GO
/****** Object:  Database [Cinema]    Script Date: 19/05/2023 17:55:02 ******/
CREATE DATABASE [Cinema]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Cinema', FILENAME = N'C:\Users\depalle1\OneDrive - Reti\PROJECTS\Percorso.net-2023-main\MngCinema\DB\Cinema.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Cinema_log', FILENAME = N'C:\Users\depalle1\OneDrive - Reti\PROJECTS\Percorso.net-2023-main\MngCinema\DB\Cinema.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Cinema] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Cinema].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Cinema] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [Cinema] SET ANSI_NULLS ON 
GO
ALTER DATABASE [Cinema] SET ANSI_PADDING ON 
GO
ALTER DATABASE [Cinema] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [Cinema] SET ARITHABORT ON 
GO
ALTER DATABASE [Cinema] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Cinema] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Cinema] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Cinema] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Cinema] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [Cinema] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [Cinema] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Cinema] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [Cinema] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Cinema] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Cinema] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Cinema] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Cinema] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Cinema] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Cinema] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Cinema] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Cinema] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Cinema] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Cinema] SET  MULTI_USER 
GO
ALTER DATABASE [Cinema] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Cinema] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Cinema] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Cinema] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Cinema] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Cinema] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Cinema] SET QUERY_STORE = OFF
GO
USE [Cinema]
GO
/****** Object:  User [cineUser]    Script Date: 19/05/2023 17:55:03 ******/
CREATE USER [cineUser] FOR LOGIN [cineUser] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  UserDefinedFunction [dbo].[GetMinDate]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetMinDate]()
RETURNS date
AS
BEGIN

	RETURN convert(datetime, '19001231', 112)

END
GO
/****** Object:  UserDefinedFunction [dbo].[GetUpgradedPrice]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  FUNCTION [dbo].[GetUpgradedPrice]
(
	@cinemaRoomId int,
	@price money
)
RETURNS money
AS
BEGIN
	declare @vipPrice money; 

	select @vipPrice = upgradeVipPrice * @price
	from CinemaRoom
	where id = @cinemaRoomId;

	RETURN @vipPrice;

END
GO
/****** Object:  Table [dbo].[Ticket]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticket](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[customerId] [int] NULL,
	[movieScheduleId] [int] NULL,
	[priceStd] [money] NULL,
	[reservedStdSeats] [int] NULL,
	[reservedVipSeat] [int] NULL,
	[priceVipPercent] [decimal](10, 2) NULL,
	[dateTicket] [datetime] NULL,
 CONSTRAINT [PK_CustomerCrossMovieSchedule] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MovieSchedule]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovieSchedule](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[movieId] [int] NOT NULL,
	[cinemaRoomId] [int] NOT NULL,
	[startDate] [datetime] NOT NULL,
	[isApproved] [int] NULL,
	[vipSeat] [int] NULL,
	[stdSeat] [int] NULL,
	[status] [varchar](20) NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movie]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movie](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[filmName] [varchar](50) NOT NULL,
	[duration] [int] NOT NULL,
	[genere] [varchar](50) NULL,
	[trama] [varchar](500) NULL,
	[moviePlot] [varchar](50) NULL,
	[actors] [varchar](500) NULL,
	[director] [varchar](50) NULL,
	[productionYear] [int] NULL,
	[cover] [nvarchar](250) NULL,
	[limitAge] [int] NULL,
 CONSTRAINT [PK_Movie] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_customer_movie_watched]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[view_customer_movie_watched] as
select m.filmName, t.dateTicket, t.customerId, m.actors, m.director, m.duration, m.genere, m.trama
from 
	Ticket t
	join
	MovieSchedule ms
		on t.movieScheduleId = ms.id
	join
	Movie m
		on ms.movieId = m.id
GO
/****** Object:  Table [dbo].[MovieRate]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MovieRate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[customerId] [int] NOT NULL,
	[movieId] [int] NULL,
	[actorRate] [int] NULL,
	[tramaRate] [int] NULL,
	[ambientRate] [int] NULL,
	[commentNote] [varchar](1000) NULL,
 CONSTRAINT [PK_MovieRate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_Review]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_Review]
AS
SELECT m.filmName, r.actorRate, r.tramaRate, r.ambientRate, r.commentNote
FROM     dbo.Movie AS m INNER JOIN
                  dbo.MovieRate AS r ON m.id = r.movieId
GO
/****** Object:  Table [dbo].[CinemaRoom]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CinemaRoom](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[roomName] [varchar](50) NULL,
	[maxVipSeat] [int] NOT NULL,
	[maxStdSeat] [int] NOT NULL,
 CONSTRAINT [PK__CinemaRo__3214EC07D65C7C8B] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CinemaRoomCrossUserEmployee]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CinemaRoomCrossUserEmployee](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userEmployeeId] [int] NULL,
	[cinemaRoomId] [int] NULL,
 CONSTRAINT [PK_CinemaRoomCrossUserEmployee] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[surname] [varchar](50) NOT NULL,
	[birthdate] [date] NOT NULL,
	[email] [varchar](100) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobEmployeeQualification]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobEmployeeQualification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[short_descr] [varchar](20) NOT NULL,
	[description] [varchar](250) NOT NULL,
	[minimum_required] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriceTicketDefault]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceTicketDefault](
	[seatType] [varchar](50) NOT NULL,
	[price] [money] NULL,
 CONSTRAINT [PK_PriceTicketDefault] PRIMARY KEY CLUSTERED 
(
	[seatType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projection]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projection](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](100) NULL,
	[start_show] [numeric](18, 0) NULL,
	[end_show] [numeric](18, 0) NULL,
	[cinemaRoomId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserEmployee]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserEmployee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[jobQualificationID] [int] NOT NULL,
	[name] [varchar](50) NULL,
	[surname] [varchar](50) NULL,
	[isActive] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersAdmin]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersAdmin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[surname] [varchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTypes]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[userTypeShort] [varchar](20) NULL,
	[description] [varchar](150) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WeekCalendar]    Script Date: 19/05/2023 17:55:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WeekCalendar](
	[id] [int] NOT NULL,
	[dayName] [varchar](20) NOT NULL,
	[startTime] [time](0) NOT NULL,
	[endTIme] [time](0) NOT NULL,
 CONSTRAINT [PK_WeekCalendar] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CinemaRoom] ON 

INSERT [dbo].[CinemaRoom] ([Id], [roomName], [maxVipSeat], [maxStdSeat]) VALUES (1, N'r1 xxx', 220, 330)
INSERT [dbo].[CinemaRoom] ([Id], [roomName], [maxVipSeat], [maxStdSeat]) VALUES (2, N'cine 2', 100, 100)
INSERT [dbo].[CinemaRoom] ([Id], [roomName], [maxVipSeat], [maxStdSeat]) VALUES (3, N'Plutone1', 100, 200)
INSERT [dbo].[CinemaRoom] ([Id], [roomName], [maxVipSeat], [maxStdSeat]) VALUES (5, N'neeew name', 0, 0)
SET IDENTITY_INSERT [dbo].[CinemaRoom] OFF
GO
SET IDENTITY_INSERT [dbo].[CinemaRoomCrossUserEmployee] ON 

INSERT [dbo].[CinemaRoomCrossUserEmployee] ([id], [userEmployeeId], [cinemaRoomId]) VALUES (2, NULL, 1)
INSERT [dbo].[CinemaRoomCrossUserEmployee] ([id], [userEmployeeId], [cinemaRoomId]) VALUES (1003, 3002, 5)
SET IDENTITY_INSERT [dbo].[CinemaRoomCrossUserEmployee] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([id], [name], [surname], [birthdate], [email], [Password], [UserName]) VALUES (1, N'Customer1', N'Cognome cust 1', CAST(N'2000-05-18' AS Date), N'string', N'Customer1', N'Customer1')
INSERT [dbo].[Customer] ([id], [name], [surname], [birthdate], [email], [Password], [UserName]) VALUES (2, N'Customer2', N'surname Customer2', CAST(N'2001-05-18' AS Date), N'string', N'Customer2', N'Customer2')
INSERT [dbo].[Customer] ([id], [name], [surname], [birthdate], [email], [Password], [UserName]) VALUES (3, N'string', N'string', CAST(N'2023-05-18' AS Date), N'string', N'string', N'string')
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[JobEmployeeQualification] ON 

INSERT [dbo].[JobEmployeeQualification] ([Id], [short_descr], [description], [minimum_required]) VALUES (1, N'GET_TICKET', N'(bigliettaio) fornitore di biglietti', 2)
INSERT [dbo].[JobEmployeeQualification] ([Id], [short_descr], [description], [minimum_required]) VALUES (2, N'OWN_SALA', N'Responsabile di sala', NULL)
SET IDENTITY_INSERT [dbo].[JobEmployeeQualification] OFF
GO
SET IDENTITY_INSERT [dbo].[Movie] ON 

INSERT [dbo].[Movie] ([id], [filmName], [duration], [genere], [trama], [moviePlot], [actors], [director], [productionYear], [cover], [limitAge]) VALUES (1, N'rambo', 120, N'DRAMMATICO', N'string', N'string', N'string', N'string', 0, N'string', NULL)
INSERT [dbo].[Movie] ([id], [filmName], [duration], [genere], [trama], [moviePlot], [actors], [director], [productionYear], [cover], [limitAge]) VALUES (2, N'roky', 110, N'DRAMMATICO', N'string', N'string', N'string', N'string', 0, N'string', NULL)
INSERT [dbo].[Movie] ([id], [filmName], [duration], [genere], [trama], [moviePlot], [actors], [director], [productionYear], [cover], [limitAge]) VALUES (3, N'pippo', 50, N'EROTICO', N'string', N'string', N'string', N'string', 0, N'string', NULL)
INSERT [dbo].[Movie] ([id], [filmName], [duration], [genere], [trama], [moviePlot], [actors], [director], [productionYear], [cover], [limitAge]) VALUES (4, N'il padrino', 200, N'THRILLER', N'string', N'string', N'string', N'string', 1950, N'string', NULL)
INSERT [dbo].[Movie] ([id], [filmName], [duration], [genere], [trama], [moviePlot], [actors], [director], [productionYear], [cover], [limitAge]) VALUES (5, N'Avatar', 300, N'ACTION', N'string', N'string', N'string', N'string', 1999, N'string', NULL)
SET IDENTITY_INSERT [dbo].[Movie] OFF
GO
SET IDENTITY_INSERT [dbo].[MovieRate] ON 

INSERT [dbo].[MovieRate] ([id], [customerId], [movieId], [actorRate], [tramaRate], [ambientRate], [commentNote]) VALUES (1, 1, 1, 1, 2, 3, N'string')
SET IDENTITY_INSERT [dbo].[MovieRate] OFF
GO
SET IDENTITY_INSERT [dbo].[MovieSchedule] ON 

INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (1, 1, 2, CAST(N'2023-05-19T09:30:00.000' AS DateTime), 1, 96, 202, N'DONE')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (2, 1, 1, CAST(N'2023-05-18T15:30:00.000' AS DateTime), 1, 0, 0, N'DONE')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (3, 4, 1, CAST(N'2023-05-13T12:30:00.000' AS DateTime), 0, 0, 0, N'CANCELLED')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (4, 3, 2, CAST(N'2023-05-19T12:30:00.000' AS DateTime), 1, 16, 8, N'CANCELLED')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (5, 1, 1, CAST(N'2023-05-14T16:31:00.000' AS DateTime), 0, 11, 11, N'CANCELLED')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (6, 4, 1, CAST(N'2023-05-13T16:30:00.000' AS DateTime), 0, 20, 10, N'CANCELLED')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (7, 4, 3, CAST(N'2023-05-14T12:31:00.000' AS DateTime), 0, 11, 11, N'CANCELLED')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (1002, 2, 3, CAST(N'2023-05-16T12:05:00.000' AS DateTime), 0, 100, 200, N'CANCELLED')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (1004, 1, 5, CAST(N'2023-05-20T13:47:00.000' AS DateTime), 0, 100, 100, N'WAITING')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (1005, 5, 1, CAST(N'2023-05-20T13:47:00.000' AS DateTime), 0, 220, 330, N'WAITING')
INSERT [dbo].[MovieSchedule] ([id], [movieId], [cinemaRoomId], [startDate], [isApproved], [vipSeat], [stdSeat], [status]) VALUES (1006, 5, 2, CAST(N'2023-05-21T14:21:00.000' AS DateTime), 0, 100, 100, N'WAITING')
SET IDENTITY_INSERT [dbo].[MovieSchedule] OFF
GO
INSERT [dbo].[PriceTicketDefault] ([seatType], [price]) VALUES (N'STD_SEAT', 7.0000)
INSERT [dbo].[PriceTicketDefault] ([seatType], [price]) VALUES (N'VIP_SEAT_PERCENTUAL', 10.0000)
GO
SET IDENTITY_INSERT [dbo].[Ticket] ON 

INSERT [dbo].[Ticket] ([id], [customerId], [movieScheduleId], [priceStd], [reservedStdSeats], [reservedVipSeat], [priceVipPercent], [dateTicket]) VALUES (1, NULL, 1, 10.0000, NULL, NULL, NULL, CAST(N'2023-05-18T22:03:36.427' AS DateTime))
INSERT [dbo].[Ticket] ([id], [customerId], [movieScheduleId], [priceStd], [reservedStdSeats], [reservedVipSeat], [priceVipPercent], [dateTicket]) VALUES (2, NULL, 2, 10.0000, NULL, NULL, NULL, CAST(N'2023-05-18T22:03:36.427' AS DateTime))
INSERT [dbo].[Ticket] ([id], [customerId], [movieScheduleId], [priceStd], [reservedStdSeats], [reservedVipSeat], [priceVipPercent], [dateTicket]) VALUES (3, NULL, 1, 8.0000, 1, 1, CAST(10.00 AS Decimal(10, 2)), CAST(N'2023-05-18T22:03:36.427' AS DateTime))
INSERT [dbo].[Ticket] ([id], [customerId], [movieScheduleId], [priceStd], [reservedStdSeats], [reservedVipSeat], [priceVipPercent], [dateTicket]) VALUES (4, NULL, 1, 8.0000, 1, 1, CAST(10.00 AS Decimal(10, 2)), CAST(N'2023-05-18T22:03:36.427' AS DateTime))
INSERT [dbo].[Ticket] ([id], [customerId], [movieScheduleId], [priceStd], [reservedStdSeats], [reservedVipSeat], [priceVipPercent], [dateTicket]) VALUES (1004, 1, 1, NULL, NULL, NULL, NULL, CAST(N'2023-05-18T22:03:36.427' AS DateTime))
INSERT [dbo].[Ticket] ([id], [customerId], [movieScheduleId], [priceStd], [reservedStdSeats], [reservedVipSeat], [priceVipPercent], [dateTicket]) VALUES (1005, 1, 1, 7.0000, 3, 0, CAST(10.00 AS Decimal(10, 2)), CAST(N'2023-05-18T22:03:36.427' AS DateTime))
INSERT [dbo].[Ticket] ([id], [customerId], [movieScheduleId], [priceStd], [reservedStdSeats], [reservedVipSeat], [priceVipPercent], [dateTicket]) VALUES (1008, 1, 1, 7.0000, 2, 2, CAST(10.00 AS Decimal(10, 2)), CAST(N'2023-05-18T00:00:00.000' AS DateTime))
INSERT [dbo].[Ticket] ([id], [customerId], [movieScheduleId], [priceStd], [reservedStdSeats], [reservedVipSeat], [priceVipPercent], [dateTicket]) VALUES (1009, NULL, 4, 5.0000, 2, 4, CAST(10.00 AS Decimal(10, 2)), CAST(N'2023-05-18T22:03:36.427' AS DateTime))
SET IDENTITY_INSERT [dbo].[Ticket] OFF
GO
SET IDENTITY_INSERT [dbo].[UserEmployee] ON 

INSERT [dbo].[UserEmployee] ([Id], [UserName], [Password], [jobQualificationID], [name], [surname], [isActive]) VALUES (1, N'bigliettaio', N'bigliettaio', 1, N'gianni', N'rossi', NULL)
INSERT [dbo].[UserEmployee] ([Id], [UserName], [Password], [jobQualificationID], [name], [surname], [isActive]) VALUES (1003, N'own 1', N'string', 2, N'string', N'string', 0)
INSERT [dbo].[UserEmployee] ([Id], [UserName], [Password], [jobQualificationID], [name], [surname], [isActive]) VALUES (3002, N'name_own_sala', N'name_own_sala', 2, N'ambrogio_own_sala', N'surname_own_sala', 0)
SET IDENTITY_INSERT [dbo].[UserEmployee] OFF
GO
SET IDENTITY_INSERT [dbo].[UsersAdmin] ON 

INSERT [dbo].[UsersAdmin] ([Id], [name], [surname], [UserName], [Password]) VALUES (1, N'Leonardo', N'De Palma', N'admin', N'adminadmin')
SET IDENTITY_INSERT [dbo].[UsersAdmin] OFF
GO
SET IDENTITY_INSERT [dbo].[UserTypes] ON 

INSERT [dbo].[UserTypes] ([Id], [userTypeShort], [description]) VALUES (1, N'ADMIN', N'amministratore della location')
INSERT [dbo].[UserTypes] ([Id], [userTypeShort], [description]) VALUES (2, N'EMPLOYEE', N'personale del cinema con diverse qualifiche (esempio: responsabile sala, bigliettaio, venditore popcorn, barista)')
INSERT [dbo].[UserTypes] ([Id], [userTypeShort], [description]) VALUES (3, N'CUSTOMER', N'cliente')
SET IDENTITY_INSERT [dbo].[UserTypes] OFF
GO
INSERT [dbo].[WeekCalendar] ([id], [dayName], [startTime], [endTIme]) VALUES (0, N'Sunday', CAST(N'11:00:00' AS Time), CAST(N'01:00:00' AS Time))
INSERT [dbo].[WeekCalendar] ([id], [dayName], [startTime], [endTIme]) VALUES (1, N'Monday', CAST(N'12:00:00' AS Time), CAST(N'23:00:00' AS Time))
INSERT [dbo].[WeekCalendar] ([id], [dayName], [startTime], [endTIme]) VALUES (2, N'Tuesday', CAST(N'12:00:00' AS Time), CAST(N'23:00:00' AS Time))
INSERT [dbo].[WeekCalendar] ([id], [dayName], [startTime], [endTIme]) VALUES (3, N'Wednesday', CAST(N'12:00:00' AS Time), CAST(N'23:00:00' AS Time))
INSERT [dbo].[WeekCalendar] ([id], [dayName], [startTime], [endTIme]) VALUES (4, N'Thursday', CAST(N'12:00:00' AS Time), CAST(N'23:00:00' AS Time))
INSERT [dbo].[WeekCalendar] ([id], [dayName], [startTime], [endTIme]) VALUES (5, N'Friday', CAST(N'12:00:00' AS Time), CAST(N'23:00:00' AS Time))
INSERT [dbo].[WeekCalendar] ([id], [dayName], [startTime], [endTIme]) VALUES (6, N'Saturday', CAST(N'11:00:00' AS Time), CAST(N'01:00:00' AS Time))
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [AK_roomName]    Script Date: 19/05/2023 17:55:03 ******/
ALTER TABLE [dbo].[CinemaRoom] ADD  CONSTRAINT [AK_roomName] UNIQUE NONCLUSTERED 
(
	[roomName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MovieSchedule] ADD  CONSTRAINT [DF_MovieSchedule_startDate]  DEFAULT ([dbo].[GetMinDate]()) FOR [startDate]
GO
ALTER TABLE [dbo].[MovieSchedule] ADD  CONSTRAINT [DF_MovieSchedule_isApproved]  DEFAULT ((0)) FOR [isApproved]
GO
ALTER TABLE [dbo].[MovieSchedule] ADD  CONSTRAINT [DF_MovieSchedule_status]  DEFAULT ('WAITING') FOR [status]
GO
ALTER TABLE [dbo].[CinemaRoomCrossUserEmployee]  WITH CHECK ADD  CONSTRAINT [FK_CinemaRoomCrossUserEmployee_CinemaRoom] FOREIGN KEY([cinemaRoomId])
REFERENCES [dbo].[CinemaRoom] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CinemaRoomCrossUserEmployee] CHECK CONSTRAINT [FK_CinemaRoomCrossUserEmployee_CinemaRoom]
GO
ALTER TABLE [dbo].[CinemaRoomCrossUserEmployee]  WITH CHECK ADD  CONSTRAINT [FK_CinemaRoomCrossUserEmployee_UserEmployee] FOREIGN KEY([userEmployeeId])
REFERENCES [dbo].[UserEmployee] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CinemaRoomCrossUserEmployee] CHECK CONSTRAINT [FK_CinemaRoomCrossUserEmployee_UserEmployee]
GO
ALTER TABLE [dbo].[MovieRate]  WITH CHECK ADD  CONSTRAINT [FK_MovieRate_Customer] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MovieRate] CHECK CONSTRAINT [FK_MovieRate_Customer]
GO
ALTER TABLE [dbo].[MovieRate]  WITH CHECK ADD  CONSTRAINT [FK_MovieRate_Movie] FOREIGN KEY([movieId])
REFERENCES [dbo].[Movie] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MovieRate] CHECK CONSTRAINT [FK_MovieRate_Movie]
GO
ALTER TABLE [dbo].[MovieSchedule]  WITH CHECK ADD  CONSTRAINT [FK_MovieSchedule_CinemaRoom] FOREIGN KEY([cinemaRoomId])
REFERENCES [dbo].[CinemaRoom] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MovieSchedule] CHECK CONSTRAINT [FK_MovieSchedule_CinemaRoom]
GO
ALTER TABLE [dbo].[MovieSchedule]  WITH CHECK ADD  CONSTRAINT [FK_MovieSchedule_Movie] FOREIGN KEY([movieId])
REFERENCES [dbo].[Movie] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MovieSchedule] CHECK CONSTRAINT [FK_MovieSchedule_Movie]
GO
ALTER TABLE [dbo].[Projection]  WITH CHECK ADD  CONSTRAINT [FK_Projection_ToTable] FOREIGN KEY([cinemaRoomId])
REFERENCES [dbo].[CinemaRoom] ([Id])
GO
ALTER TABLE [dbo].[Projection] CHECK CONSTRAINT [FK_Projection_ToTable]
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_CustomerCrossMovieSchedule_Customer] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK_CustomerCrossMovieSchedule_Customer]
GO
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_CustomerCrossMovieSchedule_MovieSchedule] FOREIGN KEY([movieScheduleId])
REFERENCES [dbo].[MovieSchedule] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK_CustomerCrossMovieSchedule_MovieSchedule]
GO
ALTER TABLE [dbo].[UserEmployee]  WITH CHECK ADD  CONSTRAINT [FK_JobQual_Employ] FOREIGN KEY([jobQualificationID])
REFERENCES [dbo].[JobEmployeeQualification] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserEmployee] CHECK CONSTRAINT [FK_JobQual_Employ]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome della sala' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CinemaRoom', @level2type=N'COLUMN',@level2name=N'roomName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'massimo numero di posti VIP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CinemaRoom', @level2type=N'COLUMN',@level2name=N'maxVipSeat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Massimo numero di posto standard' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CinemaRoom', @level2type=N'COLUMN',@level2name=N'maxStdSeat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'tabelle delle "sale cinema", utile per associare un employee alle sale cinema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CinemaRoom'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'tabella che associa gli impiegati alle sale cinema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CinemaRoomCrossUserEmployee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Numero minimo richiesto di Employee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JobEmployeeQualification', @level2type=N'COLUMN',@level2name=N'minimum_required'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'contiene le possibili mansioni che possono essere date ai soli EMPLOYEE:
> Responsabili di sala
> bigliettai' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JobEmployeeQualification'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'è la trama' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Movie', @level2type=N'COLUMN',@level2name=N'moviePlot'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'lista degli attori principali' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Movie', @level2type=N'COLUMN',@level2name=N'actors'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'regista' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Movie', @level2type=N'COLUMN',@level2name=N'director'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'questo andrebbe fatto come "image" ma posso mettere anche l''url per ora mi semplifico la vita' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Movie', @level2type=N'COLUMN',@level2name=N'cover'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'indica se è vietato ai minori di anni X' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Movie', @level2type=N'COLUMN',@level2name=N'limitAge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'elenco dei film' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Movie'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 se è stato approvato dall''Admin' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MovieSchedule', @level2type=N'COLUMN',@level2name=N'isApproved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Si intende i posti VIP rimasti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MovieSchedule', @level2type=N'COLUMN',@level2name=N'vipSeat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Si intende i posti Standard rimasti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MovieSchedule', @level2type=N'COLUMN',@level2name=N'stdSeat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'dominio:
WAITING --> deve ancora iniziare
IN_PROGRESS --> è in corso di visione
CLEAN_TIME --> è finito e stanno facendo le pulizie
DONE --> finito e sala liberata, include i 10 min extra film

utile per semplificare i filtri, aggiornata dal BGW' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MovieSchedule', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'mette in relazione i film con le sale cinematografiche, contiene:
> data e ora di inizio 
> l''approvazione dell''ADMIN {1, 0}
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MovieSchedule'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Definisce i prezzi di default per Standard e Vip Seat' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PriceTicketDefault'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'contiene la schedulazione/programmazione delle proiezioni e l''associazione della sala cinema' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Projection'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'è il prezzo del biglietto che eventualmente potrebbe essere maggiorato per vip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ticket', @level2type=N'COLUMN',@level2name=N'priceStd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Numero di bilgietti starndard acquistati' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ticket', @level2type=N'COLUMN',@level2name=N'reservedStdSeats'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'numero di biglietti Vip acquistati' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ticket', @level2type=N'COLUMN',@level2name=N'reservedVipSeat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'percentuale di maggiorazione per i prezzi Vip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ticket', @level2type=N'COLUMN',@level2name=N'priceVipPercent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'la data in cui è stato generato il ticket' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ticket', @level2type=N'COLUMN',@level2name=N'dateTicket'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'tiene traccia dei biglietti emessi
con l''informazione dei film che un customer ha comprato
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ticket'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'serve solo ai bigliettai, per indicare se sono attivi o meno {null/0, 1 }' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserEmployee', @level2type=N'COLUMN',@level2name=N'isActive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'specifica per i soli Employee (no admin)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserEmployee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Specifica per gli amministratori' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UsersAdmin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella delle qualifiche che possono essere CRUD dal''admin' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserTypes', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'nome breve' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserTypes', @level2type=N'COLUMN',@level2name=N'userTypeShort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'tabella con i vari ruoli:
ADMIN, EMPLOYEE, CUSTOMER
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'contiene i giorni della settimana con le fasce di apertura
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WeekCalendar'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[44] 4[18] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "m"
            Begin Extent = 
               Top = 343
               Left = 48
               Bottom = 506
               Right = 245
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "r"
            Begin Extent = 
               Top = 511
               Left = 48
               Bottom = 674
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 3876
         Alias = 900
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Review'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_Review'
GO
USE [master]
GO
ALTER DATABASE [Cinema] SET  READ_WRITE 
GO
