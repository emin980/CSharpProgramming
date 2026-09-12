using HangarDesk.Final.Application;
namespace HangarDesk.Final.UI.WinFormsUI;
internal sealed class FirstRunForm : Form
{
    private readonly HangarDeskService service;
    private readonly TextBox username = new() { PlaceholderText = "Kullanıcı adı" };
    private readonly TextBox displayName = new() { PlaceholderText = "Ad soyad" };
    private readonly TextBox password = new() { PlaceholderText = "Parola", UseSystemPasswordChar = true };
    public FirstRunForm(HangarDeskService service)
    {
        this.service = service; this.Text = "İlk Yönetici Kurulumu"; this.Width = 420; this.Height = 260; this.StartPosition = FormStartPosition.CenterScreen;
        FlowLayoutPanel panel = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Padding = new Padding(24) };
        foreach (Control control in new Control[] { new Label { Text = "İlk yönetici hesabını oluşturun.", AutoSize = true }, username, displayName, password }) { control.Width = 340; panel.Controls.Add(control); }
        Button create = new() { Text = "Yönetici Oluştur", Width = 180 }; create.Click += this.Create; panel.Controls.Add(create); this.Controls.Add(panel);
    }
    private void Create(object sender, EventArgs e)
    {
        try { this.service.CreateInitialAdmin(username.Text, displayName.Text, password.Text); this.DialogResult = DialogResult.OK; }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Kurulum", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}
