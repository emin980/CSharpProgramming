IF OBJECT_ID('dbo.EquipmentTypes','U') IS NULL
BEGIN
    CREATE TABLE dbo.EquipmentTypes(
        Id INT IDENTITY PRIMARY KEY,
        Name NVARCHAR(30) NOT NULL UNIQUE,
        Description NVARCHAR(300) NULL,
        IsDeleted BIT NOT NULL CONSTRAINT DF_EquipmentTypes_IsDeleted DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_EquipmentTypes_CreatedAt DEFAULT SYSUTCDATETIME()
    );

    INSERT dbo.EquipmentTypes(Name,Description)
    SELECT DISTINCT EquipmentType,N'Mevcut ekipman kayıtlarından aktarıldı.'
    FROM dbo.Equipment
    WHERE EquipmentType IS NOT NULL AND EquipmentType<>'';

    INSERT dbo.EquipmentTypes(Name,Description)
    SELECT seed.Name,seed.Description
    FROM (VALUES
        (N'Drone',N'İnsansız hava araçları'),
        (N'Sensor Kit',N'Sensör ve ölçüm takımları'),
        (N'Tool',N'El aletleri ve teknik araçlar')
    ) seed(Name,Description)
    WHERE NOT EXISTS(SELECT 1 FROM dbo.EquipmentTypes currentType WHERE currentType.Name=seed.Name);
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_EquipmentType_GetAll
AS
SELECT Id,Name,Description,CreatedAt
FROM dbo.EquipmentTypes
WHERE IsDeleted=0
ORDER BY Name;
GO
CREATE OR ALTER PROCEDURE dbo.usp_EquipmentType_Create
    @Name NVARCHAR(30),
    @Description NVARCHAR(300)=NULL
AS
BEGIN
    DECLARE @ExistingId INT;
    SELECT @ExistingId=Id FROM dbo.EquipmentTypes WHERE Name=@Name;
    IF @ExistingId IS NOT NULL
    BEGIN
        IF EXISTS(SELECT 1 FROM dbo.EquipmentTypes WHERE Id=@ExistingId AND IsDeleted=0)
            THROW 51020,'Bu materyal türü daha önce tanımlanmıştır.',1;
        UPDATE dbo.EquipmentTypes SET IsDeleted=0,Description=@Description WHERE Id=@ExistingId;
        SELECT @ExistingId;
        RETURN;
    END;
    INSERT dbo.EquipmentTypes(Name,Description) VALUES(@Name,@Description);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_EquipmentType_Update
    @Id INT,
    @Name NVARCHAR(30),
    @Description NVARCHAR(300)=NULL
AS
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRAN;
    DECLARE @OldName NVARCHAR(30);
    SELECT @OldName=Name FROM dbo.EquipmentTypes WHERE Id=@Id AND IsDeleted=0;
    IF @OldName IS NULL THROW 51021,'Materyal türü bulunamadı.',1;
    UPDATE dbo.Equipment SET EquipmentType=@Name WHERE EquipmentType=@OldName AND IsDeleted=0;
    UPDATE dbo.EquipmentTypes SET Name=@Name,Description=@Description WHERE Id=@Id;
    COMMIT;
    SELECT 1;
END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_EquipmentType_Delete
    @Id INT
AS
BEGIN
    DECLARE @Name NVARCHAR(30);
    SELECT @Name=Name FROM dbo.EquipmentTypes WHERE Id=@Id AND IsDeleted=0;
    IF EXISTS(SELECT 1 FROM dbo.Equipment WHERE EquipmentType=@Name AND IsDeleted=0)
        THROW 51022,'Kullanımda olan materyal türü silinemez.',1;
    UPDATE dbo.EquipmentTypes SET IsDeleted=1 WHERE Id=@Id AND IsDeleted=0;
    SELECT @@ROWCOUNT;
END;
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
    IF NOT EXISTS(SELECT 1 FROM dbo.EquipmentTypes WHERE Name=@EquipmentType AND IsDeleted=0)
        THROW 51023,'Geçerli bir materyal türü seçilmelidir.',1;
    INSERT dbo.Equipment(Name,SerialNumber,EquipmentType,Notes,PurchasePrice,ImageData)
    VALUES(@Name,@SerialNumber,@EquipmentType,@Notes,@PurchasePrice,@ImageData);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
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
    IF NOT EXISTS(SELECT 1 FROM dbo.EquipmentTypes WHERE Name=@EquipmentType AND IsDeleted=0)
        THROW 51023,'Geçerli bir materyal türü seçilmelidir.',1;
    UPDATE dbo.Equipment
    SET Name=@Name,SerialNumber=COALESCE(@SerialNumber,SerialNumber),EquipmentType=@EquipmentType,
        Status=@Status,Notes=@Notes,PurchasePrice=@PurchasePrice,ImageData=COALESCE(@ImageData,ImageData)
    WHERE Id=@Id AND IsDeleted=0;
    SELECT @@ROWCOUNT;
END;
GO
