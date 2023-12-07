DROP TRIGGER IF EXISTS dbo.trReport
GO

---
-- Trigger for Report that will handle logging of both update and delete
-- NOTE: inserted data is current value in row if not altered
---
CREATE TRIGGER trReport
	ON [dbo].[Report]
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
	values('Report', 'dbo', @action, CASE WHEN LEN(HOST_NAME()) < 1 THEN ' ' ELSE HOST_NAME() END,
		CASE WHEN LEN(APP_NAME()) < 1 THEN ' ' ELSE APP_NAME() END,
		SUSER_SNAME(), GETDATE(), @ROWS_COUNT, db_name(), @UserName, CURRENT_TRANSACTION_ID()
	)
	Set @AuditLogTransactionId = SCOPE_IDENTITY()

	-- [ReportId]
	IF UPDATE([ReportId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportId], NEW.[ReportId]), 0), '[[ReportId]] Is Null')),
				'[ReportId]',
				CONVERT(nvarchar(4000), OLD.[ReportId], 126),
				CONVERT(nvarchar(4000), NEW.[ReportId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportId], NEW.[ReportId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportId] = OLD.[ReportId] or (NEW.[ReportId] Is Null and OLD.[ReportId] Is Null))
			WHERE ((NEW.[ReportId] <> OLD.[ReportId]) 
					Or (NEW.[ReportId] Is Null And OLD.[ReportId] Is Not Null)
					Or (NEW.[ReportId] Is Not Null And OLD.[ReportId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [Name]
	IF UPDATE([Name]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportId], NEW.[ReportId]), 0), '[[ReportId]] Is Null')),
				'[Name]',
				CONVERT(nvarchar(4000), OLD.[Name], 126),
				CONVERT(nvarchar(4000), NEW.[Name], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportId], NEW.[ReportId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportId] = OLD.[ReportId] or (NEW.[ReportId] Is Null and OLD.[ReportId] Is Null))
			WHERE ((NEW.[Name] <> OLD.[Name]) 
					Or (NEW.[Name] Is Null And OLD.[Name] Is Not Null)
					Or (NEW.[Name] Is Not Null And OLD.[Name] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [Description]
	IF UPDATE([Description]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportId], NEW.[ReportId]), 0), '[[ReportId]] Is Null')),
				'[Description]',
				CONVERT(nvarchar(4000), OLD.[Description], 126),
				CONVERT(nvarchar(4000), NEW.[Description], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportId], NEW.[ReportId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportId] = OLD.[ReportId] or (NEW.[ReportId] Is Null and OLD.[ReportId] Is Null))
			WHERE ((NEW.[Description] <> OLD.[Description]) 
					Or (NEW.[Description] Is Null And OLD.[Description] Is Not Null)
					Or (NEW.[Description] Is Not Null And OLD.[Description] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [ReportGroupId]
	IF UPDATE([ReportGroupId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportId], NEW.[ReportId]), 0), '[[ReportId]] Is Null')),
				'[ReportGroupId]',
				CONVERT(nvarchar(4000), OLD.[ReportGroupId], 126),
				CONVERT(nvarchar(4000), NEW.[ReportGroupId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportId], NEW.[ReportId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportId] = OLD.[ReportId] or (NEW.[ReportId] Is Null and OLD.[ReportId] Is Null))
			WHERE ((NEW.[ReportGroupId] <> OLD.[ReportGroupId]) 
					Or (NEW.[ReportGroupId] Is Null And OLD.[ReportGroupId] Is Not Null)
					Or (NEW.[ReportGroupId] Is Not Null And OLD.[ReportGroupId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [Permission]
	IF UPDATE([Permission]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportId], NEW.[ReportId]), 0), '[[ReportId]] Is Null')),
				'[Permission]',
				CONVERT(nvarchar(4000), OLD.[Permission], 126),
				CONVERT(nvarchar(4000), NEW.[Permission], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportId], NEW.[ReportId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportId] = OLD.[ReportId] or (NEW.[ReportId] Is Null and OLD.[ReportId] Is Null))
			WHERE ((NEW.[Permission] <> OLD.[Permission]) 
					Or (NEW.[Permission] Is Null And OLD.[Permission] Is Not Null)
					Or (NEW.[Permission] Is Not Null And OLD.[Permission] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

END
GO
