if not exists (select * from reportgroup)
  BEGIN
	INSERT INTO ReportGroup (Name) VALUES ('1 - General')
  END

declare @reportGroupId int
declare @reportId int
declare @schemaQueryId int
declare @name nvarchar(50)

set @name = 'spReport_Tables'
exec @schemaQueryId = spAddReportArgumentQuery 'select distinct TABLE_SCHEMA, TABLE_SCHEMA from INFORMATION_SCHEMA.TABLES'

set @reportGroupId = (select reportgroupid from reportgroup where name='1 - General') 
exec @reportId = spAddReport @name, 'List Tables', @reportGroupId
exec spAddReportArgument @reportId, 'Schema', '@schema', 'varchar(256)', @schemaQueryId, 1

-------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spReport_Tables]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].spReport_Tables
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].spReport_Tables
	@schema varchar(256)=null
AS
BEGIN
	SET NOCOUNT ON;

    select *
    from INFORMATION_SCHEMA.TABLES
    where TABLE_SCHEMA=@schema or @schema is null
    order by TABLE_SCHEMA, TABLE_NAME
END
GO


select * from Report
select * from ReportArgument
select * from ReportArgumentQuery

update ReportArgumentQuery set ArgQuery='select distinct TABLE_SCHEMA, TABLE_SCHEMA from INFORMATION_SCHEMA.TABLES'
