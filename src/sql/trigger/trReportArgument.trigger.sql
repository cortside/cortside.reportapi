DROP TRIGGER IF EXISTS dbo.trReportArgument
GO

---
-- Trigger for ReportArgument that will handle logging of both update and delete
-- NOTE: inserted data is current value in row if not altered
---
CREATE TRIGGER trReportArgument
	ON [dbo].[ReportArgument]
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
	values('ReportArgument', 'dbo', @action, CASE WHEN LEN(HOST_NAME()) < 1 THEN ' ' ELSE HOST_NAME() END,
		CASE WHEN LEN(APP_NAME()) < 1 THEN ' ' ELSE APP_NAME() END,
		SUSER_SNAME(), GETDATE(), @ROWS_COUNT, db_name(), @UserName, CURRENT_TRANSACTION_ID()
	)
	Set @AuditLogTransactionId = SCOPE_IDENTITY()

	-- [ReportArgumentId]
	IF UPDATE([ReportArgumentId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportArgumentId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportArgumentId], NEW.[ReportArgumentId]), 0), '[[ReportArgumentId]] Is Null')),
				'[ReportArgumentId]',
				CONVERT(nvarchar(4000), OLD.[ReportArgumentId], 126),
				CONVERT(nvarchar(4000), NEW.[ReportArgumentId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportArgumentId], NEW.[ReportArgumentId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportArgumentId] = OLD.[ReportArgumentId] or (NEW.[ReportArgumentId] Is Null and OLD.[ReportArgumentId] Is Null))
			WHERE ((NEW.[ReportArgumentId] <> OLD.[ReportArgumentId]) 
					Or (NEW.[ReportArgumentId] Is Null And OLD.[ReportArgumentId] Is Not Null)
					Or (NEW.[ReportArgumentId] Is Not Null And OLD.[ReportArgumentId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [ReportId]
	IF UPDATE([ReportId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportArgumentId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportArgumentId], NEW.[ReportArgumentId]), 0), '[[ReportArgumentId]] Is Null')),
				'[ReportId]',
				CONVERT(nvarchar(4000), OLD.[ReportId], 126),
				CONVERT(nvarchar(4000), NEW.[ReportId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportArgumentId], NEW.[ReportArgumentId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportArgumentId] = OLD.[ReportArgumentId] or (NEW.[ReportArgumentId] Is Null and OLD.[ReportArgumentId] Is Null))
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
				convert(nvarchar(1500), IsNull('[[ReportArgumentId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportArgumentId], NEW.[ReportArgumentId]), 0), '[[ReportArgumentId]] Is Null')),
				'[Name]',
				CONVERT(nvarchar(4000), OLD.[Name], 126),
				CONVERT(nvarchar(4000), NEW.[Name], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportArgumentId], NEW.[ReportArgumentId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportArgumentId] = OLD.[ReportArgumentId] or (NEW.[ReportArgumentId] Is Null and OLD.[ReportArgumentId] Is Null))
			WHERE ((NEW.[Name] <> OLD.[Name]) 
					Or (NEW.[Name] Is Null And OLD.[Name] Is Not Null)
					Or (NEW.[Name] Is Not Null And OLD.[Name] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [ArgName]
	IF UPDATE([ArgName]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportArgumentId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportArgumentId], NEW.[ReportArgumentId]), 0), '[[ReportArgumentId]] Is Null')),
				'[ArgName]',
				CONVERT(nvarchar(4000), OLD.[ArgName], 126),
				CONVERT(nvarchar(4000), NEW.[ArgName], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportArgumentId], NEW.[ReportArgumentId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportArgumentId] = OLD.[ReportArgumentId] or (NEW.[ReportArgumentId] Is Null and OLD.[ReportArgumentId] Is Null))
			WHERE ((NEW.[ArgName] <> OLD.[ArgName]) 
					Or (NEW.[ArgName] Is Null And OLD.[ArgName] Is Not Null)
					Or (NEW.[ArgName] Is Not Null And OLD.[ArgName] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [ArgType]
	IF UPDATE([ArgType]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportArgumentId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportArgumentId], NEW.[ReportArgumentId]), 0), '[[ReportArgumentId]] Is Null')),
				'[ArgType]',
				CONVERT(nvarchar(4000), OLD.[ArgType], 126),
				CONVERT(nvarchar(4000), NEW.[ArgType], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportArgumentId], NEW.[ReportArgumentId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportArgumentId] = OLD.[ReportArgumentId] or (NEW.[ReportArgumentId] Is Null and OLD.[ReportArgumentId] Is Null))
			WHERE ((NEW.[ArgType] <> OLD.[ArgType]) 
					Or (NEW.[ArgType] Is Null And OLD.[ArgType] Is Not Null)
					Or (NEW.[ArgType] Is Not Null And OLD.[ArgType] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [ReportArgumentQueryId]
	IF UPDATE([ReportArgumentQueryId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportArgumentId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportArgumentId], NEW.[ReportArgumentId]), 0), '[[ReportArgumentId]] Is Null')),
				'[ReportArgumentQueryId]',
				CONVERT(nvarchar(4000), OLD.[ReportArgumentQueryId], 126),
				CONVERT(nvarchar(4000), NEW.[ReportArgumentQueryId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportArgumentId], NEW.[ReportArgumentId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportArgumentId] = OLD.[ReportArgumentId] or (NEW.[ReportArgumentId] Is Null and OLD.[ReportArgumentId] Is Null))
			WHERE ((NEW.[ReportArgumentQueryId] <> OLD.[ReportArgumentQueryId]) 
					Or (NEW.[ReportArgumentQueryId] Is Null And OLD.[ReportArgumentQueryId] Is Not Null)
					Or (NEW.[ReportArgumentQueryId] Is Not Null And OLD.[ReportArgumentQueryId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [Sequence]
	IF UPDATE([Sequence]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[ReportArgumentId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[ReportArgumentId], NEW.[ReportArgumentId]), 0), '[[ReportArgumentId]] Is Null')),
				'[Sequence]',
				CONVERT(nvarchar(4000), OLD.[Sequence], 126),
				CONVERT(nvarchar(4000), NEW.[Sequence], 126),
				convert(nvarchar(4000), COALESCE(OLD.[ReportArgumentId], NEW.[ReportArgumentId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[ReportArgumentId] = OLD.[ReportArgumentId] or (NEW.[ReportArgumentId] Is Null and OLD.[ReportArgumentId] Is Null))
			WHERE ((NEW.[Sequence] <> OLD.[Sequence]) 
					Or (NEW.[Sequence] Is Null And OLD.[Sequence] Is Not Null)
					Or (NEW.[Sequence] Is Not Null And OLD.[Sequence] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

END
GO
