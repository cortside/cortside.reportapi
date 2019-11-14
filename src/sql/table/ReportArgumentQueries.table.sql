if not exists (select * from dbo.sysobjects where id = object_id(N'[ReportArgumentQuery]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
  BEGIN
	CREATE TABLE ReportArgumentQuery (
		ReportArgumentQueryId INT NOT NULL Identity,
		ArgQuery VarChar(700) NULL,
		CONSTRAINT PK_ReportArgumentQuery PRIMARY KEY (ReportArgumentQueryId),
		CONSTRAINT UN_ReportArgumentQuery_ArgQuery UNIQUE (ArgQuery)
	)
  END