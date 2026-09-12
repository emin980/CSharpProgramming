IF COL_LENGTH('dbo.Equipment','IsDeleted') IS NULL
    ALTER TABLE dbo.Equipment ADD IsDeleted BIT NOT NULL CONSTRAINT DF_Equipment_IsDeleted DEFAULT 0;
IF COL_LENGTH('dbo.Equipment','PurchasePrice') IS NULL
    ALTER TABLE dbo.Equipment ADD PurchasePrice DECIMAL(18,2) NULL;
IF COL_LENGTH('dbo.Equipment','ImageData') IS NULL
    ALTER TABLE dbo.Equipment ADD ImageData VARBINARY(MAX) NULL;
IF COL_LENGTH('dbo.Personnel','IsDeleted') IS NULL
    ALTER TABLE dbo.Personnel ADD IsDeleted BIT NOT NULL CONSTRAINT DF_Personnel_IsDeleted DEFAULT 0;
IF COL_LENGTH('dbo.Personnel','ImageData') IS NULL
    ALTER TABLE dbo.Personnel ADD ImageData VARBINARY(MAX) NULL;
IF COL_LENGTH('dbo.Missions','IsDeleted') IS NULL
    ALTER TABLE dbo.Missions ADD IsDeleted BIT NOT NULL CONSTRAINT DF_Missions_IsDeleted DEFAULT 0;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Create
    @Name NVARCHAR(100),
    @SerialNumber NVARCHAR(30),
    @EquipmentType NVARCHAR(30),
    @Notes NVARCHAR(500)=NULL,
    @PurchasePrice DECIMAL(18,2)=NULL,
    @ImageData VARBINARY(MAX)=NULL
AS
BEGIN
    INSERT dbo.Equipment(Name,SerialNumber,EquipmentType,Notes,PurchasePrice,ImageData)
    VALUES(@Name,@SerialNumber,@EquipmentType,@Notes,@PurchasePrice,@ImageData);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Search
    @Search NVARCHAR(100)=NULL,
    @EquipmentType NVARCHAR(30)=NULL,
    @Status NVARCHAR(30)=NULL
AS
SELECT Id,Name,SerialNumber,EquipmentType,Status,PurchasePrice,Notes,CreatedAt,
       CAST(CASE WHEN ImageData IS NULL THEN 0 ELSE 1 END AS BIT) HasImage
FROM dbo.Equipment
WHERE IsDeleted=0
  AND (@Search IS NULL OR @Search='' OR Name LIKE '%'+@Search+'%' OR SerialNumber LIKE '%'+@Search+'%')
  AND (@EquipmentType IS NULL OR @EquipmentType='' OR EquipmentType=@EquipmentType)
  AND (@Status IS NULL OR @Status='' OR Status=@Status)
ORDER BY Name;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Update
    @Id INT,
    @Name NVARCHAR(100),
    @EquipmentType NVARCHAR(30),
    @Status NVARCHAR(30),
    @Notes NVARCHAR(500)=NULL,
    @PurchasePrice DECIMAL(18,2)=NULL,
    @ImageData VARBINARY(MAX)=NULL,
    @SerialNumber NVARCHAR(30)=NULL
AS
BEGIN
    UPDATE dbo.Equipment
    SET Name=@Name,SerialNumber=COALESCE(@SerialNumber,SerialNumber),EquipmentType=@EquipmentType,Status=@Status,Notes=@Notes,
        PurchasePrice=@PurchasePrice,ImageData=COALESCE(@ImageData,ImageData)
    WHERE Id=@Id AND IsDeleted=0;
    SELECT @@ROWCOUNT;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Delete
    @Id INT
AS
BEGIN
    IF EXISTS(SELECT 1 FROM dbo.Checkouts WHERE EquipmentId=@Id AND ReturnedAt IS NULL)
        THROW 51008,'Aktif zimmeti bulunan ekipman silinemez.',1;
    IF EXISTS(SELECT 1 FROM dbo.MaintenanceRecords WHERE EquipmentId=@Id AND CompletedAt IS NULL)
        THROW 51009,'Açık bakım kaydı bulunan ekipman silinemez.',1;
    UPDATE dbo.Equipment SET IsDeleted=1 WHERE Id=@Id AND IsDeleted=0;
    SELECT @@ROWCOUNT;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_GetImage
    @Id INT
AS
SELECT ImageData FROM dbo.Equipment WHERE Id=@Id AND IsDeleted=0;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Personnel_Create
    @FullName NVARCHAR(100),
    @Department NVARCHAR(100)=NULL,
    @Phone NVARCHAR(30)=NULL,
    @ImageData VARBINARY(MAX)=NULL
AS
BEGIN
    INSERT dbo.Personnel(FullName,Department,Phone,ImageData)
    VALUES(@FullName,@Department,@Phone,@ImageData);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Personnel_GetAll
AS
SELECT Id,FullName,Department,Phone,IsActive,
       CAST(CASE WHEN ImageData IS NULL THEN 0 ELSE 1 END AS BIT) HasImage
FROM dbo.Personnel
WHERE IsDeleted=0
ORDER BY FullName;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Personnel_Update
    @Id INT,
    @FullName NVARCHAR(100),
    @Department NVARCHAR(100)=NULL,
    @Phone NVARCHAR(30)=NULL,
    @IsActive BIT,
    @ImageData VARBINARY(MAX)=NULL
AS
BEGIN
    UPDATE dbo.Personnel
    SET FullName=@FullName,Department=@Department,Phone=@Phone,IsActive=@IsActive,
        ImageData=COALESCE(@ImageData,ImageData)
    WHERE Id=@Id AND IsDeleted=0;
    SELECT @@ROWCOUNT;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Personnel_Delete
    @Id INT
AS
BEGIN
    IF EXISTS(SELECT 1 FROM dbo.Checkouts WHERE PersonnelId=@Id AND ReturnedAt IS NULL)
        THROW 51010,'Aktif zimmeti bulunan personel silinemez.',1;
    UPDATE dbo.Personnel SET IsDeleted=1,IsActive=0 WHERE Id=@Id AND IsDeleted=0;
    SELECT @@ROWCOUNT;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Personnel_GetImage
    @Id INT
AS
SELECT ImageData FROM dbo.Personnel WHERE Id=@Id AND IsDeleted=0;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Checkout_Create
    @EquipmentId INT,
    @PersonnelId INT,
    @UserId INT,
    @DueAt DATETIME2
AS
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRAN;
    IF NOT EXISTS(SELECT 1 FROM dbo.Equipment WHERE Id=@EquipmentId AND Status='Available' AND IsDeleted=0)
        THROW 51000,'Ekipman zimmete uygun değildir.',1;
    IF NOT EXISTS(SELECT 1 FROM dbo.Personnel WHERE Id=@PersonnelId AND IsActive=1 AND IsDeleted=0)
        THROW 51011,'Etkin personel kaydı bulunamadı.',1;
    INSERT dbo.Checkouts(EquipmentId,PersonnelId,CheckedOutByUserId,DueAt)
    VALUES(@EquipmentId,@PersonnelId,@UserId,@DueAt);
    DECLARE @NewId INT=CAST(SCOPE_IDENTITY() AS INT);
    UPDATE dbo.Equipment SET Status='Assigned' WHERE Id=@EquipmentId;
    COMMIT;
    SELECT @NewId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Maintenance_Open
    @EquipmentId INT,
    @UserId INT,
    @Description NVARCHAR(500)
AS
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRAN;
    IF NOT EXISTS(SELECT 1 FROM dbo.Equipment WHERE Id=@EquipmentId AND Status='Available' AND IsDeleted=0)
        THROW 51002,'Ekipman bakıma alınmaya uygun değildir.',1;
    INSERT dbo.MaintenanceRecords(EquipmentId,OpenedByUserId,Description)
    VALUES(@EquipmentId,@UserId,@Description);
    DECLARE @NewId INT=CAST(SCOPE_IDENTITY() AS INT);
    UPDATE dbo.Equipment SET Status='InMaintenance' WHERE Id=@EquipmentId;
    COMMIT;
    SELECT @NewId;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Mission_AssignEquipment
    @MissionId INT,
    @EquipmentId INT
AS
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo.Missions WHERE Id=@MissionId AND IsDeleted=0)
        THROW 51012,'Görev kaydı bulunamadı.',1;
    IF NOT EXISTS(SELECT 1 FROM dbo.Equipment WHERE Id=@EquipmentId AND IsDeleted=0 AND Status NOT IN('InMaintenance','Lost'))
        THROW 51004,'Ekipman göreve uygun değildir.',1;
    INSERT dbo.MissionEquipment(MissionId,EquipmentId) VALUES(@MissionId,@EquipmentId);
    SELECT 1;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Mission_AssignPersonnel
    @MissionId INT,
    @PersonnelId INT
AS
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo.Missions WHERE Id=@MissionId AND IsDeleted=0)
        THROW 51012,'Görev kaydı bulunamadı.',1;
    IF NOT EXISTS(SELECT 1 FROM dbo.Personnel WHERE Id=@PersonnelId AND IsActive=1 AND IsDeleted=0)
        THROW 51006,'Etkin personel kaydı bulunamadı.',1;
    INSERT dbo.MissionPersonnel(MissionId,PersonnelId) VALUES(@MissionId,@PersonnelId);
    SELECT 1;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Mission_GetAll
AS
SELECT m.Id,m.Name,m.Description,m.PlannedAt,m.Status,
       COUNT(DISTINCT me.EquipmentId) EquipmentCount,
       COUNT(DISTINCT mp.PersonnelId) PersonnelCount
FROM dbo.Missions m
LEFT JOIN dbo.MissionEquipment me ON me.MissionId=m.Id
    AND EXISTS(SELECT 1 FROM dbo.Equipment e WHERE e.Id=me.EquipmentId AND e.IsDeleted=0)
LEFT JOIN dbo.MissionPersonnel mp ON mp.MissionId=m.Id
    AND EXISTS(SELECT 1 FROM dbo.Personnel p WHERE p.Id=mp.PersonnelId AND p.IsDeleted=0)
WHERE m.IsDeleted=0
GROUP BY m.Id,m.Name,m.Description,m.PlannedAt,m.Status
ORDER BY m.PlannedAt;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Checkout_GetAll
AS
SELECT c.Id,e.SerialNumber,e.Name Equipment,p.FullName Personnel,
       c.CheckedOutAt,c.DueAt,c.ReturnedAt,c.ReturnNotes
FROM dbo.Checkouts c
JOIN dbo.Equipment e ON e.Id=c.EquipmentId AND e.IsDeleted=0
JOIN dbo.Personnel p ON p.Id=c.PersonnelId AND p.IsDeleted=0
ORDER BY c.Id DESC;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Maintenance_GetAll
AS
SELECT m.Id,e.SerialNumber,e.Name Equipment,m.Description,m.OpenedAt,m.CompletedAt,m.Result
FROM dbo.MaintenanceRecords m
JOIN dbo.Equipment e ON e.Id=m.EquipmentId AND e.IsDeleted=0
ORDER BY m.Id DESC;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Report_Overdue
AS
SELECT c.Id,e.SerialNumber,e.Name Equipment,p.FullName Personnel,c.DueAt,
       DATEDIFF(DAY,c.DueAt,SYSUTCDATETIME()) OverdueDays
FROM dbo.Checkouts c
JOIN dbo.Equipment e ON e.Id=c.EquipmentId AND e.IsDeleted=0
JOIN dbo.Personnel p ON p.Id=c.PersonnelId AND p.IsDeleted=0
WHERE c.ReturnedAt IS NULL AND c.DueAt<SYSUTCDATETIME();
GO
CREATE OR ALTER PROCEDURE dbo.usp_Report_Dashboard
AS
SELECT
 (SELECT COUNT(*) FROM dbo.Equipment WHERE IsDeleted=0) EquipmentCount,
 (SELECT COUNT(*) FROM dbo.Equipment WHERE IsDeleted=0 AND Status='Available') AvailableCount,
 (SELECT COUNT(*) FROM dbo.Checkouts c JOIN dbo.Equipment e ON e.Id=c.EquipmentId WHERE c.ReturnedAt IS NULL AND e.IsDeleted=0) ActiveCheckoutCount,
 (SELECT COUNT(*) FROM dbo.MaintenanceRecords m JOIN dbo.Equipment e ON e.Id=m.EquipmentId WHERE m.CompletedAt IS NULL AND e.IsDeleted=0) OpenMaintenanceCount,
 (SELECT COUNT(*) FROM dbo.Missions WHERE IsDeleted=0 AND Status='Planned') PlannedMissionCount;
GO
