USE [master]
GO

CREATE DATABASE [Climate]
GO
ALTER DATABASE [Climate] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Climate].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Climate] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Climate] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Climate] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Climate] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Climate] SET ARITHABORT OFF 
GO
ALTER DATABASE [Climate] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Climate] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Climate] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Climate] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Climate] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Climate] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Climate] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Climate] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Climate] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Climate] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Climate] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Climate] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Climate] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Climate] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Climate] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Climate] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Climate] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Climate] SET RECOVERY FULL 
GO
ALTER DATABASE [Climate] SET  MULTI_USER 
GO
ALTER DATABASE [Climate] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Climate] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Climate] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Climate] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Climate] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Climate] SET QUERY_STORE = OFF
GO
USE [Climate]
GO
/****** Object:  Table [dbo].[Auth]    Script Date: 04.11.2021 17:01:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auth](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](30) NOT NULL,
	[Password] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_Auth] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_Auth] UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sensor]    Script Date: 04.11.2021 17:01:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sensor](
	[SensorID] [int] IDENTITY(1,1) NOT NULL,
	[Location] [nvarchar](30) NOT NULL,
	[Date] [date] NOT NULL,
	[NormalTemp] [int] NOT NULL,
	[NormalHumidity] [int] NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
(
	[SensorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SensorData]    Script Date: 04.11.2021 17:01:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SensorData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[SensorID] [int] NOT NULL,
	[Temp] [int] NOT NULL,
	[Humidity] [int] NOT NULL,
	[Gas] [bit] NOT NULL,
 CONSTRAINT [PK_SensorDate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Sensor] ADD  CONSTRAINT [DF_Device_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[SensorData] ADD  CONSTRAINT [DF_SensorDate_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO
ALTER TABLE [dbo].[SensorData]  WITH CHECK ADD  CONSTRAINT [Device_SensorDate] FOREIGN KEY([SensorID])
REFERENCES [dbo].[Sensor] ([SensorID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SensorData] CHECK CONSTRAINT [Device_SensorDate]
GO
USE [master]
GO
ALTER DATABASE [Climate] SET  READ_WRITE 
GO

GO

CREATE TRIGGER [dbo].[addToUP] on [dbo].[SensorData] after insert as
begin
	declare @allCount int = (SELECT COUNT(*) FROM [dbo].[SensorData]);
	
	/*���� ������� ������ 10, �� ������ ���� ������ ������*/
	if (@allCount=31)
	begin
	declare @lowStr int = (select min(ID) from SensorData); 
	delete from [dbo].[SensorData] where (ID=@lowStr);
	end
	
end
