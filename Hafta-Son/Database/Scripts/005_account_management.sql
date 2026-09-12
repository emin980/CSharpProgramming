CREATE OR ALTER PROCEDURE dbo.usp_User_UpdatePassword
    @Id INT,
    @PasswordHash VARBINARY(64),
    @PasswordSalt VARBINARY(64)
AS
BEGIN
    UPDATE dbo.Users
    SET PasswordHash=@PasswordHash,PasswordSalt=@PasswordSalt
    WHERE Id=@Id AND IsActive=1;
    SELECT @@ROWCOUNT;
END;
GO
