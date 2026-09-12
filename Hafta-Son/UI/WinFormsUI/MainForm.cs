using System.Data;
using HangarDesk.Final.Application;
using HangarDesk.Final.Domain;
using Microsoft.VisualBasic;
namespace HangarDesk.Final.UI.WinFormsUI;
internal sealed class MainForm : Form
{
    private readonly HangarDeskService service;
    private readonly AuthenticatedUser session;
    private readonly TabControl tabs = new() { Dock = DockStyle.Fill };
    public MainForm(HangarDeskService service, AuthenticatedUser session)
    {
        this.service = service; this.session = session; this.Text = "HangarDesk — " + session.DisplayName + " [" + session.Role + "]";
        this.WindowState = FormWindowState.Maximized; this.Controls.Add(tabs);
        bool canChange = session.Role != Roles.Viewer;
        if (canChange)
        {
            this.AddTab("Ekipman", () => service.Equipment(session), ("Ara", this.SearchEquipment), ("Ekle", this.AddEquipment), ("Güncelle", this.UpdateEquipment), ("Sil", this.DeleteEquipment));
            this.AddTab("Personel", () => service.Personnel(session), ("Ekle", this.AddPerson), ("Güncelle", this.UpdatePerson), ("Sil", this.DeletePerson));
            this.AddTab("Zimmet ve İade", () => service.Checkouts(session), ("Zimmet Ver", this.Checkout), ("İade Al", this.Return));
            this.AddTab("Bakım", () => service.Maintenance(session), ("Bakım Aç", this.OpenMaintenance), ("Tamamla", this.CompleteMaintenance));
            this.AddTab("Görev", () => service.Missions(session), ("Görev Oluştur", this.CreateMission), ("Ekipman Ata", this.AssignMission), ("Personel Ata", this.AssignMissionPerson), ("Durum Değiştir", this.SetMissionStatus));
        }
        else
        {
            this.AddTab("Ekipman", () => service.Equipment(session), ("Ara", this.SearchEquipment));
            this.AddTab("Personel", () => service.Personnel(session));
            this.AddTab("Zimmet ve İade", () => service.Checkouts(session));
            this.AddTab("Bakım", () => service.Maintenance(session));
            this.AddTab("Görev", () => service.Missions(session));
        }
        this.AddTab("Rapor", () => service.Dashboard(session), ("Gecikenler", this.ShowOverdue));
        if (session.Role == Roles.Administrator) this.AddTab("Kullanıcı", () => service.Users(session), ("Kullanıcı Ekle", this.AddUser), ("Etkinlik Değiştir", this.ToggleUser), ("Rol Değiştir", this.SetUserRole));
        if (session.Role == Roles.Administrator) this.AddTab("Denetim", () => service.AuditLog(session));
        this.FormClosed += (_, _) => this.service.Logout(this.session);
    }
    private void AddTab(string title, Func<DataTable> loader, params (string Text, EventHandler Handler)[] actions)
    {
        TabPage page = new(title); DataGridView grid = new() { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        FlowLayoutPanel bar = new() { Dock = DockStyle.Top, Height = 42 };
        Button refresh = new() { Text = "Yenile" }; refresh.Click += (_, _) => this.LoadGrid(grid, loader); bar.Controls.Add(refresh);
        foreach (var action in actions)
        {
            if (action.Text == "Sil" && this.session.Role != Roles.Administrator) continue;
            Button button = new() { Text = action.Text, AutoSize = true };
            button.Click += action.Handler;
            bar.Controls.Add(button);
        }
        page.Controls.Add(grid); page.Controls.Add(bar); tabs.TabPages.Add(page); this.LoadGrid(grid, loader);
    }
    private void LoadGrid(DataGridView grid, Func<DataTable> loader) { try { grid.DataSource = loader(); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private static string Ask(string prompt) => Interaction.InputBox(prompt, "HangarDesk");
    private void SearchEquipment(object s, EventArgs e) { using Form form = TableForm("Ekipman Arama Sonuçları", service.Equipment(session, Ask("Ad veya seri numarası"))); form.ShowDialog(); }
    private void AddEquipment(object s, EventArgs e) { try { service.AddEquipment(session, Ask("Ekipman adı"), Ask("Seri numarası"), Ask("Tür"), Ask("Açıklama")); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void UpdateEquipment(object s, EventArgs e) { try { service.UpdateEquipment(session, int.Parse(Ask("Ekipman Id")), Ask("Ekipman adı"), Ask("Tür"), Ask("Durum (Available/Assigned/InMaintenance/Lost)"), Ask("Açıklama")); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void DeleteEquipment(object s, EventArgs e) { try { service.DeleteEquipment(session, int.Parse(Ask("Ekipman Id"))); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void AddPerson(object s, EventArgs e) { try { service.AddPerson(session, Ask("Ad soyad"), Ask("Bölüm"), Ask("Telefon")); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void UpdatePerson(object s, EventArgs e) { try { service.UpdatePerson(session, int.Parse(Ask("Personel Id")), Ask("Ad soyad"), Ask("Bölüm"), Ask("Telefon"), bool.Parse(Ask("Etkin mi? true/false"))); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void DeletePerson(object s, EventArgs e) { try { service.DeletePerson(session, int.Parse(Ask("Personel Id"))); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void Checkout(object s, EventArgs e) { try { service.Checkout(session, int.Parse(Ask("Ekipman Id")), int.Parse(Ask("Personel Id")), DateTime.Now.AddDays(int.Parse(Ask("Süre (gün)")))); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void Return(object s, EventArgs e) { try { service.Return(session, int.Parse(Ask("Zimmet Id")), Ask("İade notu")); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void OpenMaintenance(object s, EventArgs e) { try { service.OpenMaintenance(session, int.Parse(Ask("Ekipman Id")), Ask("Bakım açıklaması")); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void CompleteMaintenance(object s, EventArgs e) { try { service.CompleteMaintenance(session, int.Parse(Ask("Bakım Id")), Ask("Sonuç")); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void CreateMission(object s, EventArgs e) { try { service.CreateMission(session, Ask("Görev adı"), Ask("Açıklama"), DateTime.Parse(Ask("Plan tarihi"))); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void AssignMission(object s, EventArgs e) { try { service.AssignMissionEquipment(session, int.Parse(Ask("Görev Id")), int.Parse(Ask("Ekipman Id"))); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void AssignMissionPerson(object s, EventArgs e) { try { service.AssignMissionPersonnel(session, int.Parse(Ask("Görev Id")), int.Parse(Ask("Personel Id"))); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void SetMissionStatus(object s, EventArgs e) { try { service.SetMissionStatus(session, int.Parse(Ask("Görev Id")), Ask("Durum (Planned/Active/Completed/Cancelled)")); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void ShowOverdue(object s, EventArgs e) { using Form form = TableForm("Geciken Zimmetler", service.Overdue(session)); form.ShowDialog(); }
    private void AddUser(object s, EventArgs e) { try { service.CreateUser(session, Ask("Kullanıcı adı"), Ask("Ad soyad"), Ask("Parola"), Ask("Rol")); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void ToggleUser(object s, EventArgs e) { try { service.SetUserActive(session, int.Parse(Ask("Kullanıcı Id")), bool.Parse(Ask("Etkin mi? true/false"))); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private void SetUserRole(object s, EventArgs e) { try { service.SetUserRole(session, int.Parse(Ask("Kullanıcı Id")), Ask("Rol (Administrator/Technician/Viewer)")); } catch (Exception ex) { MessageBox.Show(ex.Message); } }
    private static Form TableForm(string title, DataTable data) { Form form = new() { Text = title, Width = 900, Height = 500 }; form.Controls.Add(new DataGridView { Dock = DockStyle.Fill, DataSource = data, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill }); return form; }
}
