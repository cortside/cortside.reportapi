IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spAddReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spAddReport]
GO

if exists (select 1 from dbo.sysobjects where id = object_id(N'[dbo].[spAddReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.spAddReport
GO

create procedure dbo.spAddReport
	@name varchar(100),
	@description varchar(100),
	@reportGroupId int NULL = NULL,
    @reportGroupName varchar(100) NULL = NULL

as
    if (@reportGroupId is null and @reportGroupName is null)
      THROW 50000, 'Either @reportGroupId or @reportGroupName must be provided', 1;

    if (@reportGroupName is not null)
      begin
        if not exists (select * from ReportGroup where Name = @reportGroupName)
          begin
            insert into ReportGroup (Name) values(@reportGroupName)
            set @reportGroupId = (select SCOPE_IDENTITY())
          end
        else
          begin
            select @reportGroupId = ReportGroupId from ReportGroup where Name = @reportGroupName
          end
      end

	print N'Adding report: ' + @name
	if not exists (select * from Report where Name = @name and ReportGroupId = @reportGroupId)
		begin
			insert into Report (Name, [Description], ReportGroupId, Permission)
			values (@name, @description, @reportGroupId, @name)
		
			return (select SCOPE_IDENTITY())
		end
	else
		begin
			print N'Report: ' + @name + ' already exists.'
			return (select ReportId from Report where Name = @name and ReportGroupId = @reportGroupId)
		end
go
