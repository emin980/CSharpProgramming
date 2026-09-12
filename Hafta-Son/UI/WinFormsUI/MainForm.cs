using System.Data;
using System.Globalization;
using HangarDesk.Final.Application;
using HangarDesk.Final.Domain;

namespace HangarDesk.Final.UI.WinFormsUI;

internal sealed class MainForm : Form
{
    private static readonly IReadOnlyList<SelectionOption> EquipmentStatuses =
    [
        new("Available", "Müsait"),
        new("Assigned", "Zimmetli"),
        new("InMaintenance", "Bakımda"),
        new("Lost", "Kayıp")
    ];
    private static readonly IReadOnlyList<SelectionOption> MissionStatuses =
    [
        new("Planned", "Planlandı"),
        new("Active", "Devam Ediyor"),
        new("Completed", "Tamamlandı"),
        new("Cancelled", "İptal Edildi")
    ];
    private static readonly IReadOnlyList<SelectionOption> UserRoles =
    [
        new(Roles.Administrator, "Yönetici"),
        new(Roles.Technician, "Teknik Personel"),
        new(Roles.Viewer, "Görüntüleyici")
    ];

    private readonly HangarDeskService service;
    private readonly AuthenticatedUser session;
    private readonly TabControl tabs = new() { Dock = DockStyle.Fill };
    private readonly Dictionary<string, (DataGridView Grid, Func<DataTable> Loader)> views = new();
    internal bool HasSeparatedNavigationLayout =>
        this.tabs.Parent is TableLayoutPanel layout && layout.GetRow(this.tabs) == 1 && this.tabs.TabCount >= 7;

    public MainForm(HangarDeskService service, AuthenticatedUser session)
    {
        this.service = service;
        this.session = session;
        this.Text = "HangarDesk — " + session.DisplayName + " [" + session.Role + "]";
        this.WindowState = FormWindowState.Maximized;
        TableLayoutPanel applicationLayout = new() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        applicationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        applicationLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        applicationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        ToolStrip accountBar = new() { Dock = DockStyle.Fill, GripStyle = ToolStripGripStyle.Hidden };
        accountBar.Items.Add(new ToolStripLabel("Oturum: " + session.DisplayName));
        ToolStripButton changePassword = new("Parolamı Değiştir");
        changePassword.Click += this.ChangeOwnPassword;
        accountBar.Items.Add(changePassword);
        applicationLayout.Controls.Add(accountBar, 0, 0);
        applicationLayout.Controls.Add(this.tabs, 0, 1);
        this.Controls.Add(applicationLayout);

        if (session.Role != Roles.Viewer)
        {
            this.AddTab("Ekipman", () => service.Equipment(session),
                ("Ara/Filtrele", this.SearchEquipment), ("Ekle", this.AddEquipment), ("Güncelle", this.UpdateEquipment),
                ("Sil", this.DeleteEquipment));
            this.AddTab("Materyal Türleri", () => service.EquipmentTypes(session),
                ("Tür Ekle", this.AddEquipmentType), ("Türü Güncelle", this.UpdateEquipmentType), ("Sil", this.DeleteEquipmentType));
            this.AddTab("Personel", () => service.Personnel(session),
                ("Ekle", this.AddPerson), ("Güncelle", this.UpdatePerson), ("Sil", this.DeletePerson));
            this.AddTab("Zimmet ve İade", () => service.Checkouts(session), ("Zimmet Ver", this.Checkout), ("İade Al", this.Return));
            this.AddTab("Bakım", () => service.Maintenance(session), ("Bakım Aç", this.OpenMaintenance), ("Tamamla", this.CompleteMaintenance));
            this.AddTab("Görev", () => service.Missions(session),
                ("Görev Oluştur", this.CreateMission), ("Ekipman Ata", this.AssignMission),
                ("Personel Ata", this.AssignMissionPerson), ("Durum Değiştir", this.SetMissionStatus));
        }
        else
        {
            this.AddTab("Ekipman", () => service.Equipment(session), ("Ara/Filtrele", this.SearchEquipment));
            this.AddTab("Materyal Türleri", () => service.EquipmentTypes(session));
            this.AddTab("Personel", () => service.Personnel(session));
            this.AddTab("Zimmet ve İade", () => service.Checkouts(session));
            this.AddTab("Bakım", () => service.Maintenance(session));
            this.AddTab("Görev", () => service.Missions(session));
        }

        this.AddTab("Rapor", () => service.Dashboard(session), ("Gecikenler", this.ShowOverdue));
        if (session.Role == Roles.Administrator)
            this.AddTab("Kullanıcı", () => service.Users(session),
                ("Kullanıcı Ekle", this.AddUser), ("Geçici Parola Belirle", this.ResetUserPassword),
                ("Etkinlik Değiştir", this.ToggleUser), ("Rol Değiştir", this.SetUserRole));
        if (session.Role == Roles.Administrator)
            this.AddTab("Denetim", () => service.AuditLog(session));

        this.FormClosed += (_, _) => this.service.Logout(this.session);
    }

    private void AddTab(string title, Func<DataTable> loader, params (string Text, EventHandler Handler)[] actions)
    {
        TabPage page = new(title);
        DataGridView grid = new()
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        FlowLayoutPanel bar = new() { Dock = DockStyle.Top, Height = 42 };
        Button refresh = new() { Text = "Yenile", AutoSize = true };
        refresh.Click += (_, _) => this.LoadGrid(grid, loader);
        bar.Controls.Add(refresh);
        foreach (var action in actions)
        {
            if (action.Text == "Sil" && this.session.Role != Roles.Administrator) continue;
            Button button = new() { Text = action.Text, AutoSize = true };
            button.Click += action.Handler;
            bar.Controls.Add(button);
        }
        if (title is "Ekipman" or "Personel")
        {
            SplitContainer split = new() { Dock = DockStyle.Fill, FixedPanel = FixedPanel.Panel2 };
            split.Panel1.Controls.Add(grid);
            split.SizeChanged += (_, _) =>
            {
                int target = split.Width - 300;
                if (target > split.Panel1MinSize && target < split.Width - 250)
                    split.SplitterDistance = target;
            };
            split.Panel2.Controls.Add(this.CreateDetailPanel(title, grid));
            page.Controls.Add(split);
        }
        else
        {
            page.Controls.Add(grid);
        }
        page.Controls.Add(bar);
        this.tabs.TabPages.Add(page);
        this.views[title] = (grid, loader);
        this.LoadGrid(grid, loader);
    }

    private void LoadGrid(DataGridView grid, Func<DataTable> loader)
    {
        try
        {
            grid.DataSource = loader();
            FormatGrid(grid);
        }
        catch (Exception exception) { ShowError(exception); }
    }

    private void RefreshAll()
    {
        foreach (var view in this.views.Values)
            this.LoadGrid(view.Grid, view.Loader);
    }

    private void Execute(Action operation)
    {
        try
        {
            operation();
            this.RefreshAll();
        }
        catch (Exception exception) { ShowError(exception); }
    }

    private bool TrySelectedRow(string tabName, out DataRow row)
    {
        row = null;
        if (!this.views.TryGetValue(tabName, out var view) || view.Grid.CurrentRow?.DataBoundItem is not DataRowView dataRow)
        {
            MessageBox.Show("Önce işlem yapılacak kaydı seçin.", "Kayıt Seçimi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }
        row = dataRow.Row;
        return true;
    }

    private void SearchEquipment(object sender, EventArgs e)
    {
        IReadOnlyList<SelectionOption> filterStatuses = [new("", "Tümü"), .. EquipmentStatuses];
        IReadOnlyList<SelectionOption> filterTypes = [new("", "Tümü"), .. ValueOptions(this.service.EquipmentTypes(this.session), "Name", "Name")];
        using DataEntryDialog dialog = new("Ekipman Arama ve Filtreleme",
            new("search", "Ad veya seri numarası", InputKind.Text),
            new("type", "Materyal türü", InputKind.Choice, "", false, filterTypes),
            new("status", "Durum", InputKind.Choice, "", false, filterStatuses));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        using Form form = TableForm("Ekipman Arama Sonuçları",
            this.service.Equipment(this.session, dialog.Value<string>("search"), dialog.Value<string>("type"), dialog.Value<string>("status")));
        form.ShowDialog(this);
    }

    private void AddEquipment(object sender, EventArgs e)
    {
        using DataEntryDialog dialog = EquipmentDialog("Ekipman Ekle");
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.AddEquipment(this.session,
            dialog.Value<string>("name"), dialog.Value<string>("serial"), dialog.Value<string>("type"),
            dialog.Value<string>("notes"), dialog.Value<decimal>("price"), dialog.Value<byte[]>("image")));
    }

    private void UpdateEquipment(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Ekipman", out DataRow row)) return;
        using DataEntryDialog dialog = EquipmentDialog("Ekipman Güncelle", row);
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.UpdateEquipment(this.session, Convert.ToInt32(row["Id"]),
            dialog.Value<string>("name"), dialog.Value<string>("type"), dialog.Value<string>("status"),
            dialog.Value<string>("notes"), dialog.Value<decimal>("price"), dialog.Value<byte[]>("image"), dialog.Value<string>("serial")));
    }

    private void DeleteEquipment(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Ekipman", out DataRow row) || !ConfirmDelete(Convert.ToString(row["Name"]))) return;
        this.Execute(() => this.service.DeleteEquipment(this.session, Convert.ToInt32(row["Id"])));
    }

    private void AddEquipmentType(object sender, EventArgs e)
    {
        using DataEntryDialog dialog = new("Materyal Türü Ekle",
            new("name", "Tür adı", InputKind.Text, null, true),
            new("description", "Açıklama", InputKind.Multiline));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.AddEquipmentType(this.session,
            dialog.Value<string>("name"), dialog.Value<string>("description")));
    }

    private void UpdateEquipmentType(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Materyal Türleri", out DataRow row)) return;
        using DataEntryDialog dialog = new("Materyal Türünü Güncelle",
            new("name", "Tür adı", InputKind.Text, row["Name"], true),
            new("description", "Açıklama", InputKind.Multiline, row["Description"]));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.UpdateEquipmentType(this.session, Convert.ToInt32(row["Id"]),
            dialog.Value<string>("name"), dialog.Value<string>("description")));
    }

    private void DeleteEquipmentType(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Materyal Türleri", out DataRow row) || !ConfirmDelete(Convert.ToString(row["Name"]))) return;
        this.Execute(() => this.service.DeleteEquipmentType(this.session, Convert.ToInt32(row["Id"])));
    }

    private void AddPerson(object sender, EventArgs e)
    {
        using DataEntryDialog dialog = PersonDialog("Personel Ekle");
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.AddPerson(this.session,
            dialog.Value<string>("name"), dialog.Value<string>("department"), dialog.Value<string>("phone"), dialog.Value<byte[]>("image")));
    }

    private void UpdatePerson(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Personel", out DataRow row)) return;
        using DataEntryDialog dialog = PersonDialog("Personel Güncelle", row);
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.UpdatePerson(this.session, Convert.ToInt32(row["Id"]),
            dialog.Value<string>("name"), dialog.Value<string>("department"), dialog.Value<string>("phone"),
            dialog.Value<bool>("active"), dialog.Value<byte[]>("image")));
    }

    private void DeletePerson(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Personel", out DataRow row) || !ConfirmDelete(Convert.ToString(row["FullName"]))) return;
        this.Execute(() => this.service.DeletePerson(this.session, Convert.ToInt32(row["Id"])));
    }

    private void Checkout(object sender, EventArgs e)
    {
        IReadOnlyList<SelectionOption> equipment = Options(this.service.Equipment(this.session, "", "", "Available"), "Id", "SerialNumber", "Name");
        IReadOnlyList<SelectionOption> personnel = Options(this.service.Personnel(this.session).Select("IsActive=true"), "Id", "FullName");
        using DataEntryDialog dialog = new("Zimmet Ver",
            new("equipment", "Ekipman", InputKind.Choice, null, true, equipment),
            new("personnel", "Personel", InputKind.Choice, null, true, personnel),
            new("due", "Son teslim tarihi", InputKind.DateTime, DateTime.Now.AddDays(7), true));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.Checkout(this.session,
            dialog.Value<int>("equipment"), dialog.Value<int>("personnel"), dialog.Value<DateTime>("due")));
    }

    private void Return(object sender, EventArgs e)
    {
        DataRow[] active = this.service.Checkouts(this.session).Select("ReturnedAt IS NULL");
        using DataEntryDialog dialog = new("İade Al",
            new("checkout", "Açık zimmet", InputKind.Choice, null, true, Options(active, "Id", "Equipment", "Personnel")),
            new("notes", "İade açıklaması", InputKind.Multiline));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.Return(this.session, dialog.Value<int>("checkout"), dialog.Value<string>("notes")));
    }

    private void OpenMaintenance(object sender, EventArgs e)
    {
        IReadOnlyList<SelectionOption> equipment = Options(this.service.Equipment(this.session, "", "", "Available"), "Id", "SerialNumber", "Name");
        using DataEntryDialog dialog = new("Bakım Kaydı Aç",
            new("equipment", "Ekipman", InputKind.Choice, null, true, equipment),
            new("description", "Bakım açıklaması", InputKind.Multiline, null, true));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.OpenMaintenance(this.session, dialog.Value<int>("equipment"), dialog.Value<string>("description")));
    }

    private void CompleteMaintenance(object sender, EventArgs e)
    {
        DataRow[] open = this.service.Maintenance(this.session).Select("CompletedAt IS NULL");
        using DataEntryDialog dialog = new("Bakımı Tamamla",
            new("maintenance", "Açık bakım", InputKind.Choice, null, true, Options(open, "Id", "Equipment", "Description")),
            new("result", "Bakım sonucu", InputKind.Multiline, null, true));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.CompleteMaintenance(this.session, dialog.Value<int>("maintenance"), dialog.Value<string>("result")));
    }

    private void CreateMission(object sender, EventArgs e)
    {
        using DataEntryDialog dialog = new("Görev Oluştur",
            new("name", "Görev adı", InputKind.Text, null, true),
            new("description", "Açıklama", InputKind.Multiline),
            new("date", "Planlanan tarih", InputKind.DateTime, DateTime.Now.AddDays(1), true));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.CreateMission(this.session,
            dialog.Value<string>("name"), dialog.Value<string>("description"), dialog.Value<DateTime>("date")));
    }

    private void AssignMission(object sender, EventArgs e)
    {
        using DataEntryDialog dialog = new("Göreve Ekipman Ata",
            new("mission", "Görev", InputKind.Choice, null, true, Options(this.service.Missions(this.session), "Id", "Name")),
            new("equipment", "Ekipman", InputKind.Choice, null, true, Options(this.service.Equipment(this.session), "Id", "SerialNumber", "Name")));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.AssignMissionEquipment(this.session, dialog.Value<int>("mission"), dialog.Value<int>("equipment")));
    }

    private void AssignMissionPerson(object sender, EventArgs e)
    {
        using DataEntryDialog dialog = new("Göreve Personel Ata",
            new("mission", "Görev", InputKind.Choice, null, true, Options(this.service.Missions(this.session), "Id", "Name")),
            new("personnel", "Personel", InputKind.Choice, null, true, Options(this.service.Personnel(this.session).Select("IsActive=true"), "Id", "FullName")));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.AssignMissionPersonnel(this.session, dialog.Value<int>("mission"), dialog.Value<int>("personnel")));
    }

    private void SetMissionStatus(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Görev", out DataRow row)) return;
        using DataEntryDialog dialog = new("Görev Durumunu Değiştir",
            new InputField("status", "Durum", InputKind.Choice, Convert.ToString(row["Status"]), true, MissionStatuses));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.SetMissionStatus(this.session, Convert.ToInt32(row["Id"]), dialog.Value<string>("status")));
    }

    private void ShowOverdue(object sender, EventArgs e)
    {
        using Form form = TableForm("Geciken Zimmetler", this.service.Overdue(this.session));
        form.ShowDialog(this);
    }

    private void AddUser(object sender, EventArgs e)
    {
        using DataEntryDialog dialog = new("Kullanıcı Ekle",
            new("username", "Kullanıcı adı", InputKind.Text, null, true),
            new("display", "Ad soyad", InputKind.Text, null, true),
            new("password", "Geçici parola", InputKind.Password, null, true),
            new("confirmation", "Parola doğrulama", InputKind.Password, null, true),
            new("role", "Rol", InputKind.Choice, Roles.Viewer, true, UserRoles));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() =>
        {
            string password = dialog.Value<string>("password");
            if (password != dialog.Value<string>("confirmation"))
                throw new ArgumentException("Parola ve parola doğrulaması eşleşmiyor.");
            this.service.CreateUser(this.session, dialog.Value<string>("username"),
                dialog.Value<string>("display"), password, dialog.Value<string>("role"));
        });
    }

    private void ChangeOwnPassword(object sender, EventArgs e)
    {
        using DataEntryDialog dialog = new("Parolamı Değiştir",
            new("current", "Mevcut parola", InputKind.Password, null, true),
            new("new", "Yeni parola", InputKind.Password, null, true),
            new("confirmation", "Yeni parola doğrulama", InputKind.Password, null, true));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.ChangePassword(this.session,
            dialog.Value<string>("current"), dialog.Value<string>("new"), dialog.Value<string>("confirmation")));
    }

    private void ResetUserPassword(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Kullanıcı", out DataRow row)) return;
        using DataEntryDialog dialog = new("Geçici Parola Belirle",
            new("password", "Yeni geçici parola", InputKind.Password, null, true),
            new("confirmation", "Parola doğrulama", InputKind.Password, null, true));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.ResetPassword(this.session, Convert.ToInt32(row["Id"]),
            dialog.Value<string>("password"), dialog.Value<string>("confirmation")));
    }

    private void ToggleUser(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Kullanıcı", out DataRow row)) return;
        using DataEntryDialog dialog = new("Kullanıcı Etkinliği",
            new InputField("active", "Kullanıcı etkin", InputKind.Boolean, row["IsActive"]));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.SetUserActive(this.session, Convert.ToInt32(row["Id"]), dialog.Value<bool>("active")));
    }

    private void SetUserRole(object sender, EventArgs e)
    {
        if (!this.TrySelectedRow("Kullanıcı", out DataRow row)) return;
        using DataEntryDialog dialog = new("Kullanıcı Rolü",
            new InputField("role", "Rol", InputKind.Choice, Convert.ToString(row["Role"]), true, UserRoles));
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        this.Execute(() => this.service.SetUserRole(this.session, Convert.ToInt32(row["Id"]), dialog.Value<string>("role")));
    }

    private DataEntryDialog EquipmentDialog(string title, DataRow row = null)
    {
        return new DataEntryDialog(title,
            new("name", "Ekipman adı", InputKind.Text, row?["Name"], true),
            new("serial", "Seri numarası", InputKind.Text, row?["SerialNumber"], true),
            new("type", "Materyal türü", InputKind.Choice, row?["EquipmentType"], true,
                ValueOptions(this.service.EquipmentTypes(this.session), "Name", "Name")),
            new("status", "Durum", InputKind.Choice, row?["Status"] ?? "Available", true, EquipmentStatuses),
            new("price", "Alış bedeli", InputKind.Currency, row?["PurchasePrice"]),
            new("notes", "Açıklama", InputKind.Multiline, row?["Notes"]),
            new("image", "Ürün görseli", InputKind.Image));
    }

    private static DataEntryDialog PersonDialog(string title, DataRow row = null)
    {
        return new DataEntryDialog(title,
            new("name", "Ad soyad", InputKind.Text, row?["FullName"], true),
            new("department", "Bölüm", InputKind.Text, row?["Department"]),
            new("phone", "Telefon", InputKind.Text, row?["Phone"]),
            new("active", "Etkin personel", InputKind.Boolean, row?["IsActive"] ?? true),
            new("image", "Personel görseli", InputKind.Image));
    }

    private Control CreateDetailPanel(string entity, DataGridView grid)
    {
        PictureBox image = new()
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.WhiteSmoke
        };
        Label heading = new()
        {
            Text = entity + " Ayrıntısı",
            Dock = DockStyle.Fill,
            Font = new Font(SystemFonts.DefaultFont.FontFamily, 12, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        Label details = new() { Dock = DockStyle.Fill, AutoSize = false, Padding = new Padding(4) };
        TableLayoutPanel panel = new() { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 3, ColumnCount = 1 };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 55));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
        panel.Controls.Add(heading, 0, 0);
        panel.Controls.Add(image, 0, 1);
        panel.Controls.Add(details, 0, 2);

        void UpdatePreview()
        {
            if (grid.CurrentRow?.DataBoundItem is not DataRowView selected)
            {
                image.Image?.Dispose();
                image.Image = null;
                details.Text = "Ayrıntılarını incelemek için bir kayıt seçin.";
                return;
            }
            try
            {
                DataRow row = selected.Row;
                byte[] bytes = entity == "Ekipman"
                    ? this.service.EquipmentImage(this.session, Convert.ToInt32(row["Id"]))
                    : this.service.PersonnelImage(this.session, Convert.ToInt32(row["Id"]));
                Image previous = image.Image;
                image.Image = BytesToImage(bytes);
                previous?.Dispose();
                details.Text = entity == "Ekipman"
                    ? $"Ad: {row["Name"]}\nSeri numarası: {row["SerialNumber"]}\nTür: {row["EquipmentType"]}\n" +
                      $"Durum: {row["Status"]}\nAlış bedeli: {FormatPrice(row["PurchasePrice"])}\nAçıklama: {row["Notes"]}"
                    : $"Ad soyad: {row["FullName"]}\nBölüm: {row["Department"]}\nTelefon: {row["Phone"]}\n" +
                      $"Durum: {(Convert.ToBoolean(row["IsActive"]) ? "Etkin" : "Pasif")}";
                if (image.Image == null) details.Text += "\n\nGörsel bulunmamaktadır.";
            }
            catch (Exception exception)
            {
                details.Text = "Ayrıntı yüklenemedi: " + exception.Message;
            }
        }

        grid.SelectionChanged += (_, _) => UpdatePreview();
        grid.DataBindingComplete += (_, _) => UpdatePreview();
        panel.Disposed += (_, _) => image.Image?.Dispose();
        return panel;
    }

    private static IReadOnlyList<SelectionOption> Options(DataTable table, string idColumn, params string[] textColumns)
        => Options(table.Rows.Cast<DataRow>(), idColumn, textColumns);

    private static IReadOnlyList<SelectionOption> Options(IEnumerable<DataRow> rows, string idColumn, params string[] textColumns)
        => rows.Select(row => new SelectionOption(
            Convert.ToInt32(row[idColumn]),
            string.Join(" — ", textColumns.Select(column => Convert.ToString(row[column]))))).ToList();

    private static IReadOnlyList<SelectionOption> ValueOptions(DataTable table, string valueColumn, params string[] textColumns)
        => table.Rows.Cast<DataRow>().Select(row => new SelectionOption(
            row[valueColumn],
            string.Join(" — ", textColumns.Select(column => Convert.ToString(row[column]))))).ToList();

    private static Image BytesToImage(byte[] bytes)
    {
        if (bytes?.Length is not > 0) return null;
        using MemoryStream stream = new(bytes);
        using Image source = Image.FromStream(stream);
        return new Bitmap(source);
    }

    private static string FormatPrice(object value)
        => value == DBNull.Value ? "-" : Convert.ToDecimal(value).ToString("C2", CultureInfo.GetCultureInfo("tr-TR"));

    private static bool ConfirmDelete(string name)
        => MessageBox.Show(name + " kaydı silinmiş olarak işaretlenecektir. Devam edilsin mi?",
            "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;

    private static void ShowImage(string title, byte[] bytes)
    {
        if (bytes?.Length is not > 0)
        {
            MessageBox.Show("Bu kayıt için görsel bulunmamaktadır.", "Görsel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using MemoryStream stream = new(bytes);
        using Image source = Image.FromStream(stream);
        using Bitmap display = new(source);
        using Form form = new() { Text = title, Width = 700, Height = 600, StartPosition = FormStartPosition.CenterParent };
        form.Controls.Add(new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, Image = display });
        form.ShowDialog();
    }

    private static Form TableForm(string title, DataTable data)
    {
        Form form = new() { Text = title, Width = 1000, Height = 550, StartPosition = FormStartPosition.CenterParent };
        DataGridView grid = new()
        {
            Dock = DockStyle.Fill,
            DataSource = data,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        FormatGrid(grid);
        form.Controls.Add(grid);
        return form;
    }

    private static void FormatGrid(DataGridView grid)
    {
        CultureInfo culture = CultureInfo.GetCultureInfo("tr-TR");
        foreach (DataGridViewColumn column in grid.Columns)
        {
            column.DefaultCellStyle.FormatProvider = culture;
            if (column.ValueType == typeof(DateTime))
                column.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            else if (column.ValueType == typeof(decimal))
                column.DefaultCellStyle.Format = "C2";
        }
    }

    private static void ShowError(Exception exception)
        => MessageBox.Show(exception.Message, "İşlem Gerçekleştirilemedi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
