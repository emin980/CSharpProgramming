using System.Data;
using HangarDesk.Final.Domain;
using HangarDesk.Final.UI.WinFormsUI;
namespace HangarDesk.Final.Application;
internal static class SmokeTest
{
    public static void Run(HangarDeskService service)
    {
        if (!service.HasUsers()) service.CreateInitialAdmin("smokeadmin", "Smoke Admin", "SmokeTest!123");
        AuthenticatedUser user = service.Login("smokeadmin", "SmokeTest!123");
        if (user == null) throw new InvalidOperationException("Smoke test oturumu açılamadı.");
        string suffix = DateTime.UtcNow.Ticks.ToString()[^6..];
        byte[] image = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=");
        int personId = service.AddPerson(user, "Test Personeli " + suffix, "Test", null, image);
        int equipmentId = service.AddEquipment(user, "Test Ekipmanı " + suffix, "TST-" + suffix, "Tool", null, 1250.50m, image);
        service.UpdatePerson(user, personId, "Güncel Test Personeli " + suffix, "Operasyon", null, true);
        service.UpdateEquipment(user, equipmentId, "Güncel Test Ekipmanı " + suffix, "Tool", "Available", null);
        int checkoutId = service.Checkout(user, equipmentId, personId, DateTime.Now.AddDays(-1));
        if (service.Overdue(user).Rows.Count == 0) throw new InvalidOperationException("Gecikme raporu doğrulanamadı.");
        service.Return(user, checkoutId, "Doğrulama iadesi");
        int maintenanceId = service.OpenMaintenance(user, equipmentId, "Doğrulama bakımı");
        service.CompleteMaintenance(user, maintenanceId, "Başarıyla tamamlandı");
        int missionId = service.CreateMission(user, "Test Görevi " + suffix, "Uçtan uca doğrulama", DateTime.Now.AddDays(1));
        service.AssignMissionEquipment(user, missionId, equipmentId);
        service.AssignMissionPersonnel(user, missionId, personId);
        service.SetMissionStatus(user, missionId, "Active");
        string viewerName = "viewer" + suffix;
        service.CreateUser(user, viewerName, "Test Görüntüleyici", "ViewerTest!123", Roles.Viewer);
        AuthenticatedUser viewer = service.Login(viewerName, "ViewerTest!123");
        bool authorizationWorked = false;
        try { service.AddEquipment(viewer, "Yetkisiz", "DENIED-" + suffix, "Tool", null); }
        catch (AuthorizationException) { authorizationWorked = true; }
        service.SetUserRole(user, viewer.Id, Roles.Technician);
        viewer = service.Login(viewerName, "ViewerTest!123");
        if (viewer?.Role != Roles.Technician) throw new InvalidOperationException("Rol değişikliği doğrulanamadı.");
        service.SetUserActive(user, viewer.Id, false);
        if (service.Login(viewerName, "ViewerTest!123") != null) throw new InvalidOperationException("Kullanıcı etkinlik denetimi doğrulanamadı.");
        service.SetUserActive(user, viewer.Id, true);
        int typeId = service.AddEquipmentType(user, "TestType" + suffix, "Doğrulama materyal türü");
        int typedEquipmentId = service.AddEquipment(user, "Tür Test Ekipmanı " + suffix, "TYPE-" + suffix, "TestType" + suffix, null);
        service.UpdateEquipmentType(user, typeId, "UpdatedType" + suffix, "Güncellendi");
        if (service.Equipment(user, "", "UpdatedType" + suffix).Rows.Count != 1)
            throw new InvalidOperationException("Materyal türü güncellemesi doğrulanamadı.");
        service.DeleteEquipment(user, typedEquipmentId);
        service.DeleteEquipmentType(user, typeId);
        string memberName = "member" + suffix;
        service.Register(memberName, "Kendi Kaydını Oluşturan Kullanıcı", "MemberTest!123", "MemberTest!123");
        AuthenticatedUser member = service.Login(memberName, "MemberTest!123");
        if (member?.Role != Roles.Viewer) throw new InvalidOperationException("Üyelik rolü doğrulanamadı.");
        bool roleSecurityWorked = false;
        try { service.SetUserRole(member, member.Id, Roles.Administrator); }
        catch (AuthorizationException) { roleSecurityWorked = true; }
        service.ChangePassword(member, "MemberTest!123", "ChangedTest!123", "ChangedTest!123");
        if (!roleSecurityWorked || service.Login(memberName, "MemberTest!123") != null)
            throw new InvalidOperationException("Parola veya rol güvenliği doğrulanamadı.");
        member = service.Login(memberName, "ChangedTest!123");
        service.ResetPassword(user, member.Id, "ResetTest!123", "ResetTest!123");
        if (service.Login(memberName, "ResetTest!123") == null)
            throw new InvalidOperationException("Yönetici parola sıfırlaması doğrulanamadı.");
        int deletedEquipmentId = service.AddEquipment(user, "Silinecek Ekipman " + suffix, "DEL-" + suffix, "Tool", null);
        service.DeleteEquipment(user, deletedEquipmentId);
        int deletedPersonId = service.AddPerson(user, "Silinecek Personel " + suffix, "Test", null);
        service.DeletePerson(user, deletedPersonId);
        if (!authorizationWorked || service.Equipment(user).Rows.Count == 0 || service.Checkouts(user).Rows.Count == 0 ||
            service.Maintenance(user).Rows.Count == 0 || service.Missions(user).Rows.Count == 0 ||
            service.Dashboard(user).Rows.Count != 1 || service.AuditLog(user).Rows.Count == 0 ||
            service.EquipmentImage(user, equipmentId)?.Length != image.Length ||
            service.PersonnelImage(user, personId)?.Length != image.Length ||
            service.Equipment(user).AsEnumerable().Any(row => row.Field<int>("Id") == deletedEquipmentId) ||
            service.Personnel(user).AsEnumerable().Any(row => row.Field<int>("Id") == deletedPersonId))
            throw new InvalidOperationException("Smoke test verisi doğrulanamadı.");
        using (LoginForm loginForm = new(service)) { }
        using (MainForm mainForm = new(service, user))
        {
            if (!mainForm.HasSeparatedNavigationLayout)
                throw new InvalidOperationException("Windows Forms sekme yerleşimi doğrulanamadı.");
        }
        service.Logout(viewer);
        service.Logout(user);
        Console.WriteLine("Hafta-Son smoke test başarıyla tamamlandı.");
    }
}
