Create Database [SpeerDb]
GO

USE [SpeerDb]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Notes] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (50)  NOT NULL,
    [Details]   NVARCHAR (500) NOT NULL,
    [CreatedBy] NVARCHAR (50)  NOT NULL,
    [CreatedOn] DATETIME       NOT NULL,
    [UpdatedBy] NVARCHAR (50)  NULL,
    [UpdatedOn] DATETIME       NULL
);
GO

CREATE TABLE [dbo].[SharedNotes] (
    [Id]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [NoteId]     BIGINT        NOT NULL,
    [SharedWith] NVARCHAR (50) NOT NULL,
    [SharedBy]   NVARCHAR (50) NOT NULL,
    [SharedOn]   DATETIME      NOT NULL
);
GO

CREATE TABLE [dbo].[Users](
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



CREATE NONCLUSTERED INDEX [IX_Notes_CreatedBy]
    ON [dbo].[Notes]([CreatedBy] ASC);
GO


CREATE NONCLUSTERED INDEX [IX_Users_Column]
    ON [dbo].[Users]([UserName] ASC);
GO



CREATE UNIQUE INDEX [IX_Notes_Id] ON [dbo].[Notes] ([Id])
GO

CREATE FULLTEXT CATALOG NoteCatalog;
GO

CREATE FULLTEXT INDEX ON [dbo].[Notes] ([Title], [Details]) KEY INDEX [IX_Notes_Id] ON [NoteCatalog] WITH CHANGE_TRACKING AUTO
GO





