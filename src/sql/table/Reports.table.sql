if not exists (select * from dbo.sysobjects where id = object_id(N'[Report]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
  BEGIN
	CREATE TABLE Report (
		ReportId INT NOT NULL Identity,
		Name VARCHAR(100) NOT NULL,
		Description VARCHAR(100) NOT NULL,
		ReportGroupId INT NULL,
		PermissionId INT NOT NULL,
		CONSTRAINT PK_Report PRIMARY KEY (ReportId),
		CONSTRAINT UN_Report_Name_Group UNIQUE (Name, ReportGroupId),
		CONSTRAINT FK_Report_Permission FOREIGN KEY (PermissionId) REFERENCES Permission (PermissionId),
		CONSTRAINT FK_Report_ReportGroup FOREIGN KEY (ReportGroupId) REFERENCES ReportGroup (ReportGroupId)
	)
  END