if not exists (select * from dbo.sysobjects where id = object_id(N'[ReportArgument]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
  BEGIN
	CREATE TABLE ReportArgument (
		ReportArgumentId INT NOT NULL identity,
		ReportId INT NOT NULL,
		Name VarChar(100) NOT NULL,
		ArgName VarChar(100) NOT NULL,
		ArgType VarChar(50) NOT NULL,
		ReportArgumentQueryId Int NULL,
		Sequence Int NOT NULL,
		CONSTRAINT PK_ReportArgument PRIMARY KEY (ReportArgumentId),
		CONSTRAINT FK_ReportArgument_Report FOREIGN KEY (ReportId) REFERENCES Report (ReportId),
		CONSTRAINT FK_ReportArgument_ReportArgumentQuery FOREIGN KEY (ReportArgumentQueryId) REFERENCES ReportArgumentQuery (ReportArgumentQueryId),
		CONSTRAINT UN_ReportArgument_ReportId_Name UNIQUE (ReportId, Name),
		CONSTRAINT UN_ReportArgument_ReportId_ArgName UNIQUE (ReportId, ArgName),
		CONSTRAINT UN_ReportArgument_ReportId_Sequence UNIQUE (ReportId, Sequence)
	)
  END