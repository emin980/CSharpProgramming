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
        int personId = service.AddPerson(user, "Test Personeli " + suffix, "Test", null);
        int equipmentId = service.AddEquipment(user, "Test Ekipmanı " + suffix, "TST-" + suffix, "Tool", null);
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
        if (!authorizationWorked || service.Equipment(user).Rows.Count == 0 || service.Checkouts(user).Rows.Count == 0 ||
            service.Maintenance(user).Rows.Count == 0 || service.Missions(user).Rows.Count == 0 ||
            service.Dashboard(user).Rows.Count != 1 || service.AuditLog(user).Rows.Count == 0)
            throw new InvalidOperationException("Smoke test verisi doğrulanamadı.");
        using (LoginForm loginForm = new(service)) { }
        using (MainForm mainForm = new(service, user)) { }
        service.Logout(viewer);
        service.Logout(user);
        Console.WriteLine("Hafta-Son smoke test başarıyla tamamlandı.");
    }
}
