if not exists (select * from Permission)
  BEGIN
	INSERT INTO Permission (Name, Description) VALUES ('General Reports', 'Permission to view general reports')
  END