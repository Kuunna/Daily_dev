CREATE TABLE [Fact_News] (
  [NewID] INT PRIMARY KEY,
  [Title] NVARCHAR(255),
  [Content] TEXT,
  [PublishedDate] INT,
  [SourceID] INT,
  [TopicID] INT,
  [Author] NVARCHAR(100),
  [ImageURL] NVARCHAR(255),
  [ViewCount] INT,
  [LikeCount] INT,
  [CommentCount] INT
)
GO

CREATE TABLE [Dim_Source] (
  [SourceID] INT PRIMARY KEY,
  [SourceName] NVARCHAR(255),
  [RSS_URL] NVARCHAR(500),
  [LastUpdated] INT,
  [ArticleCount] INT
)
GO

CREATE TABLE [Dim_Tag] (
  [TagID] INT PRIMARY KEY,
  [TagName] NVARCHAR(255),
  [TagDescription] TEXT,
  [TagCount] INT
)
GO

CREATE TABLE [Dim_User] (
  [UserID] INT PRIMARY KEY,
  [UserName] NVARCHAR(255),
  [Email] NVARCHAR(255),
  [Password] NVARCHAR(255),
  [JoinDate] INT,
  [LastLogin] INT,
  [Preferences] TEXT,
  [Role] NVARCHAR(50)
)
GO

CREATE TABLE [Dim_Date]
(	[DateKey] INT primary key, 
	[Date] DATETIME,
	[Weekday] INT,
	[DayName] VARCHAR(9), 
	[DayOfWeek] INT,
	[DayOfMonth] INT,
	[DayOfYear] INT,
	[Month] INT, 
	[MonthName] VARCHAR(9),
	[Quarter] INT,
	[Year] INT,
	[MonthYear] CHAR(10), 
	[MM-YYYY] CHAR(6),
)

GO

CREATE TABLE [Fact_Article_Interaction] (
  [InteractionID] INT PRIMARY KEY,
  [UserID] INT,
  [NewsID] INT,
  [InteractionType] NVARCHAR(50),
  [InteractionDate] INT,
  [CommentText] TEXT,
  [UpvoteCount] INT
)
GO

CREATE TABLE [Fact_Comments] (
  [CommentID] INT PRIMARY KEY,
  [UserID] INT,
  [NewsID] INT,
  [CommentText] TEXT,
  [CommentDate] INT
)
GO

CREATE TABLE [Fact_Bookmark] (
  [BookmarkID] INT PRIMARY KEY,
  [UserID] INT,
  [NewsID] INT,
  [BookmarkDate] INT
)
GO

CREATE TABLE [Fact_History] (
  [HistoryID] INT PRIMARY KEY,
  [UserID] INT,
  [NewsID] INT,
  [ReadDate] INT,
  [ReadDuration] INT
)
GO

CREATE TABLE [Dim_Category] (
  [CategoryID] INT PRIMARY KEY,
  [CategoryName] NVARCHAR(255),
  [CategoryDescription] TEXT
)
GO

CREATE TABLE [News_Tag] (
  [NewsID] INT,
  [TagID] INT
)
GO

CREATE TABLE [User_Source] (
  [UserID] INT,
  [SourceID] INT,
  [FollowDate] INT
)
GO

CREATE TABLE [User_Tag] (
  [UserID] INT,
  [TagID] INT,
  [InterestLevel] NVARCHAR(50),
  [FollowDate] INT
)
