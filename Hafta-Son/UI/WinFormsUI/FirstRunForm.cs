using HangarDesk.Final.Application;
namespace HangarDesk.Final.UI.WinFormsUI;
internal sealed class FirstRunForm : Form
{
    private readonly HangarDeskService service;
    private readonly TextBox username = new() { PlaceholderText = "Kullanıcı adı" };
    private readonly TextBox displayName = new() { PlaceholderText = "Ad soyad" };
    private readonly TextBox password = new() { PlaceholderText = "Parola", UseSystemPasswordChar = true };
    private readonly TextBox confirmation = new() { PlaceholderText = "Parola doğrulama", UseSystemPasswordChar = true };
    public FirstRunForm(HangarDeskService service)
    {
        this.service = service; this.Text = "İlk Yönetici Kurulumu"; this.Width = 440; this.Height = 360; this.StartPosition = FormStartPosition.CenterScreen;
        FlowLayoutPanel panel = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(24) };
        Label information = new() { Text = "Sistemi yönetmek için ilk yönetici hesabını oluşturun.\nParola en az sekiz karakter olmalı; büyük harf, küçük harf ve rakam içermelidir.", AutoSize = true };
        foreach (Control control in new Control[] { information, username, displayName, password, confirmation }) { control.Width = 360; panel.Controls.Add(control); }
        CheckBox showPassword = new() { Text = "Parolaları göster", AutoSize = true };
        showPassword.CheckedChanged += (_, _) =>
        {
            this.password.UseSystemPasswordChar = !showPassword.Checked;
            this.confirmation.UseSystemPasswordChar = !showPassword.Checked;
        };
        panel.Controls.Add(showPassword);
        Button create = new() { Text = "Yönetici Oluştur", Width = 180 }; create.Click += this.Create; panel.Controls.Add(create); this.Controls.Add(panel);
        this.AcceptButton = create;
    }
    private void Create(object sender, EventArgs e)
    {
        try
        {
            if (this.password.Text != this.confirmation.Text) throw new ArgumentException("Parola ve parola doğrulaması eşleşmiyor.");
            this.service.CreateInitialAdmin(username.Text, displayName.Text, password.Text);
            this.DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Kurulum", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}
