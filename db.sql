USE [master]
GO
/****** Object:  Database [Director]    Script Date: 10/5/2015 7:17:01 PM ******/
CREATE DATABASE [Director]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Director', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER2014\MSSQL\DATA\Director.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Director_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER2014\MSSQL\DATA\\Director_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Director] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Director].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Director] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Director] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Director] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Director] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Director] SET ARITHABORT OFF 
GO
ALTER DATABASE [Director] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Director] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Director] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Director] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Director] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Director] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Director] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Director] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Director] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Director] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Director] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Director] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Director] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Director] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Director] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Director] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Director] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Director] SET RECOVERY FULL 
GO
ALTER DATABASE [Director] SET  MULTI_USER 
GO
ALTER DATABASE [Director] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Director] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Director] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Director] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Director] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Director', N'ON'
GO
USE [Director]
GO
/****** Object:  Table [dbo].[Actions]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actions](
	[ConditionID] [uniqueidentifier] NOT NULL,
	[ActionID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Actions_ActionID]  DEFAULT (newid()),
	[ActionType] [int] NOT NULL,
	[ObjectXML] [xml] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Automations]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Automations](
	[AutomationID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Automations_AutomationID]  DEFAULT (newid()),
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Conditions]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conditions](
	[SceneID] [uniqueidentifier] NOT NULL,
	[ConditionID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Conditions_CondtionID]  DEFAULT (newid()),
	[ObjectXML] [xml] NOT NULL,
	[ConditionType] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Scenes]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Scenes](
	[AutomationID] [uniqueidentifier] NOT NULL,
	[SceneID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Scenes_SceneID]  DEFAULT (newid()),
	[SortID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ObjectXML] [xml] NOT NULL,
	[SceneType] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[AddAction]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddAction] 
@conditionid uniqueidentifier,
@actionid uniqueidentifier,
@actiontype int,
@objectxml XML
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Actions(ConditionID,ActionID,ActionType,ObjectXML) OUTPUT inserted.ActionID
	VALUES (@conditionid,@actionid,@actiontype,@objectxml)
END











GO
/****** Object:  StoredProcedure [dbo].[AddAutomation]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddAutomation]
	-- Add the parameters for the stored procedure here
	@autname varchar(50),
	@desc varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into dbo.Automations (Name,Description) OUTPUT inserted.AutomationID values (@autname,@desc)
END











GO
/****** Object:  StoredProcedure [dbo].[AddCondition]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddCondition] 
	-- Add the parameters for the stored procedure here
@sceneid uniqueidentifier,
@conditionid uniqueidentifier,
@objectxml xml,
@conditiontype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Conditions (
	SceneId,
	ConditionId,
	ObjectXML,
	ConditionType) VALUES (
	@sceneid,
	@conditionid,
	@objectxml,
	@conditiontype)
END






GO
/****** Object:  StoredProcedure [dbo].[AddScene]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddScene] 
	-- Add the parameters for the stored procedure here
	@automationID uniqueidentifier,
	@sceneid uniqueidentifier,
	@name varchar(50),
	@sortid	int,
	@objectxml xml,
	@scenetype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Scenes(AutomationID,SceneId,Name,SortID, ObjectXML,SceneType) OUTPUT inserted.SceneID
	Values(@automationID,@sceneid,@name,@sortid,@objectxml,@scenetype);
END











GO
/****** Object:  StoredProcedure [dbo].[GetActions]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetActions] 
	-- Add the parameters for the stored procedure here
	@conditionid uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ActionID,ActionType,ObjectXML from Actions where ConditionID = @conditionid
END



GO
/****** Object:  StoredProcedure [dbo].[GetAutomationByGUID]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAutomationByGUID]
	-- Add the parameters for the stored procedure here
	@automationid uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT AutomationID,Name,Description from dbo.Automations where AutomationID = @automationid order by Name 
END











GO
/****** Object:  StoredProcedure [dbo].[GetAutomations]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAutomations]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT AutomationID,Name,Description from dbo.Automations order by Name
END











GO
/****** Object:  StoredProcedure [dbo].[GetConditions]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetConditions] 
	-- Add the parameters for the stored procedure here
	@sceneid uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ConditionId, ObjectXML, ConditionType from Conditions where SceneID = @sceneid
END





GO
/****** Object:  StoredProcedure [dbo].[GetScenes]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetScenes] 
	-- Add the parameters for the stored procedure here
	@automationid uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SceneID, SortID, Name, ObjectXML, SceneType from Scenes where AutomationID = @automationid order by SortID
END











GO
/****** Object:  StoredProcedure [dbo].[UpdateAction]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAction] 
@conditionid uniqueidentifier,
@actionid uniqueidentifier,
@objectxml XML
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update Actions set
		ConditionID = @conditionid,
		ObjectXML = @objectxml
		where ActionID= @actionid
END











GO
/****** Object:  StoredProcedure [dbo].[UpdateCondition]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCondition] 
	-- Add the parameters for the stored procedure here
@sceneid uniqueidentifier,
@conditionid uniqueidentifier,
@objectxml xml,
@conditiontype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
UPDATE Conditions SET
ObjectXML = @objectxml,
ConditionType = @conditiontype
WHERE
ConditionID=@conditionid
END






GO
/****** Object:  StoredProcedure [dbo].[UpdateScene]    Script Date: 10/5/2015 7:17:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateScene] 
	-- Add the parameters for the stored procedure here
	@sceneid uniqueidentifier,
	@name varchar(50),
	@sortid	int,
	@objectxml xml,
	@scenetype int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Scenes SET
		Name=@name,
		SortId=@sortid,
		SceneType=@scenetype,
		ObjectXML=@objectxml
	WHERE
		SceneID = @sceneid;
END






GO
USE [master]
GO
ALTER DATABASE [Director] SET  READ_WRITE 
GO
