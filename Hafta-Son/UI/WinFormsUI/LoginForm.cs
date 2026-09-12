using HangarDesk.Final.Application;
using HangarDesk.Final.Domain;
namespace HangarDesk.Final.UI.WinFormsUI;
internal sealed class LoginForm : Form
{
    private readonly HangarDeskService service;
    private readonly TextBox username = new() { PlaceholderText = "Kullanıcı adı" };
    private readonly TextBox password = new() { PlaceholderText = "Parola", UseSystemPasswordChar = true };
    public AuthenticatedUser Session { get; private set; }
    public LoginForm(HangarDeskService service)
    {
        this.service = service; this.Text = "HangarDesk Giriş"; this.Width = 380; this.Height = 220; this.StartPosition = FormStartPosition.CenterScreen;
        FlowLayoutPanel panel = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(24) };
        username.Width = password.Width = 300; panel.Controls.Add(username); panel.Controls.Add(password);
        Button login = new() { Text = "Oturum Aç", Width = 150 }; login.Click += this.Login; panel.Controls.Add(login); this.Controls.Add(panel);
    }
    private void Login(object sender, EventArgs e)
    {
        this.Session = this.service.Login(username.Text, password.Text);
        if (this.Session == null) MessageBox.Show("Kullanıcı adı veya parola geçersizdir."); else this.DialogResult = DialogResult.OK;
    }
}
