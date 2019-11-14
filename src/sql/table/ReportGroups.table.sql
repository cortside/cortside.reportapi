if not exists (select * from dbo.sysobjects where id = object_id(N'[ReportGroup]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
  BEGIN
	CREATE TABLE ReportGroup (
		ReportGroupId INT NOT NULL identity,
		Name VARCHAR(200) NOT NULL,
		CONSTRAINT PK_ReportGroup PRIMARY KEY (ReportGroupId),
		CONSTRAINT UN_ReportGroup_Name UNIQUE (Name)
	)
  END