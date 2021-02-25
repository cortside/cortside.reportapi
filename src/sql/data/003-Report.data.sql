declare @reportGroupId int
declare @reportId int
declare @stateQueryId int

update ReportArgumentQuery set ArgQuery='select distinct a.St ID, a.St State from eboa.dbo.Contractor c join eboa.dbo.Address a on c.MailingAddressId=a.AddressID' where ReportArgumentQueryId=2
exec @stateQueryId = spAddReportArgumentQuery 'select distinct a.St ID, a.St State from eboa.dbo.Contractor c join eboa.dbo.Address a on c.MailingAddressId=a.AddressID'

set @reportGroupId = 1
exec @reportId = spAddReport 'spReport_Contractors', 'List Contractors', @reportGroupId
	exec spAddReportArgument @reportId, 'Start Date', '@startdate', 'DateTime', null, 1
	exec spAddReportArgument @reportId, 'End Date', '@enddate', 'DateTime', null, 2
	exec spAddReportArgument @reportId, 'State', '@state', 'varchar(2)', @stateQueryId, 3

update ReportArgument set ArgName='@state', ArgType='varchar(2)' where ReportArgumentId=6

delete from ReportArgumentQuery where ReportArgumentQueryId not in (select ReportArgumentQueryId from ReportArgument where ReportArgumentQueryId is not null)

--select * from report
--select * from reportArgument
--select * from reportargumentquery



--select * from reportargumentquery

--update reportargumentquery set argquery='select distinct st ID, st Name from c1_qa_eboa.dbo.address where st is not null and st !=''  '' order by st'


--update reportargument set argname='@state' where reportargumentid=3

