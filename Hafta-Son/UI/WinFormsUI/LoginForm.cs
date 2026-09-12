using HangarDesk.Final.Application;
using HangarDesk.Final.Domain;
namespace HangarDesk.Final.UI.WinFormsUI;
internal sealed class LoginForm : Form
{
    private readonly HangarDeskService service;
    private readonly TextBox username = new();
    private readonly TextBox password = new() { UseSystemPasswordChar = true };
    public AuthenticatedUser Session { get; private set; }

    public LoginForm(HangarDeskService service)
    {
        this.service = service;
        this.Text = "HangarDesk — Oturum Aç";
        this.Width = 470;
        this.Height = 360;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(32),
            ColumnCount = 2,
            RowCount = 7
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        Label title = new()
        {
            Text = "HangarDesk",
            Font = new Font(SystemFonts.DefaultFont.FontFamily, 18, FontStyle.Bold),
            AutoSize = true
        };
        Label description = new() { Text = "Görev ve envanter yönetim sistemine giriş yapın.", AutoSize = true, ForeColor = Color.DimGray };
        layout.Controls.Add(title, 0, 0);
        layout.SetColumnSpan(title, 2);
        layout.Controls.Add(description, 0, 1);
        layout.SetColumnSpan(description, 2);
        AddRow(layout, 2, "Kullanıcı adı", this.username);
        AddRow(layout, 3, "Parola", this.password);

        CheckBox showPassword = new() { Text = "Parolayı göster", AutoSize = true };
        showPassword.CheckedChanged += (_, _) => this.password.UseSystemPasswordChar = !showPassword.Checked;
        layout.Controls.Add(showPassword, 1, 4);

        FlowLayoutPanel buttons = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
        Button login = new() { Text = "Oturum Aç", Width = 120 };
        Button register = new() { Text = "Üye Ol", Width = 100 };
        login.Click += this.Login;
        register.Click += this.OpenRegistration;
        buttons.Controls.Add(login);
        buttons.Controls.Add(register);
        layout.Controls.Add(buttons, 0, 5);
        layout.SetColumnSpan(buttons, 2);

        Label help = new()
        {
            Text = "Parolanızı unuttuysanız sistem yöneticinizden yeni bir geçici parola talep edin.",
            AutoSize = true,
            ForeColor = Color.DimGray
        };
        layout.Controls.Add(help, 0, 6);
        layout.SetColumnSpan(help, 2);

        this.AcceptButton = login;
        this.Controls.Add(layout);
    }

    private static void AddRow(TableLayoutPanel layout, int row, string label, Control input)
    {
        input.Dock = DockStyle.Fill;
        layout.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row);
        layout.Controls.Add(input, 1, row);
    }

    private void Login(object sender, EventArgs e)
    {
        this.Session = this.service.Login(this.username.Text.Trim(), this.password.Text);
        if (this.Session == null)
        {
            MessageBox.Show("Kullanıcı adı veya parola geçersizdir.", "Oturum Açılamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.password.Clear();
            this.password.Focus();
            return;
        }
        this.DialogResult = DialogResult.OK;
    }

    private void OpenRegistration(object sender, EventArgs e)
    {
        using RegistrationForm registration = new(this.service);
        if (registration.ShowDialog(this) != DialogResult.OK) return;
        this.username.Text = registration.RegisteredUsername;
        this.password.Clear();
        this.password.Focus();
    }
}
