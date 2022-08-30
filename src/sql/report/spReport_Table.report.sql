if not exists (select * from reportgroup)
  BEGIN
	INSERT INTO ReportGroup (Name) VALUES ('1 - General')
  END

declare @reportGroupId int
declare @reportId int
declare @stateQueryId int
declare @name nvarchar(50)

set @name = 'spReport_Table'
set @reportGroupId = (select reportgroupid from reportgroup where name='1 - General') 
exec @reportId = spAddReport @name, 'Table Details', @reportGroupId
exec spAddReportArgument @reportId, 'Table Name', '@table', 'varchar(256)', null, 1

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spReport_Table]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].spReport_Table
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].spReport_Table
	@table varchar(256)=null
AS
BEGIN
	SET NOCOUNT ON;

    select *
    from INFORMATION_SCHEMA.TABLES
    where TABLE_NAME like '%' + coalesce(@table, '') + '%' OR @table IS NULL
    order by TABLE_SCHEMA, TABLE_NAME

    select *
    from INFORMATION_SCHEMA.COLUMNS
    where TABLE_NAME like '%' + coalesce(@table, '') + '%' OR @table IS NULL
    order by TABLE_SCHEMA, TABLE_NAME, ORDINAL_POSITION
END
GO
