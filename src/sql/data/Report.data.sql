declare @reportGroupId int
declare @reportId int
declare @stateQueryId int

exec @stateQueryId = spAddReportArgumentQuery 'select State ID, State Name from Contractor order by State'

set @reportGroupId = 1
exec @reportId = spAddReport 'spReport_Contractors', 'List Contractors', @reportGroupId
	exec spAddReportArgument @reportId, 'Start Date', '@startdate', 'DateTime', null, 1
	exec spAddReportArgument @reportId, 'End Date', '@enddate', 'DateTime', null, 2
	exec spAddReportArgument @reportId, 'State', '@shippingMethodId', 'Int', @stateQueryId, 3

select * from report
select * from reportArgument
select * from reportargumentquery



select * from reportargumentquery

update reportargumentquery set argquery='select distinct State ID, State Name from eboa.dbo.Contractor order by State'


update reportargument set argname='@state' where reportargumentid=3