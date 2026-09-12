CREATE OR ALTER PROCEDURE dbo.usp_User_Count AS SELECT COUNT(*) FROM dbo.Users;
GO
CREATE OR ALTER PROCEDURE dbo.usp_User_Create @Username NVARCHAR(50),@DisplayName NVARCHAR(100),@PasswordHash VARBINARY(64),@PasswordSalt VARBINARY(64),@RoleName NVARCHAR(40) AS
BEGIN INSERT dbo.Users(Username,DisplayName,PasswordHash,PasswordSalt,RoleId) SELECT @Username,@DisplayName,@PasswordHash,@PasswordSalt,Id FROM dbo.Roles WHERE Name=@RoleName; SELECT CAST(SCOPE_IDENTITY() AS INT); END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_User_GetByUsername @Username NVARCHAR(50) AS SELECT u.Id,u.Username,u.DisplayName,u.PasswordHash,u.PasswordSalt,r.Name RoleName,u.IsActive FROM dbo.Users u JOIN dbo.Roles r ON r.Id=u.RoleId WHERE u.Username=@Username;
GO
CREATE OR ALTER PROCEDURE dbo.usp_User_GetAll AS SELECT u.Id,u.Username,u.DisplayName,r.Name Role,u.IsActive,u.CreatedAt FROM dbo.Users u JOIN dbo.Roles r ON r.Id=u.RoleId ORDER BY u.Username;
GO
CREATE OR ALTER PROCEDURE dbo.usp_User_SetActive @Id INT,@IsActive BIT AS UPDATE dbo.Users SET IsActive=@IsActive WHERE Id=@Id; SELECT @@ROWCOUNT;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Create @Name NVARCHAR(100),@SerialNumber NVARCHAR(30),@EquipmentType NVARCHAR(30),@Notes NVARCHAR(500)=NULL AS BEGIN INSERT dbo.Equipment(Name,SerialNumber,EquipmentType,Notes) VALUES(@Name,@SerialNumber,@EquipmentType,@Notes); SELECT CAST(SCOPE_IDENTITY() AS INT); END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Search @Search NVARCHAR(100)=NULL AS SELECT * FROM dbo.Equipment WHERE @Search IS NULL OR @Search='' OR Name LIKE '%'+@Search+'%' OR SerialNumber LIKE '%'+@Search+'%' ORDER BY Name;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Update @Id INT,@Name NVARCHAR(100),@EquipmentType NVARCHAR(30),@Status NVARCHAR(30),@Notes NVARCHAR(500)=NULL AS UPDATE dbo.Equipment SET Name=@Name,EquipmentType=@EquipmentType,Status=@Status,Notes=@Notes WHERE Id=@Id; SELECT @@ROWCOUNT;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Delete @Id INT AS DELETE dbo.Equipment WHERE Id=@Id; SELECT @@ROWCOUNT;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Personnel_Create @FullName NVARCHAR(100),@Department NVARCHAR(100)=NULL,@Phone NVARCHAR(30)=NULL AS BEGIN INSERT dbo.Personnel(FullName,Department,Phone) VALUES(@FullName,@Department,@Phone); SELECT CAST(SCOPE_IDENTITY() AS INT); END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Personnel_GetAll AS SELECT * FROM dbo.Personnel ORDER BY FullName;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Checkout_Create @EquipmentId INT,@PersonnelId INT,@UserId INT,@DueAt DATETIME2 AS
BEGIN SET XACT_ABORT ON; BEGIN TRAN; IF NOT EXISTS(SELECT 1 FROM dbo.Equipment WHERE Id=@EquipmentId AND Status='Available') THROW 51000,'Ekipman zimmete uygun değildir.',1; INSERT dbo.Checkouts(EquipmentId,PersonnelId,CheckedOutByUserId,DueAt) VALUES(@EquipmentId,@PersonnelId,@UserId,@DueAt); UPDATE dbo.Equipment SET Status='Assigned' WHERE Id=@EquipmentId; COMMIT; SELECT CAST(SCOPE_IDENTITY() AS INT); END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Checkout_Return @CheckoutId INT,@ReturnNotes NVARCHAR(500)=NULL AS
BEGIN SET XACT_ABORT ON; BEGIN TRAN; DECLARE @EquipmentId INT; SELECT @EquipmentId=EquipmentId FROM dbo.Checkouts WHERE Id=@CheckoutId AND ReturnedAt IS NULL; IF @EquipmentId IS NULL THROW 51001,'Açık zimmet bulunamadı.',1; UPDATE dbo.Checkouts SET ReturnedAt=SYSUTCDATETIME(),ReturnNotes=@ReturnNotes WHERE Id=@CheckoutId; UPDATE dbo.Equipment SET Status='Available' WHERE Id=@EquipmentId; COMMIT; SELECT 1; END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Checkout_GetAll AS SELECT c.Id,e.SerialNumber,e.Name Equipment,p.FullName Personnel,c.CheckedOutAt,c.DueAt,c.ReturnedAt,c.ReturnNotes FROM dbo.Checkouts c JOIN dbo.Equipment e ON e.Id=c.EquipmentId JOIN dbo.Personnel p ON p.Id=c.PersonnelId ORDER BY c.Id DESC;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Maintenance_Open @EquipmentId INT,@UserId INT,@Description NVARCHAR(500) AS
BEGIN SET XACT_ABORT ON; BEGIN TRAN; IF EXISTS(SELECT 1 FROM dbo.Equipment WHERE Id=@EquipmentId AND Status='Assigned') THROW 51002,'Zimmetli ekipman bakıma alınamaz.',1; INSERT dbo.MaintenanceRecords(EquipmentId,OpenedByUserId,Description) VALUES(@EquipmentId,@UserId,@Description); UPDATE dbo.Equipment SET Status='InMaintenance' WHERE Id=@EquipmentId; COMMIT; SELECT CAST(SCOPE_IDENTITY() AS INT); END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Maintenance_Complete @Id INT,@Result NVARCHAR(500) AS BEGIN SET XACT_ABORT ON; BEGIN TRAN; DECLARE @EquipmentId INT; SELECT @EquipmentId=EquipmentId FROM dbo.MaintenanceRecords WHERE Id=@Id AND CompletedAt IS NULL; IF @EquipmentId IS NULL THROW 51003,'Açık bakım kaydı bulunamadı.',1; UPDATE dbo.MaintenanceRecords SET CompletedAt=SYSUTCDATETIME(),Result=@Result WHERE Id=@Id; UPDATE dbo.Equipment SET Status='Available' WHERE Id=@EquipmentId; COMMIT; SELECT 1; END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Maintenance_GetAll AS SELECT m.Id,e.SerialNumber,e.Name Equipment,m.Description,m.OpenedAt,m.CompletedAt,m.Result FROM dbo.MaintenanceRecords m JOIN dbo.Equipment e ON e.Id=m.EquipmentId ORDER BY m.Id DESC;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Mission_Create @Name NVARCHAR(100),@Description NVARCHAR(500),@PlannedAt DATETIME2,@UserId INT AS BEGIN INSERT dbo.Missions(Name,Description,PlannedAt,CreatedByUserId) VALUES(@Name,@Description,@PlannedAt,@UserId); SELECT CAST(SCOPE_IDENTITY() AS INT); END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Mission_AssignEquipment @MissionId INT,@EquipmentId INT AS BEGIN IF EXISTS(SELECT 1 FROM dbo.Equipment WHERE Id=@EquipmentId AND Status IN('InMaintenance','Lost')) THROW 51004,'Ekipman göreve uygun değildir.',1; INSERT dbo.MissionEquipment(MissionId,EquipmentId) VALUES(@MissionId,@EquipmentId); SELECT 1; END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Mission_GetAll AS SELECT m.Id,m.Name,m.Description,m.PlannedAt,m.Status,COUNT(me.EquipmentId) EquipmentCount FROM dbo.Missions m LEFT JOIN dbo.MissionEquipment me ON me.MissionId=m.Id GROUP BY m.Id,m.Name,m.Description,m.PlannedAt,m.Status ORDER BY m.PlannedAt;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Report_Dashboard AS SELECT (SELECT COUNT(*) FROM dbo.Equipment) EquipmentCount,(SELECT COUNT(*) FROM dbo.Equipment WHERE Status='Available') AvailableCount,(SELECT COUNT(*) FROM dbo.Checkouts WHERE ReturnedAt IS NULL) ActiveCheckoutCount,(SELECT COUNT(*) FROM dbo.MaintenanceRecords WHERE CompletedAt IS NULL) OpenMaintenanceCount,(SELECT COUNT(*) FROM dbo.Missions WHERE Status='Planned') PlannedMissionCount;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Report_Overdue AS SELECT c.Id,e.SerialNumber,e.Name Equipment,p.FullName Personnel,c.DueAt,DATEDIFF(DAY,c.DueAt,SYSUTCDATETIME()) OverdueDays FROM dbo.Checkouts c JOIN dbo.Equipment e ON e.Id=c.EquipmentId JOIN dbo.Personnel p ON p.Id=c.PersonnelId WHERE c.ReturnedAt IS NULL AND c.DueAt<SYSUTCDATETIME();
GO
CREATE OR ALTER PROCEDURE dbo.usp_Audit_Create @UserId INT=NULL,@Action NVARCHAR(50),@EntityName NVARCHAR(50),@EntityId INT=NULL,@Detail NVARCHAR(1000)=NULL AS BEGIN INSERT dbo.AuditLogs(UserId,Action,EntityName,EntityId,Detail) VALUES(@UserId,@Action,@EntityName,@EntityId,@Detail); SELECT CAST(SCOPE_IDENTITY() AS INT); END;
GO
CREATE OR ALTER PROCEDURE dbo.usp_Audit_GetAll AS SELECT TOP 500 a.Id,u.Username,a.Action,a.EntityName,a.EntityId,a.Detail,a.CreatedAt FROM dbo.AuditLogs a LEFT JOIN dbo.Users u ON u.Id=a.UserId ORDER BY a.Id DESC;
GO
