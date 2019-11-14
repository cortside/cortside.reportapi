IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spAddReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spAddReport]
GO

if exists (select 1 from dbo.sysobjects where id = object_id(N'[dbo].[spAddReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.spAddReport
GO

create procedure dbo.spAddReport
	@name varchar(100),
	@description varchar(100),
	@reportGroupId int

as
	print N'Adding report permission: ' + @name
	if not exists (select * from Report where Name = @name and ReportGroupId = @reportGroupId)
		begin
			declare @permissionId int
			select @permissionId = PermissionId from Permission where Name = @name and Description = @description
			if(@permissionId is null)
			begin
				insert into Permission (Name, Description) values (@name, @description)
				select @permissionId = SCOPE_IDENTITY()
			end

			insert into Report (Name, [Description], ReportGroupId, PermissionId)
			values (@name, @description, @reportGroupId, @permissionId)
		
			return (select SCOPE_IDENTITY())
		end
	else
		begin
			print N'Report: ' + @name + ' already exists.'
			return (select ReportId from Report where Name = @name and ReportGroupId = @reportGroupId)
		end
go
