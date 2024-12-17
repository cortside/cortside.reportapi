DECLARE @description AS sql_variant;

PRINT 'Before TRY'
BEGIN TRY
	BEGIN TRAN
	PRINT 'First Statement in the TRY block'
BEGIN TRANSACTION;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    ALTER TABLE [dbo].[Report] DROP CONSTRAINT [FK_Report_ReportGroup_ReportGroupId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    ALTER TABLE [dbo].[ReportArgument] DROP CONSTRAINT [FK_ReportArgument_Report_ReportId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    SET @description = N'Username (upn claim)';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'UserPrincipalName';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    SET @description = N'Subject primary key';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'Name';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    
    SET @description = N'Subject primary key';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'GivenName';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    
    SET @description = N'Subject Surname ()';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'FamilyName';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    
    SET @description = N'Date and time entity was created';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'CreatedDate';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    
    SET @description = N'Subject primary key';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'SubjectId';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    ALTER TABLE [dbo].[Report] ADD CONSTRAINT [FK_Report_ReportGroup_ReportGroupId] FOREIGN KEY ([ReportGroupId]) REFERENCES [dbo].[ReportGroup] ([ReportGroupId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    ALTER TABLE [dbo].[ReportArgument] ADD CONSTRAINT [FK_ReportArgument_Report_ReportId] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[Report] ([ReportId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241217231220_CreatedSubject'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241217231220_CreatedSubject', N'8.0.11');
END;

COMMIT;

	PRINT 'Last Statement in the TRY block'
	COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'In CATCH Block'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;

    THROW; -- Raise error to the client.
END CATCH
PRINT 'After END CATCH'
GO
