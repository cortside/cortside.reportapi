IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spAddReportArgumentQuery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spAddReportArgumentQuery]
GO

if exists (select 1 from dbo.sysobjects where id = object_id(N'[dbo].[spAddReportArgumentQuery]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.spAddReportArgumentQuery
GO

create procedure dbo.spAddReportArgumentQuery
	@reportArgQuery varchar(1000)
as	
	if not exists (select * from ReportArgumentQuery where ArgQuery = @reportArgQuery)
		begin
			insert into ReportArgumentQuery (ArgQuery) values (@reportArgQuery)
			return (select SCOPE_IDENTITY())
		end
	else
		return (select ReportArgumentQueryId from ReportArgumentQuery where ArgQuery = @reportArgQuery)
go
