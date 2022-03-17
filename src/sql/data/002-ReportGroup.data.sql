if not exists (select * from reportgroup)
  BEGIN
	INSERT INTO ReportGroup (Name) VALUES ('1 - General')
  END
