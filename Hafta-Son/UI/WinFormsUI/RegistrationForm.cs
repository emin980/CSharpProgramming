using HangarDesk.Final.Application;

namespace HangarDesk.Final.UI.WinFormsUI;

internal sealed class RegistrationForm : Form
{
    private readonly HangarDeskService service;
    private readonly TextBox username = new();
    private readonly TextBox displayName = new();
    private readonly TextBox password = new() { UseSystemPasswordChar = true };
    private readonly TextBox confirmation = new() { UseSystemPasswordChar = true };

    public string RegisteredUsername { get; private set; }

    public RegistrationForm(HangarDeskService service)
    {
        this.service = service;
        this.Text = "HangarDesk — Üyelik Oluştur";
        this.Width = 500;
        this.Height = 410;
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            ColumnCount = 2,
            RowCount = 8
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        Label title = new()
        {
            Text = "Yeni Kullanıcı Kaydı",
            Font = new Font(SystemFonts.DefaultFont.FontFamily, 15, FontStyle.Bold),
            AutoSize = true
        };
        layout.Controls.Add(title, 0, 0);
        layout.SetColumnSpan(title, 2);
        AddRow(layout, 1, "Kullanıcı adı", this.username);
        AddRow(layout, 2, "Ad soyad", this.displayName);
        AddRow(layout, 3, "Parola", this.password);
        AddRow(layout, 4, "Parola doğrulama", this.confirmation);

        CheckBox showPassword = new() { Text = "Parolaları göster", AutoSize = true };
        showPassword.CheckedChanged += (_, _) =>
        {
            this.password.UseSystemPasswordChar = !showPassword.Checked;
            this.confirmation.UseSystemPasswordChar = !showPassword.Checked;
        };
        layout.Controls.Add(showPassword, 1, 5);

        Label information = new()
        {
            Text = "Parola en az sekiz karakter olmalı; büyük harf, küçük harf ve rakam içermelidir.\n" +
                   "Yeni hesaplar Görüntüleyici rolüyle oluşturulur. Yetki değişikliğini yalnızca yöneticiler gerçekleştirebilir.",
            AutoSize = true,
            ForeColor = Color.DimGray
        };
        layout.Controls.Add(information, 0, 6);
        layout.SetColumnSpan(information, 2);

        FlowLayoutPanel buttons = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
        Button register = new() { Text = "Üyeliği Oluştur", AutoSize = true };
        Button cancel = new() { Text = "İptal", AutoSize = true, DialogResult = DialogResult.Cancel };
        register.Click += this.Register;
        buttons.Controls.Add(register);
        buttons.Controls.Add(cancel);
        layout.Controls.Add(buttons, 0, 7);
        layout.SetColumnSpan(buttons, 2);

        this.AcceptButton = register;
        this.CancelButton = cancel;
        this.Controls.Add(layout);
    }

    private static void AddRow(TableLayoutPanel layout, int row, string label, Control input)
    {
        input.Dock = DockStyle.Fill;
        layout.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row);
        layout.Controls.Add(input, 1, row);
    }

    private void Register(object sender, EventArgs e)
    {
        try
        {
            this.service.Register(this.username.Text, this.displayName.Text, this.password.Text, this.confirmation.Text);
            this.RegisteredUsername = this.username.Text.Trim();
            MessageBox.Show(
                "Üyeliğiniz oluşturuldu. Görüntüleyici rolüyle oturum açabilirsiniz.",
                "Üyelik Tamamlandı",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Üyelik Oluşturulamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
