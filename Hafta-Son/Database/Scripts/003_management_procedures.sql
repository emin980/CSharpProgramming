IF OBJECT_ID('dbo.MissionPersonnel','U') IS NULL
    CREATE TABLE dbo.MissionPersonnel(
        MissionId INT NOT NULL REFERENCES dbo.Missions(Id),
        PersonnelId INT NOT NULL REFERENCES dbo.Personnel(Id),
        CONSTRAINT PK_MissionPersonnel PRIMARY KEY(MissionId,PersonnelId)
    );
GO
CREATE OR ALTER PROCEDURE dbo.usp_User_SetRole
    @Id INT,
    @RoleName NVARCHAR(40)
AS
BEGIN
    UPDATE dbo.Users
    SET RoleId=(SELECT Id FROM dbo.Roles WHERE Name=@RoleName)
    WHERE Id=@Id;
    SELECT @@ROWCOUNT;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Personnel_Update
    @Id INT,
    @FullName NVARCHAR(100),
    @Department NVARCHAR(100)=NULL,
    @Phone NVARCHAR(30)=NULL,
    @IsActive BIT
AS
BEGIN
    UPDATE dbo.Personnel
    SET FullName=@FullName,Department=@Department,Phone=@Phone,IsActive=@IsActive
    WHERE Id=@Id;
    SELECT @@ROWCOUNT;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Personnel_Delete
    @Id INT
AS
BEGIN
    IF EXISTS(SELECT 1 FROM dbo.Checkouts WHERE PersonnelId=@Id)
        THROW 51005,'Geçmiş hareketi bulunan personel silinemez; pasif duruma getirilebilir.',1;
    DELETE dbo.Personnel WHERE Id=@Id;
    SELECT @@ROWCOUNT;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Mission_AssignPersonnel
    @MissionId INT,
    @PersonnelId INT
AS
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo.Personnel WHERE Id=@PersonnelId AND IsActive=1)
        THROW 51006,'Etkin personel kaydı bulunamadı.',1;
    INSERT dbo.MissionPersonnel(MissionId,PersonnelId) VALUES(@MissionId,@PersonnelId);
    SELECT 1;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Mission_SetStatus
    @Id INT,
    @Status NVARCHAR(30)
AS
BEGIN
    IF @Status NOT IN ('Planned','Active','Completed','Cancelled')
        THROW 51007,'Geçersiz görev durumu.',1;
    UPDATE dbo.Missions SET Status=@Status WHERE Id=@Id;
    SELECT @@ROWCOUNT;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Mission_GetAll
AS
SELECT m.Id,m.Name,m.Description,m.PlannedAt,m.Status,
       COUNT(DISTINCT me.EquipmentId) EquipmentCount,
       COUNT(DISTINCT mp.PersonnelId) PersonnelCount
FROM dbo.Missions m
LEFT JOIN dbo.MissionEquipment me ON me.MissionId=m.Id
LEFT JOIN dbo.MissionPersonnel mp ON mp.MissionId=m.Id
GROUP BY m.Id,m.Name,m.Description,m.PlannedAt,m.Status
ORDER BY m.PlannedAt;
GO
