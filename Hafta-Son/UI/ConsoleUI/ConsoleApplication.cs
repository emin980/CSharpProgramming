using System.Data;
using HangarDesk.Final.Application;
using HangarDesk.Final.Domain;
namespace HangarDesk.Final.UI.ConsoleUI;
internal sealed class ConsoleApplication
{
    private readonly HangarDeskService service;
    private AuthenticatedUser session;
    public ConsoleApplication(HangarDeskService service) { this.service = service; }
    public void Run()
    {
        if (!this.service.HasUsers()) this.CreateFirstAdmin();
        this.Login();
        if (this.session == null) return;
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n1 Ekipman  2 Personel  3 Zimmet  4 İade  5 Bakım  6 Görev  7 Rapor" +
                (this.session.Role == Roles.Administrator ? "  8 Kullanıcı  9 Denetim" : "") + "  0 Çıkış");
            try
            {
                string choice = Console.ReadLine();
                if (this.session.Role == Roles.Viewer && choice is "3" or "4" or "5" or "6") throw new AuthorizationException();
                if (this.session.Role != Roles.Administrator && choice is "8" or "9") throw new AuthorizationException();
                switch (choice)
                {
                    case "1": this.EquipmentMenu(); break;
                    case "2": this.PersonnelMenu(); break;
                    case "3": this.Checkout(); break;
                    case "4": this.Return(); break;
                    case "5": this.Maintenance(); break;
                    case "6": this.Mission(); break;
                    case "7": Print(this.service.Dashboard(this.session)); Print(this.service.Overdue(this.session)); break;
                    case "8": this.UserMenu(); break;
                    case "9": Print(this.service.AuditLog(this.session)); break;
                    case "0": running = false; break;
                }
            }
            catch (Exception exception) { Console.WriteLine("İşlem gerçekleştirilemedi: " + exception.Message); }
        }
        this.service.Logout(this.session);
    }
    private void CreateFirstAdmin()
    {
        Console.WriteLine("İlk yönetici hesabı oluşturulacaktır.");
        Console.Write("Kullanıcı adı: "); string username = Console.ReadLine();
        Console.Write("Ad soyad: "); string display = Console.ReadLine();
        Console.Write("Parola (en az 8 karakter): "); string password = ReadPassword();
        this.service.CreateInitialAdmin(username, display, password);
    }
    private void Login()
    {
        Console.Write("Kullanıcı adı: "); string username = Console.ReadLine();
        Console.Write("Parola: "); string password = ReadPassword();
        this.session = this.service.Login(username, password);
        Console.WriteLine(this.session == null ? "Giriş başarısız." : "Hoş geldiniz, " + this.session.DisplayName + " [" + this.session.Role + "]");
    }
    private void EquipmentMenu()
    {
        Print(this.service.Equipment(this.session));
        Console.Write("1 Ekle 2 Ara 3 Güncelle 4 Sil 0 Geri: "); string choice = Console.ReadLine();
        if (choice == "2") { Console.Write("Arama: "); Print(this.service.Equipment(this.session, Console.ReadLine())); return; }
        if (this.session.Role == Roles.Viewer || choice == "0") return;
        if (choice == "1")
        {
            Console.Write("Ad: "); string name = Console.ReadLine(); Console.Write("Seri: "); string serial = Console.ReadLine();
            Console.Write("Tür: "); string type = Console.ReadLine(); Console.Write("Not: "); string notes = Console.ReadLine();
            this.service.AddEquipment(this.session, name, serial, type, notes);
        }
        else if (choice == "3")
        {
            Console.Write("Id: "); int id = Convert.ToInt32(Console.ReadLine()); Console.Write("Ad: "); string name = Console.ReadLine();
            Console.Write("Tür: "); string type = Console.ReadLine(); Console.Write("Durum: "); string status = Console.ReadLine();
            Console.Write("Not: "); this.service.UpdateEquipment(this.session, id, name, type, status, Console.ReadLine());
        }
        else if (choice == "4" && this.session.Role == Roles.Administrator)
        { Console.Write("Id: "); this.service.DeleteEquipment(this.session, Convert.ToInt32(Console.ReadLine())); }
    }
    private void PersonnelMenu()
    {
        Print(this.service.Personnel(this.session)); Console.Write("1 Ekle 2 Güncelle 3 Sil 0 Geri: "); string choice = Console.ReadLine();
        if (this.session.Role == Roles.Viewer || choice == "0") return;
        if (choice == "1")
        { Console.Write("Ad soyad: "); string name = Console.ReadLine(); Console.Write("Bölüm: "); string department = Console.ReadLine(); Console.Write("Telefon: "); this.service.AddPerson(this.session, name, department, Console.ReadLine()); }
        else if (choice == "2")
        {
            Console.Write("Id: "); int id = Convert.ToInt32(Console.ReadLine()); Console.Write("Ad soyad: "); string name = Console.ReadLine();
            Console.Write("Bölüm: "); string department = Console.ReadLine(); Console.Write("Telefon: "); string phone = Console.ReadLine();
            Console.Write("Etkin mi? (true/false): "); this.service.UpdatePerson(this.session, id, name, department, phone, bool.Parse(Console.ReadLine()));
        }
        else if (choice == "3" && this.session.Role == Roles.Administrator)
        { Console.Write("Id: "); this.service.DeletePerson(this.session, Convert.ToInt32(Console.ReadLine())); }
    }
    private void Checkout()
    {
        Print(this.service.Equipment(this.session)); Print(this.service.Personnel(this.session));
        Console.Write("Ekipman Id: "); int equipment = Convert.ToInt32(Console.ReadLine());
        Console.Write("Personel Id: "); int person = Convert.ToInt32(Console.ReadLine());
        Console.Write("Kaç gün: "); int days = Convert.ToInt32(Console.ReadLine());
        this.service.Checkout(this.session, equipment, person, DateTime.Now.AddDays(days));
    }
    private void Return()
    {
        Print(this.service.Checkouts(this.session)); Console.Write("Zimmet Id: "); int id = Convert.ToInt32(Console.ReadLine());
        this.service.Return(this.session, id, "Konsol üzerinden iade");
    }
    private void Maintenance()
    {
        Print(this.service.Maintenance(this.session)); Console.Write("1 Aç 2 Tamamla: "); string choice = Console.ReadLine();
        Console.Write("Id: "); int id = Convert.ToInt32(Console.ReadLine()); Console.Write("Açıklama/sonuç: "); string text = Console.ReadLine();
        if (choice == "1") this.service.OpenMaintenance(this.session, id, text); else this.service.CompleteMaintenance(this.session, id, text);
    }
    private void Mission()
    {
        Print(this.service.Missions(this.session)); Console.Write("1 Oluştur 2 Ekipman ata 3 Personel ata 4 Durum değiştir: "); string choice = Console.ReadLine();
        if (choice == "1") { Console.Write("Ad: "); string name = Console.ReadLine(); Console.Write("Açıklama: "); string description = Console.ReadLine(); this.service.CreateMission(this.session, name, description, DateTime.Now.AddDays(1)); }
        else if (choice == "2") { Console.Write("Görev Id: "); int mission = Convert.ToInt32(Console.ReadLine()); Console.Write("Ekipman Id: "); int equipment = Convert.ToInt32(Console.ReadLine()); this.service.AssignMissionEquipment(this.session, mission, equipment); }
        else if (choice == "3") { Console.Write("Görev Id: "); int mission = Convert.ToInt32(Console.ReadLine()); Console.Write("Personel Id: "); int person = Convert.ToInt32(Console.ReadLine()); this.service.AssignMissionPersonnel(this.session, mission, person); }
        else if (choice == "4") { Console.Write("Görev Id: "); int mission = Convert.ToInt32(Console.ReadLine()); Console.Write("Durum: "); this.service.SetMissionStatus(this.session, mission, Console.ReadLine()); }
    }
    private void UserMenu()
    {
        Print(this.service.Users(this.session)); Console.Write("1 Ekle 2 Etkinlik değiştir 3 Rol değiştir 0 Geri: "); string choice = Console.ReadLine();
        if (choice == "1")
        {
            Console.Write("Kullanıcı adı: "); string username = Console.ReadLine(); Console.Write("Ad soyad: "); string display = Console.ReadLine();
            Console.Write("Rol (Administrator/Technician/Viewer): "); string role = Console.ReadLine(); Console.Write("Parola: "); string password = ReadPassword();
            this.service.CreateUser(this.session, username, display, password, role);
        }
        else if (choice == "2") { Console.Write("Kullanıcı Id: "); int id = Convert.ToInt32(Console.ReadLine()); Console.Write("Etkin mi? (true/false): "); this.service.SetUserActive(this.session, id, bool.Parse(Console.ReadLine())); }
        else if (choice == "3") { Console.Write("Kullanıcı Id: "); int id = Convert.ToInt32(Console.ReadLine()); Console.Write("Rol: "); this.service.SetUserRole(this.session, id, Console.ReadLine()); }
    }
    private static void Print(DataTable table)
    {
        Console.WriteLine("\n" + string.Join(" | ", table.Columns.Cast<DataColumn>().Select(column => column.ColumnName)));
        foreach (DataRow row in table.Rows) Console.WriteLine(string.Join(" | ", row.ItemArray.Select(value => value == DBNull.Value ? "-" : value)));
    }
    private static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace && password.Length > 0) password = password[..^1];
            else if (!char.IsControl(key.KeyChar)) password += key.KeyChar;
        }
        Console.WriteLine();
        return password;
    }
}
