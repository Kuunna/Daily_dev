CREATE TABLE [Provider] (
  [Id] INT PRIMARY KEY,
  [Name] NVARCHAR(255),
  [Source] NVARCHAR(255)
)
GO

CREATE TABLE [Category] (
  [Id] INT PRIMARY KEY,
  [Name] NVARCHAR(255),
  [ProviderId] INT,
  [Source] NVARCHAR(255),
  [ttl] INT,
  [generator] NVARCHAR(255),
  [docs] NVARCHAR(255)
)
GO

CREATE TABLE [Item] (
  [Id] INT PRIMARY KEY,
  [Title] NVARCHAR(255),
  [Link] NVARCHAR(255),
  [Guid] NVARCHAR(255),
  [PubDate] DATETIME,
  [Image] NVARCHAR(255),
  [CategoryId] INT,
  [author] NVARCHAR(255),
  [summary] NVARCHAR(255),
  [comments] NVARCHAR(255)
)
GO

CREATE TABLE [Tag] (
  [Id] INT PRIMARY KEY,
  [Name] NVARCHAR(255),
  [description] NVARCHAR(255)
)
GO

CREATE TABLE [NewTag] (
  [Id] INT PRIMARY KEY,
  [NewId] INT,
  [TagId] INT
)
GO

CREATE TABLE [User] (
  [Id] INT PRIMARY KEY,
  [Name] NVARCHAR(255),
  [Password] varchar(255)
)

GO

CREATE TABLE [UserCategory] (
  [Id] INT PRIMARY KEY,
  [UserId] INT,
  [CategoryId] INT
)
GO

CREATE TABLE [UserTag] (
  [Id] INT PRIMARY KEY,
  [UserId] INT,
  [TagId] INT
)
GO

CREATE TABLE [TableConfig] (
  [Id] INT PRIMARY KEY,
  [UserId] INT,
  [MostLiked] INT,
  [MostRead] INT,
  [MostTagged] INT,
  [FavoriteCategory] INT
)
GO

ALTER TABLE [Category] ADD FOREIGN KEY ([ProviderId]) REFERENCES [Provider] ([Id])
GO

ALTER TABLE [Item] ADD FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id])
GO

ALTER TABLE [NewTag] ADD FOREIGN KEY ([NewId]) REFERENCES [Item] ([Id])
GO

ALTER TABLE [NewTag] ADD FOREIGN KEY ([TagId]) REFERENCES [Tag] ([Id])
GO

ALTER TABLE [UserCategory] ADD FOREIGN KEY ([UserId]) REFERENCES [User] ([Id])
GO

ALTER TABLE [UserCategory] ADD FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id])
GO

ALTER TABLE [UserTag] ADD FOREIGN KEY ([UserId]) REFERENCES [User] ([Id])
GO

ALTER TABLE [UserTag] ADD FOREIGN KEY ([TagId]) REFERENCES [Tag] ([Id])
GO

ALTER TABLE [TableConfig] ADD FOREIGN KEY ([UserId]) REFERENCES [User] ([Id])
GO

ALTER TABLE [TableConfig] ADD FOREIGN KEY ([FavoriteCategory]) REFERENCES [Category] ([Id])
GO
