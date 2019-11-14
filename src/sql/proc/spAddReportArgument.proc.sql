if exists (select 1 from dbo.sysobjects where id = object_id(N'[dbo].[spAddReportArgument]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.spAddReportArgument
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spAddReportArgument]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spAddReportArgument]
GO

create procedure dbo.spAddReportArgument
	@reportId int,
	@displayName varchar(100),
	@paramName varchar(100),
	@type varchar(50),
	@reportArgQueryId int,
	@sequence int
as
select * from reportargument

	print N'Adding report argument: ' + @displayName + ' to report: ' + convert(varchar, @reportId)
	if not exists(select 1 from ReportArgument where ReportId = @reportId and ArgName = @paramName)
	begin
		insert into [ReportArgument] ([ReportId], [Name], [ArgName], [ArgType], [ReportArgumentQueryId], [Sequence])
		values (@reportId, @displayName, @paramName, @type, @reportArgQueryId, @sequence)
	end
	else 
	begin
		update [ReportArgument] set Name = @displayName, [ArgType] = @type, [ReportArgumentQueryId] = @reportArgQueryId, [Sequence] = @sequence
		where ReportId = @reportId and ArgName = @paramName
	end
go