DROP TRIGGER IF EXISTS dbo.trReportGroup
GO

---
-- Trigger for ReportGroup that will handle logging of both update and delete
-- NOTE: inserted data is current value in row if not altered
---
CREATE TRIGGER trReportGroup
	ON [dbo].[ReportGroup]
	FOR UPDATE, DELETE
	AS
		BEGIN
	SET NOCOUNT ON

	DECLARE 
		@AuditLogTransactionId	bigint,
		@Inserted	    		int = 0,
 		@ROWS_COUNT				int

	SELECT @ROWS_COUNT=count(*) from inserted

    -- Check if this is an INSERT, UPDATE or DELETE Action.
    DECLARE @action as varchar(10);
    SET @action = 'INSERT';
    IF EXISTS(SELECT 1 FROM DELETED)
    BEGIN
        SET @action = 
            CASE
                WHEN EXISTS(SELECT 1 FROM INSERTED) THEN 'UPDATE'
                ELSE 'DELETE'
            END
        SELECT @ROWS_COUNT=count(*) from deleted
    END

	-- determine username
	DECLARE @UserName nvarchar(200);
	set @username = current_user

	-- insert parent transaction
	INSERT INTO audit.AuditLogTransaction (TableName, TableSchema, Action, HostName, ApplicationName, AuditLogin, AuditDate, AffectedRows, DatabaseName, UserId, TransactionId)
	values('ReportGroup', 'dbo', @action, CASE WHEN LEN(HOST_NAME()) < 1 THEN ' ' ELSE HOST_NAME() END,
		CASE WHEN LEN(APP_NAME()) < 1 THEN ' ' ELSE APP_NAME() END,
		SUSER_SNAME(), GETDATE(), @ROWS_COUNT, db_name(), @UserName, CURRENT_TRANSACTION_ID()
	)
	Set @AuditLogTransactionId = SCOPE_IDENTITY()

	-- [ReportGroupId]
	IF UPDATE([ReportGroupId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportGroupId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportGroupId], NEW.[ReportGroupId]), 0), '[[ReportGroupId]] Is Null')),
				'[ReportGroupId]',
				CONVERT(nvarchar(4000), OLD.[ReportGroupId], 126),
				CONVERT(nvarchar(4000), NEW.[ReportGroupId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportGroupId], NEW.[ReportGroupId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportGroupId] = OLD.[ReportGroupId] or (NEW.[ReportGroupId] Is Null and OLD.[ReportGroupId] Is Null))
			WHERE ((NEW.[ReportGroupId] <> OLD.[ReportGroupId]) 
					Or (NEW.[ReportGroupId] Is Null And OLD.[ReportGroupId] Is Not Null)
					Or (NEW.[ReportGroupId] Is Not Null And OLD.[ReportGroupId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [Name]
	IF UPDATE([Name]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportGroupId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportGroupId], NEW.[ReportGroupId]), 0), '[[ReportGroupId]] Is Null')),
				'[Name]',
				CONVERT(nvarchar(4000), OLD.[Name], 126),
				CONVERT(nvarchar(4000), NEW.[Name], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportGroupId], NEW.[ReportGroupId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportGroupId] = OLD.[ReportGroupId] or (NEW.[ReportGroupId] Is Null and OLD.[ReportGroupId] Is Null))
			WHERE ((NEW.[Name] <> OLD.[Name]) 
					Or (NEW.[Name] Is Null And OLD.[Name] Is Not Null)
					Or (NEW.[Name] Is Not Null And OLD.[Name] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

END
GO
