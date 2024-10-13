Drop table Category;
CREATE TABLE [Category] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Name] NVARCHAR(255),
  [ProviderId] INT,
  [Source] NVARCHAR(255)
)

Drop table [Provider];
CREATE TABLE [Provider] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [Name] NVARCHAR(255),
  [Source] NVARCHAR(255)
)

Drop table [Item];
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
  [Source] NVARCHAR(255)
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
    Id INT PRIMARY KEY IDENTITY,
    [Name] NVARCHAR(100) UNIQUE,
    Password NVARCHAR(256),
    Email NVARCHAR(100),
    FullName NVARCHAR(100),
    DOB DATE,
    --Role NVARCHAR(50) CHECK (Role IN ('Admin', 'RegisterUser', 'ClientUser')) -- Thêm Role để phân quyền
);


GO

CREATE TABLE [UserCategory] (
  [Id] INT IDENTITY(1,1) PRIMARY KEY,
  [UserId] INT,
  [CategoryId] INT
)
GO

CREATE TABLE [UserProvider] (
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

CREATE TABLE UserLike (
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    ItemId INT
);


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
