CREATE PROCEDURE [dbo].[UpdatePersonalInfo]
	@MiddleName nvarchar(100),
	@Id int
AS
	UPDATE dbo.NameTable SET 
	MiddleName = @MiddleName
	WHERE Id = @Id
RETURN 0
