using System.Data;
using HangarDesk.Final.Domain;
using HangarDesk.Final.Infrastructure;
using Microsoft.Data.SqlClient;
namespace HangarDesk.Final.Application;
internal sealed class HangarDeskService
{
    private readonly HangarDeskRepository repository;
    private readonly PasswordHasher hasher;
    public HangarDeskService(HangarDeskRepository repository, PasswordHasher hasher) { this.repository = repository; this.hasher = hasher; }
    public bool HasUsers() => this.repository.Scalar("dbo.usp_User_Count") > 0;
    public void CreateInitialAdmin(string username, string displayName, string password)
    {
        if (this.HasUsers()) throw new InvalidOperationException("İlk yönetici hesabı daha önce oluşturulmuştur.");
        this.CreateUserCore(username, displayName, password, Roles.Administrator);
    }
    public void Register(string username, string displayName, string password, string passwordConfirmation)
    {
        if (password != passwordConfirmation) throw new ArgumentException("Parola ve parola doğrulaması eşleşmiyor.");
        this.CreateUserCore(username, displayName, password, Roles.Viewer);
    }
    public AuthenticatedUser Login(string username, string password)
    {
        UserCredential user = this.repository.GetCredential(username);
        if (user == null || !user.IsActive || !this.hasher.Verify(password, user.PasswordHash, user.PasswordSalt)) return null;
        this.repository.Audit(user.Id, "Login", "User", user.Id, "Oturum açıldı.");
        return new AuthenticatedUser { Id = user.Id, Username = user.Username, DisplayName = user.DisplayName, Role = user.Role };
    }
    public void CreateUser(AuthenticatedUser actor, string username, string displayName, string password, string role)
    { Demand(actor, Roles.Administrator); this.CreateUserCore(username, displayName, password, role); this.Audit(actor, "Create", "User", null, username); }
    private int CreateUserCore(string username, string displayName, string password, string role)
    {
        ValidatePassword(password);
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Kullanıcı adı zorunludur.");
        EnsureRole(role);
        (byte[] hash, byte[] salt) = this.hasher.Hash(password);
        int id;
        try
        {
            id = this.repository.Scalar("dbo.usp_User_Create", P("@Username", username.Trim()), P("@DisplayName", Required(displayName, "Ad soyad")), P("@PasswordHash", hash), P("@PasswordSalt", salt), P("@RoleName", role));
        }
        catch (SqlException exception) when (exception.Number is 2601 or 2627)
        {
            throw new ArgumentException("Bu kullanıcı adı daha önce alınmıştır.");
        }
        this.repository.Audit(id, "Create", "User", id, "Kullanıcı hesabı oluşturuldu.");
        return id;
    }
    public DataTable Users(AuthenticatedUser actor) { Demand(actor, Roles.Administrator); return this.repository.Table("dbo.usp_User_GetAll"); }
    public void SetUserActive(AuthenticatedUser actor, int id, bool active) { Demand(actor, Roles.Administrator); if (actor.Id == id && !active) throw new InvalidOperationException("Açık oturumun kullanıcı hesabı pasif duruma getirilemez."); this.repository.Scalar("dbo.usp_User_SetActive", P("@Id", id), P("@IsActive", active)); this.Audit(actor, "Update", "User", id, "Etkinlik değiştirildi."); }
    public void SetUserRole(AuthenticatedUser actor, int id, string role) { Demand(actor, Roles.Administrator); EnsureRole(role); if (actor.Id == id && role != Roles.Administrator) throw new InvalidOperationException("Açık oturumun yönetici rolü değiştirilemez."); this.repository.Scalar("dbo.usp_User_SetRole", P("@Id", id), P("@RoleName", role)); this.Audit(actor, "UpdateRole", "User", id, role); }
    public void ChangePassword(AuthenticatedUser actor, string currentPassword, string newPassword, string confirmation)
    {
        Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer);
        UserCredential user = this.repository.GetCredential(actor.Username);
        if (user == null || !this.hasher.Verify(currentPassword, user.PasswordHash, user.PasswordSalt))
            throw new ArgumentException("Mevcut parola doğru değildir.");
        if (newPassword != confirmation) throw new ArgumentException("Yeni parola ve parola doğrulaması eşleşmiyor.");
        this.UpdatePassword(actor.Id, newPassword);
        this.Audit(actor, "ChangePassword", "User", actor.Id, "Kullanıcı parolasını değiştirdi.");
    }
    public void ResetPassword(AuthenticatedUser actor, int userId, string newPassword, string confirmation)
    {
        Demand(actor, Roles.Administrator);
        if (newPassword != confirmation) throw new ArgumentException("Yeni parola ve parola doğrulaması eşleşmiyor.");
        this.UpdatePassword(userId, newPassword);
        this.Audit(actor, "ResetPassword", "User", userId, "Yönetici geçici parola belirledi.");
    }
    private void UpdatePassword(int userId, string password)
    {
        ValidatePassword(password);
        (byte[] hash, byte[] salt) = this.hasher.Hash(password);
        if (this.repository.Scalar("dbo.usp_User_UpdatePassword", P("@Id", userId), P("@PasswordHash", hash), P("@PasswordSalt", salt)) == 0)
            throw new InvalidOperationException("Etkin kullanıcı hesabı bulunamadı.");
    }
    public void Logout(AuthenticatedUser actor) { if (actor != null) this.Audit(actor, "Logout", "User", actor.Id, "Oturum kapatıldı."); }
    public DataTable Equipment(AuthenticatedUser actor, string search = "", string type = "", string status = "")
    { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Table("dbo.usp_Equipment_Search", P("@Search", search), P("@EquipmentType", type), P("@Status", status)); }
    public DataTable EquipmentTypes(AuthenticatedUser actor)
    { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Table("dbo.usp_EquipmentType_GetAll"); }
    public int AddEquipmentType(AuthenticatedUser actor, string name, string description)
    { Demand(actor, Roles.Administrator, Roles.Technician); int id = this.repository.Scalar("dbo.usp_EquipmentType_Create", P("@Name", Required(name, "Materyal türü adı")), P("@Description", description)); this.Audit(actor, "Create", "EquipmentType", id, name); return id; }
    public void UpdateEquipmentType(AuthenticatedUser actor, int id, string name, string description)
    { Demand(actor, Roles.Administrator, Roles.Technician); this.repository.Scalar("dbo.usp_EquipmentType_Update", P("@Id", id), P("@Name", Required(name, "Materyal türü adı")), P("@Description", description)); this.Audit(actor, "Update", "EquipmentType", id, name); }
    public void DeleteEquipmentType(AuthenticatedUser actor, int id)
    { Demand(actor, Roles.Administrator); this.repository.Scalar("dbo.usp_EquipmentType_Delete", P("@Id", id)); this.Audit(actor, "Delete", "EquipmentType", id, null); }
    public int AddEquipment(AuthenticatedUser actor, string name, string serial, string type, string notes, decimal? purchasePrice = null, byte[] imageData = null)
    { Demand(actor, Roles.Administrator, Roles.Technician); int id = this.repository.Scalar("dbo.usp_Equipment_Create", P("@Name", Required(name, "Ekipman adı")), P("@SerialNumber", Required(serial, "Seri numarası")), P("@EquipmentType", Required(type, "Ekipman türü")), P("@Notes", notes), P("@PurchasePrice", purchasePrice), P("@ImageData", imageData)); this.Audit(actor, "Create", "Equipment", id, serial); return id; }
    public void UpdateEquipment(AuthenticatedUser actor, int id, string name, string type, string status, string notes, decimal? purchasePrice = null, byte[] imageData = null, string serial = null)
    { Demand(actor, Roles.Administrator, Roles.Technician); EnsureEquipmentStatus(status); this.repository.Scalar("dbo.usp_Equipment_Update", P("@Id", id), P("@Name", Required(name, "Ekipman adı")), P("@EquipmentType", Required(type, "Ekipman türü")), P("@Status", status), P("@Notes", notes), P("@PurchasePrice", purchasePrice), P("@ImageData", imageData), P("@SerialNumber", serial == null ? null : Required(serial, "Seri numarası"))); this.Audit(actor, "Update", "Equipment", id, status); }
    public void DeleteEquipment(AuthenticatedUser actor, int id) { Demand(actor, Roles.Administrator); this.repository.Scalar("dbo.usp_Equipment_Delete", P("@Id", id)); this.Audit(actor, "Delete", "Equipment", id, null); }
    public byte[] EquipmentImage(AuthenticatedUser actor, int id) { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Bytes("dbo.usp_Equipment_GetImage", P("@Id", id)); }
    public DataTable Personnel(AuthenticatedUser actor) { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Table("dbo.usp_Personnel_GetAll"); }
    public int AddPerson(AuthenticatedUser actor, string name, string department, string phone, byte[] imageData = null)
    { Demand(actor, Roles.Administrator, Roles.Technician); int id = this.repository.Scalar("dbo.usp_Personnel_Create", P("@FullName", Required(name, "Ad soyad")), P("@Department", department), P("@Phone", phone), P("@ImageData", imageData)); this.Audit(actor, "Create", "Personnel", id, name); return id; }
    public void UpdatePerson(AuthenticatedUser actor, int id, string name, string department, string phone, bool active, byte[] imageData = null)
    { Demand(actor, Roles.Administrator, Roles.Technician); this.repository.Scalar("dbo.usp_Personnel_Update", P("@Id", id), P("@FullName", Required(name, "Ad soyad")), P("@Department", department), P("@Phone", phone), P("@IsActive", active), P("@ImageData", imageData)); this.Audit(actor, "Update", "Personnel", id, name); }
    public void DeletePerson(AuthenticatedUser actor, int id)
    { Demand(actor, Roles.Administrator); this.repository.Scalar("dbo.usp_Personnel_Delete", P("@Id", id)); this.Audit(actor, "Delete", "Personnel", id, null); }
    public byte[] PersonnelImage(AuthenticatedUser actor, int id) { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Bytes("dbo.usp_Personnel_GetImage", P("@Id", id)); }
    public DataTable Checkouts(AuthenticatedUser actor) { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Table("dbo.usp_Checkout_GetAll"); }
    public int Checkout(AuthenticatedUser actor, int equipmentId, int personnelId, DateTime dueAt)
    { Demand(actor, Roles.Administrator, Roles.Technician); int id = this.repository.Scalar("dbo.usp_Checkout_Create", P("@EquipmentId", equipmentId), P("@PersonnelId", personnelId), P("@UserId", actor.Id), P("@DueAt", dueAt)); this.Audit(actor, "Checkout", "Checkout", id, null); return id; }
    public void Return(AuthenticatedUser actor, int checkoutId, string notes)
    { Demand(actor, Roles.Administrator, Roles.Technician); this.repository.Scalar("dbo.usp_Checkout_Return", P("@CheckoutId", checkoutId), P("@ReturnNotes", notes)); this.Audit(actor, "Return", "Checkout", checkoutId, notes); }
    public DataTable Maintenance(AuthenticatedUser actor) { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Table("dbo.usp_Maintenance_GetAll"); }
    public int OpenMaintenance(AuthenticatedUser actor, int equipmentId, string description)
    { Demand(actor, Roles.Administrator, Roles.Technician); int id = this.repository.Scalar("dbo.usp_Maintenance_Open", P("@EquipmentId", equipmentId), P("@UserId", actor.Id), P("@Description", description)); this.Audit(actor, "Open", "Maintenance", id, description); return id; }
    public void CompleteMaintenance(AuthenticatedUser actor, int id, string result)
    { Demand(actor, Roles.Administrator, Roles.Technician); this.repository.Scalar("dbo.usp_Maintenance_Complete", P("@Id", id), P("@Result", result)); this.Audit(actor, "Complete", "Maintenance", id, result); }
    public DataTable Missions(AuthenticatedUser actor) { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Table("dbo.usp_Mission_GetAll"); }
    public int CreateMission(AuthenticatedUser actor, string name, string description, DateTime plannedAt)
    { Demand(actor, Roles.Administrator, Roles.Technician); int id = this.repository.Scalar("dbo.usp_Mission_Create", P("@Name", name), P("@Description", description), P("@PlannedAt", plannedAt), P("@UserId", actor.Id)); this.Audit(actor, "Create", "Mission", id, name); return id; }
    public void AssignMissionEquipment(AuthenticatedUser actor, int missionId, int equipmentId)
    { Demand(actor, Roles.Administrator, Roles.Technician); this.repository.Scalar("dbo.usp_Mission_AssignEquipment", P("@MissionId", missionId), P("@EquipmentId", equipmentId)); this.Audit(actor, "Assign", "Mission", missionId, "Equipment " + equipmentId); }
    public void AssignMissionPersonnel(AuthenticatedUser actor, int missionId, int personnelId)
    { Demand(actor, Roles.Administrator, Roles.Technician); this.repository.Scalar("dbo.usp_Mission_AssignPersonnel", P("@MissionId", missionId), P("@PersonnelId", personnelId)); this.Audit(actor, "Assign", "Mission", missionId, "Personnel " + personnelId); }
    public void SetMissionStatus(AuthenticatedUser actor, int missionId, string status)
    { Demand(actor, Roles.Administrator, Roles.Technician); this.repository.Scalar("dbo.usp_Mission_SetStatus", P("@Id", missionId), P("@Status", status)); this.Audit(actor, "UpdateStatus", "Mission", missionId, status); }
    public DataTable Dashboard(AuthenticatedUser actor) { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Table("dbo.usp_Report_Dashboard"); }
    public DataTable Overdue(AuthenticatedUser actor) { Demand(actor, Roles.Administrator, Roles.Technician, Roles.Viewer); return this.repository.Table("dbo.usp_Report_Overdue"); }
    public DataTable AuditLog(AuthenticatedUser actor) { Demand(actor, Roles.Administrator); return this.repository.Table("dbo.usp_Audit_GetAll"); }
    private void Audit(AuthenticatedUser actor, string action, string entity, int? id, string detail) => this.repository.Audit(actor?.Id, action, entity, id, detail);
    private static SqlParameter P(string name, object value) => HangarDeskRepository.P(name, value);
    private static string Required(string value, string field)
    { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(field + " zorunludur."); return value.Trim(); }
    private static void EnsureRole(string role)
    { if (role != Roles.Administrator && role != Roles.Technician && role != Roles.Viewer) throw new ArgumentException("Geçersiz kullanıcı rolü."); }
    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            throw new ArgumentException("Parola en az sekiz karakter olmalıdır.");
        if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
            throw new ArgumentException("Parola en az bir büyük harf, bir küçük harf ve bir rakam içermelidir.");
    }
    private static void EnsureEquipmentStatus(string status)
    { if (status is not ("Available" or "Assigned" or "InMaintenance" or "Lost")) throw new ArgumentException("Geçersiz ekipman durumu."); }
    private static void Demand(AuthenticatedUser actor, params string[] roles)
    { if (actor == null || !roles.Contains(actor.Role)) throw new AuthorizationException(); }
}
