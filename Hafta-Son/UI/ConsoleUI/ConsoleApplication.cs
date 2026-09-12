using System.Data;
using System.Globalization;
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
        Console.Write("1 Oturum aç  2 Üye ol: ");
        if (Console.ReadLine() == "2")
        {
            try { this.Register(); }
            catch (Exception exception) { Console.WriteLine("Üyelik oluşturulamadı: " + exception.Message); }
        }
        this.Login();
        if (this.session == null) return;
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n1 Ekipman  2 Personel  3 Zimmet  4 İade  5 Bakım  6 Görev  7 Rapor" +
                (this.session.Role == Roles.Administrator ? "  8 Kullanıcı  9 Denetim" : "") +
                "  10 Parolamı Değiştir  11 Materyal Türleri  0 Çıkış");
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
                    case "10": this.ChangePassword(); break;
                    case "11": this.EquipmentTypeMenu(); break;
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
        Console.Write("Parola doğrulama: "); string confirmation = ReadPassword();
        if (password != confirmation) throw new ArgumentException("Parola ve parola doğrulaması eşleşmiyor.");
        this.service.CreateInitialAdmin(username, display, password);
    }
    private void Login()
    {
        Console.Write("Kullanıcı adı: "); string username = Console.ReadLine();
        Console.Write("Parola: "); string password = ReadPassword();
        this.session = this.service.Login(username, password);
        Console.WriteLine(this.session == null ? "Giriş başarısız." : "Hoş geldiniz, " + this.session.DisplayName + " [" + this.session.Role + "]");
    }
    private void Register()
    {
        Console.Write("Kullanıcı adı: "); string username = Console.ReadLine();
        Console.Write("Ad soyad: "); string displayName = Console.ReadLine();
        Console.Write("Parola: "); string password = ReadPassword();
        Console.Write("Parola doğrulama: "); string confirmation = ReadPassword();
        this.service.Register(username, displayName, password, confirmation);
        Console.WriteLine("Üyeliğiniz Görüntüleyici rolüyle oluşturuldu.");
    }
    private void ChangePassword()
    {
        Console.Write("Mevcut parola: "); string current = ReadPassword();
        Console.Write("Yeni parola: "); string password = ReadPassword();
        Console.Write("Yeni parola doğrulama: "); string confirmation = ReadPassword();
        this.service.ChangePassword(this.session, current, password, confirmation);
        Console.WriteLine("Parolanız değiştirildi.");
    }
    private void EquipmentMenu()
    {
        Print(this.service.Equipment(this.session));
        Console.Write("1 Ekle 2 Ara/Filtrele 3 Güncelle 4 Sil 5 Görseli Dosyaya Aktar 0 Geri: "); string choice = Console.ReadLine();
        if (choice == "2")
        {
            Console.Write("Ad veya seri numarası: "); string search = Console.ReadLine();
            Console.Write("Materyal türü (tümü için boş): "); string type = Console.ReadLine();
            Console.Write("Durum (tümü için boş): "); string status = Console.ReadLine();
            Print(this.service.Equipment(this.session, search, type, status));
            return;
        }
        if (choice == "5")
        {
            int id = ReadInt("Ekipman Id");
            WriteImageFile(this.service.EquipmentImage(this.session, id), "ekipman-" + id + ".png");
            return;
        }
        if (this.session.Role == Roles.Viewer || choice == "0") return;
        if (choice == "1")
        {
            Console.Write("Ad: "); string name = Console.ReadLine(); Console.Write("Seri: "); string serial = Console.ReadLine();
            Print(this.service.EquipmentTypes(this.session)); Console.Write("Materyal türü adı: "); string type = Console.ReadLine();
            Console.Write("Not: "); string notes = Console.ReadLine();
            decimal price = ReadDecimal("Alış bedeli");
            byte[] image = ReadImageFile();
            this.service.AddEquipment(this.session, name, serial, type, notes, price, image);
        }
        else if (choice == "3")
        {
            int id = ReadInt("Ekipman Id"); Console.Write("Ad: "); string name = Console.ReadLine();
            Console.Write("Seri numarası: "); string serial = Console.ReadLine();
            Print(this.service.EquipmentTypes(this.session)); Console.Write("Materyal türü adı: "); string type = Console.ReadLine();
            Console.Write("Durum (Available/Assigned/InMaintenance/Lost): "); string status = Console.ReadLine();
            Console.Write("Not: "); string notes = Console.ReadLine();
            decimal price = ReadDecimal("Alış bedeli");
            byte[] image = ReadImageFile();
            this.service.UpdateEquipment(this.session, id, name, type, status, notes, price, image, serial);
        }
        else if (choice == "4" && this.session.Role == Roles.Administrator)
        { this.service.DeleteEquipment(this.session, ReadInt("Ekipman Id")); }
    }
    private void PersonnelMenu()
    {
        Print(this.service.Personnel(this.session)); Console.Write("1 Ekle 2 Güncelle 3 Sil 4 Görseli Dosyaya Aktar 0 Geri: "); string choice = Console.ReadLine();
        if (choice == "4")
        {
            int id = ReadInt("Personel Id");
            WriteImageFile(this.service.PersonnelImage(this.session, id), "personel-" + id + ".png");
            return;
        }
        if (this.session.Role == Roles.Viewer || choice == "0") return;
        if (choice == "1")
        {
            Console.Write("Ad soyad: "); string name = Console.ReadLine(); Console.Write("Bölüm: "); string department = Console.ReadLine();
            Console.Write("Telefon: "); string phone = Console.ReadLine();
            this.service.AddPerson(this.session, name, department, phone, ReadImageFile());
        }
        else if (choice == "2")
        {
            int id = ReadInt("Personel Id"); Console.Write("Ad soyad: "); string name = Console.ReadLine();
            Console.Write("Bölüm: "); string department = Console.ReadLine(); Console.Write("Telefon: "); string phone = Console.ReadLine();
            bool active = ReadBool("Etkin mi");
            this.service.UpdatePerson(this.session, id, name, department, phone, active, ReadImageFile());
        }
        else if (choice == "3" && this.session.Role == Roles.Administrator)
        { this.service.DeletePerson(this.session, ReadInt("Personel Id")); }
    }
    private void EquipmentTypeMenu()
    {
        Print(this.service.EquipmentTypes(this.session));
        if (this.session.Role == Roles.Viewer) return;
        Console.Write("1 Tür ekle 2 Güncelle 3 Sil 0 Geri: "); string choice = Console.ReadLine();
        if (choice == "1")
        {
            Console.Write("Tür adı: "); string name = Console.ReadLine();
            Console.Write("Açıklama: "); this.service.AddEquipmentType(this.session, name, Console.ReadLine());
        }
        else if (choice == "2")
        {
            int id = ReadInt("Tür Id"); Console.Write("Tür adı: "); string name = Console.ReadLine();
            Console.Write("Açıklama: "); this.service.UpdateEquipmentType(this.session, id, name, Console.ReadLine());
        }
        else if (choice == "3" && this.session.Role == Roles.Administrator)
            this.service.DeleteEquipmentType(this.session, ReadInt("Tür Id"));
    }
    private void Checkout()
    {
        Print(this.service.Equipment(this.session)); Print(this.service.Personnel(this.session));
        int equipment = ReadInt("Ekipman Id");
        int person = ReadInt("Personel Id");
        DateTime dueAt = ReadDateTime("Son teslim tarihi");
        this.service.Checkout(this.session, equipment, person, dueAt);
    }
    private void Return()
    {
        Print(this.service.Checkouts(this.session)); int id = ReadInt("Zimmet Id");
        Console.Write("İade açıklaması: "); this.service.Return(this.session, id, Console.ReadLine());
    }
    private void Maintenance()
    {
        Print(this.service.Maintenance(this.session)); Console.Write("1 Aç 2 Tamamla: "); string choice = Console.ReadLine();
        int id = ReadInt(choice == "1" ? "Ekipman Id" : "Bakım Id"); Console.Write("Açıklama/sonuç: "); string text = Console.ReadLine();
        if (choice == "1") this.service.OpenMaintenance(this.session, id, text); else this.service.CompleteMaintenance(this.session, id, text);
    }
    private void Mission()
    {
        Print(this.service.Missions(this.session)); Console.Write("1 Oluştur 2 Ekipman ata 3 Personel ata 4 Durum değiştir: "); string choice = Console.ReadLine();
        if (choice == "1") { Console.Write("Ad: "); string name = Console.ReadLine(); Console.Write("Açıklama: "); string description = Console.ReadLine(); this.service.CreateMission(this.session, name, description, ReadDateTime("Planlanan tarih")); }
        else if (choice == "2") { int mission = ReadInt("Görev Id"); int equipment = ReadInt("Ekipman Id"); this.service.AssignMissionEquipment(this.session, mission, equipment); }
        else if (choice == "3") { int mission = ReadInt("Görev Id"); int person = ReadInt("Personel Id"); this.service.AssignMissionPersonnel(this.session, mission, person); }
        else if (choice == "4") { int mission = ReadInt("Görev Id"); Console.Write("Durum: "); this.service.SetMissionStatus(this.session, mission, Console.ReadLine()); }
    }
    private void UserMenu()
    {
        Print(this.service.Users(this.session)); Console.Write("1 Ekle 2 Etkinlik değiştir 3 Rol değiştir 4 Geçici parola belirle 0 Geri: "); string choice = Console.ReadLine();
        if (choice == "1")
        {
            Console.Write("Kullanıcı adı: "); string username = Console.ReadLine(); Console.Write("Ad soyad: "); string display = Console.ReadLine();
            Console.Write("Rol (Administrator/Technician/Viewer): "); string role = Console.ReadLine(); Console.Write("Parola: "); string password = ReadPassword();
            Console.Write("Parola doğrulama: "); string confirmation = ReadPassword();
            if (password != confirmation) throw new ArgumentException("Parola ve parola doğrulaması eşleşmiyor.");
            this.service.CreateUser(this.session, username, display, password, role);
        }
        else if (choice == "2") { int id = ReadInt("Kullanıcı Id"); this.service.SetUserActive(this.session, id, ReadBool("Etkin mi")); }
        else if (choice == "3") { int id = ReadInt("Kullanıcı Id"); Console.Write("Rol: "); this.service.SetUserRole(this.session, id, Console.ReadLine()); }
        else if (choice == "4")
        {
            int id = ReadInt("Kullanıcı Id");
            Console.Write("Geçici parola: "); string password = ReadPassword();
            Console.Write("Parola doğrulama: "); string confirmation = ReadPassword();
            this.service.ResetPassword(this.session, id, password, confirmation);
        }
    }
    private static void Print(DataTable table)
    {
        Console.WriteLine("\n" + string.Join(" | ", table.Columns.Cast<DataColumn>().Select(column => column.ColumnName)));
        foreach (DataRow row in table.Rows) Console.WriteLine(string.Join(" | ", row.ItemArray.Select(FormatValue)));
    }
    private static string FormatValue(object value)
    {
        if (value == DBNull.Value) return "-";
        if (value is DateTime date) return date.ToString("dd.MM.yyyy HH:mm", CultureInfo.GetCultureInfo("tr-TR"));
        if (value is decimal amount) return amount.ToString("C2", CultureInfo.GetCultureInfo("tr-TR"));
        return Convert.ToString(value);
    }
    private static int ReadInt(string label)
    {
        while (true)
        {
            Console.Write(label + ": ");
            if (int.TryParse(Console.ReadLine(), out int value)) return value;
            Console.WriteLine("Geçerli bir tam sayı girin.");
        }
    }
    private static decimal ReadDecimal(string label)
    {
        while (true)
        {
            Console.Write(label + ": ");
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.GetCultureInfo("tr-TR"), out decimal value)) return value;
            Console.WriteLine("Geçerli bir parasal değer girin. Örnek: 1250,50");
        }
    }
    private static DateTime ReadDateTime(string label)
    {
        while (true)
        {
            Console.Write(label + " (gg.aa.yyyy ss:dd): ");
            if (DateTime.TryParse(Console.ReadLine(), CultureInfo.GetCultureInfo("tr-TR"), DateTimeStyles.None, out DateTime value)) return value;
            Console.WriteLine("Geçerli bir tarih ve saat girin. Örnek: 15.09.2026 17:30");
        }
    }
    private static bool ReadBool(string label)
    {
        while (true)
        {
            Console.Write(label + " (E/H): ");
            string value = Console.ReadLine();
            if (string.Equals(value, "E", StringComparison.OrdinalIgnoreCase)) return true;
            if (string.Equals(value, "H", StringComparison.OrdinalIgnoreCase)) return false;
            Console.WriteLine("E veya H değerini girin.");
        }
    }
    private static byte[] ReadImageFile()
    {
        Console.Write("Görsel dosyası yolu (görsel eklenmeyecekse boş): ");
        string path = Console.ReadLine()?.Trim('"');
        if (string.IsNullOrWhiteSpace(path)) return null;
        FileInfo file = new(path);
        if (!file.Exists) throw new FileNotFoundException("Görsel dosyası bulunamadı.", path);
        if (file.Length > 5 * 1024 * 1024) throw new InvalidOperationException("Görsel dosyası 5 MB boyutunu aşamaz.");
        return File.ReadAllBytes(path);
    }
    private static void WriteImageFile(byte[] bytes, string defaultName)
    {
        if (bytes?.Length is not > 0)
        {
            Console.WriteLine("Bu kayıt için görsel bulunmamaktadır.");
            return;
        }
        Console.Write("Hedef dosya yolu (varsayılan: " + defaultName + "): ");
        string path = Console.ReadLine()?.Trim('"');
        if (string.IsNullOrWhiteSpace(path)) path = Path.Combine(Environment.CurrentDirectory, defaultName);
        File.WriteAllBytes(path, bytes);
        Console.WriteLine("Görsel kaydedildi: " + Path.GetFullPath(path));
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
