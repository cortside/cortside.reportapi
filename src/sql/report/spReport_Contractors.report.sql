IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spReport_Contractors]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].spReport_Contractors
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].spReport_Contractors 
	@startDate DATETIME=null,
	@endDate DATETIME=null,
	@state varchar(2)=null
AS
BEGIN
	SET NOCOUNT ON;

	select *
	from eboa.dbo.contractor
	where effectivedate between coalesce(@startDate, '1/1/1970') and coalesce(@endDate, '12/31/2199')
		and (state=@state or @state is null)
	order by Name

END


GO
