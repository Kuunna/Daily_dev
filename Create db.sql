CREATE TABLE [Provider] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Name] NVARCHAR(255),
  [Source] NVARCHAR(255)
)
GO

CREATE TABLE [Category] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Name] NVARCHAR(255),
  [ProviderId] INT,
  [Source] NVARCHAR(255),
  [ttl] INT,
  [generator] NVARCHAR(255),
  [docs] NVARCHAR(255)
)
GO

CREATE TABLE [Item] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Title] NVARCHAR(255),
  [Link] NVARCHAR(255),
  [Guid] NVARCHAR(255),
  [PubDate] DATETIME,
  [Image] NVARCHAR(255),
  [CategoryId] INT,
  [author] NVARCHAR(255),
  [summary] NVARCHAR(MAX),
  [comments] NVARCHAR(255)
)
GO

CREATE TABLE [Tag] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Name] NVARCHAR(255),
  [description] NVARCHAR(255)
)
GO

CREATE TABLE [NewTag] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [NewId] INT,
  [TagId] INT
)
GO

CREATE TABLE [User] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Name] NVARCHAR(255),
  [Password] varchar(255)
)

GO

CREATE TABLE [UserCategory] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [UserId] INT,
  [CategoryId] INT
)
GO

CREATE TABLE [UserTag] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [UserId] INT,
  [TagId] INT
)
GO

CREATE TABLE [TableConfig] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [UserId] INT,
  [MostLiked] INT,
  [MostRead] INT,
  [MostTagged] INT,
  [FavoriteCategory] INT
)

Drop table Category;
Drop table Item;
Drop table NewTag;
Drop table [Provider];
Drop table TableConfig;
Drop table Tag;
Drop table [User];
Drop table [UserCategory];
Drop table UserTag;