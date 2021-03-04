IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210226164733_Initial')
BEGIN
    CREATE TABLE [dbo].[ReportArgumentQuery] (
        [ReportArgumentQueryId] int NOT NULL IDENTITY,
        [ArgQuery] nvarchar(max) NULL,
        CONSTRAINT [PK_ReportArgumentQuery] PRIMARY KEY ([ReportArgumentQueryId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210226164733_Initial')
BEGIN
    CREATE TABLE [dbo].[ReportGroup] (
        [ReportGroupId] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NULL,
        CONSTRAINT [PK_ReportGroup] PRIMARY KEY ([ReportGroupId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210226164733_Initial')
BEGIN
    CREATE TABLE [dbo].[Subject] (
        [SubjectId] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NULL,
        [GivenName] nvarchar(100) NULL,
        [FamilyName] nvarchar(100) NULL,
        [UserPrincipalName] nvarchar(100) NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Subject] PRIMARY KEY ([SubjectId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210226164733_Initial')
BEGIN
    CREATE TABLE [dbo].[Report] (
        [ReportId] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NULL,
        [Description] nvarchar(250) NULL,
        [ReportGroupId] int NOT NULL,
        [Permission] nvarchar(50) NULL,
        CONSTRAINT [PK_Report] PRIMARY KEY ([ReportId]),
        CONSTRAINT [FK_Report_ReportGroup_ReportGroupId] FOREIGN KEY ([ReportGroupId]) REFERENCES [dbo].[ReportGroup] ([ReportGroupId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210226164733_Initial')
BEGIN
    CREATE TABLE [dbo].[ReportArgument] (
        [ReportArgumentId] int NOT NULL IDENTITY,
        [ReportId] int NOT NULL,
        [Name] nvarchar(50) NULL,
        [ArgName] nvarchar(50) NULL,
        [ArgType] nvarchar(50) NULL,
        [ReportArgumentQueryId] int NULL,
        [Sequence] int NOT NULL,
        CONSTRAINT [PK_ReportArgument] PRIMARY KEY ([ReportArgumentId]),
        CONSTRAINT [FK_ReportArgument_Report_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[Report] ([ReportId]) ON DELETE CASCADE,
        CONSTRAINT [FK_ReportArgument_ReportArgumentQuery_ReportArgumentQueryId] FOREIGN KEY ([ReportArgumentQueryId]) REFERENCES [dbo].[ReportArgumentQuery] ([ReportArgumentQueryId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210226164733_Initial')
BEGIN
    CREATE INDEX [IX_Report_ReportGroupId] ON [dbo].[Report] ([ReportGroupId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210226164733_Initial')
BEGIN
    CREATE INDEX [IX_ReportArgument_ReportArgumentQueryId] ON [dbo].[ReportArgument] ([ReportArgumentQueryId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210226164733_Initial')
BEGIN
    CREATE INDEX [IX_ReportArgument_ReportId] ON [dbo].[ReportArgument] ([ReportId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210226164733_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210226164733_Initial', N'5.0.3');
END;
GO

COMMIT;
GO

