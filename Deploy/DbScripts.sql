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

CREATE NONCLUSTERED INDEX [IX_Notes_UserName]
    ON [dbo].[Notes]([CreatedBy] ASC);


GO

CREATE UNIQUE INDEX [IX_Notes_Id] ON [dbo].[Notes] ([Id])
GO

CREATE FULLTEXT CATALOG NoteCatalog;
GO

CREATE FULLTEXT INDEX ON [dbo].[Notes] ([Title], [Details]) KEY INDEX [IX_Notes_Id] ON [NoteCatalog] WITH CHANGE_TRACKING AUTO
GO

CREATE TABLE [dbo].[SharedNotes] (
    [Id]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [NoteId]     BIGINT        NOT NULL,
    [SharedWith] NVARCHAR (50) NOT NULL,
    [SharedBy]   NVARCHAR (50) NOT NULL,
    [SharedOn]   DATETIME      NOT NULL
);
GO

CREATE TABLE [dbo].[Users] (
    [UserName]  NVARCHAR (50)  NOT NULL,
    [Password]  NVARCHAR (200) NOT NULL,
    [CreatedOn] DATETIME       NOT NULL
);

GO

CREATE NONCLUSTERED INDEX [IX_Users_Column]
    ON [dbo].[Users]([UserName] ASC);


GO
ALTER TABLE [dbo].[Users]
    ADD CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserName] ASC);


