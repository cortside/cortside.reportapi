if not exists (select * from dbo.sysobjects where id = object_id(N'[Permission]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
  BEGIN
	CREATE TABLE Permission (
		PermissionId INT NOT NULL identity,
		Name VarChar(200) NOT NULL,
		Description VarChar(200) NULL, 
		CONSTRAINT PK_Permission PRIMARY KEY (PermissionId)
	)
  END