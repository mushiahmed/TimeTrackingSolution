
CREATE LOGIN timetracking_read WITH PASSWORD = 'test123'
GO

CREATE USER timetracking_read FOR LOGIN timetracking_read

GO
EXEC sp_addrolemember 'db_datareader', 'timetracking_read'

GO
CREATE LOGIN timetracking_write WITH PASSWORD = 'test123'
GO

CREATE USER timetracking_write FOR LOGIN timetracking_write
GO
EXEC sp_addrolemember 'db_datareader', 'timetracking_write'
EXEC sp_addrolemember 'db_datawriter', 'timetracking_write'
