namespace HangarDesk.Final.UI.WinFormsUI;

internal enum InputKind { Text, Multiline, Password, DateTime, Currency, Boolean, Choice, Image }

internal sealed record SelectionOption(object Value, string Text)
{
    public override string ToString() => this.Text;
}

internal sealed record InputField(
    string Key,
    string Label,
    InputKind Kind,
    object InitialValue = null,
    bool Required = false,
    IReadOnlyList<SelectionOption> Options = null);

internal sealed class DataEntryDialog : Form
{
    private readonly Dictionary<string, Control> inputs = new();
    private readonly IReadOnlyList<InputField> fields;

    public DataEntryDialog(string title, params InputField[] fields)
    {
        this.fields = fields;
        this.Text = title;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Width = 540;
        this.Height = Math.Min(760, 150 + fields.Sum(field => field.Kind is InputKind.Multiline or InputKind.Image ? 140 : 50));
        this.MinimizeBox = false;
        this.MaximizeBox = false;

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(14),
            ColumnCount = 2
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        foreach (InputField field in fields)
        {
            Label label = new() { Text = field.Label + (field.Required ? " *" : ""), AutoSize = true, Anchor = AnchorStyles.Left };
            Control input = CreateInput(field);
            input.Dock = DockStyle.Fill;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, field.Kind is InputKind.Multiline or InputKind.Image ? 125 : 38));
            layout.Controls.Add(label);
            layout.Controls.Add(input);
            this.inputs[field.Key] = input;
        }

        FlowLayoutPanel buttons = new() { FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
        Button save = new() { Text = "Kaydet", DialogResult = DialogResult.None, Width = 100 };
        Button cancel = new() { Text = "İptal", DialogResult = DialogResult.Cancel, Width = 100 };
        save.Click += this.Save;
        buttons.Controls.Add(save);
        buttons.Controls.Add(cancel);
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        layout.Controls.Add(buttons, 0, fields.Length);
        layout.SetColumnSpan(buttons, 2);

        this.AcceptButton = save;
        this.CancelButton = cancel;
        this.Controls.Add(layout);
        this.FormClosing += (_, _) =>
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        };
    }

    public T Value<T>(string key)
    {
        Control control = this.inputs[key];
        object value = control switch
        {
            TextBox textBox => textBox.Text,
            DateTimePicker dateTimePicker => dateTimePicker.Value,
            NumericUpDown numeric => numeric.Value,
            CheckBox checkBox => checkBox.Checked,
            ComboBox comboBox => ((SelectionOption)comboBox.SelectedItem).Value,
            ImagePicker imagePicker => imagePicker.ImageBytes,
            _ => null
        };
        if (value == null) return default;
        if (value is T typed) return typed;
        return (T)Convert.ChangeType(value, typeof(T));
    }

    private static Control CreateInput(InputField field)
    {
        switch (field.Kind)
        {
            case InputKind.Multiline:
                return new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Text = Convert.ToString(field.InitialValue) };
            case InputKind.Password:
                return new TextBox { UseSystemPasswordChar = true, Text = Convert.ToString(field.InitialValue) };
            case InputKind.DateTime:
                return new DateTimePicker
                {
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "dd.MM.yyyy HH:mm",
                    ShowUpDown = false,
                    Value = field.InitialValue is DateTime date ? date : DateTime.Now
                };
            case InputKind.Currency:
                return new NumericUpDown
                {
                    DecimalPlaces = 2,
                    ThousandsSeparator = true,
                    Minimum = 0,
                    Maximum = 1000000000,
                    Value = field.InitialValue == null || field.InitialValue == DBNull.Value ? 0 : Convert.ToDecimal(field.InitialValue)
                };
            case InputKind.Boolean:
                return new CheckBox { Checked = field.InitialValue == null || Convert.ToBoolean(field.InitialValue), AutoSize = true };
            case InputKind.Choice:
                ComboBox combo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
                if (field.Options != null)
                {
                    combo.Items.AddRange(field.Options.Cast<object>().ToArray());
                    int selected = field.InitialValue == null ? 0 : field.Options.ToList().FindIndex(option => Equals(option.Value, field.InitialValue));
                    if (combo.Items.Count > 0) combo.SelectedIndex = Math.Max(0, selected);
                }
                return combo;
            case InputKind.Image:
                return new ImagePicker(field.InitialValue as byte[]);
            default:
                return new TextBox { Text = Convert.ToString(field.InitialValue) };
        }
    }

    private void Save(object sender, EventArgs e)
    {
        foreach (InputField field in this.fields.Where(field => field.Required))
        {
            Control control = this.inputs[field.Key];
            bool missing = control is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text)
                || control is ComboBox comboBox && comboBox.SelectedItem == null;
            if (missing)
            {
                MessageBox.Show(field.Label + " alanı zorunludur.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                control.Focus();
                return;
            }
        }
        this.DialogResult = DialogResult.OK;
    }
}

internal sealed class ImagePicker : UserControl
{
    private const int MaximumFileSize = 5 * 1024 * 1024;
    private readonly PictureBox preview = new() { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
    public byte[] ImageBytes { get; private set; }

    public ImagePicker(byte[] initialImage)
    {
        this.ImageBytes = initialImage;
        Button select = new() { Text = "Görsel Seç", Dock = DockStyle.Right, Width = 100 };
        select.Click += this.SelectImage;
        this.Controls.Add(preview);
        this.Controls.Add(select);
        if (initialImage?.Length > 0) this.SetPreview(initialImage);
    }

    private void SelectImage(object sender, EventArgs e)
    {
        using OpenFileDialog dialog = new()
        {
            Title = "Görsel Dosyası Seçin",
            Filter = "Görsel Dosyaları|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Tüm Dosyalar|*.*"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        FileInfo file = new(dialog.FileName);
        if (file.Length > MaximumFileSize)
        {
            MessageBox.Show("Görsel dosyası 5 MB boyutunu aşamaz.", "Dosya Boyutu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            byte[] bytes = File.ReadAllBytes(dialog.FileName);
            this.SetPreview(bytes);
            this.ImageBytes = bytes;
        }
        catch (Exception exception)
        {
            MessageBox.Show("Görsel okunamadı: " + exception.Message, "Dosya Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void SetPreview(byte[] bytes)
    {
        using MemoryStream stream = new(bytes);
        using Image source = Image.FromStream(stream);
        Image previous = this.preview.Image;
        this.preview.Image = new Bitmap(source);
        previous?.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) this.preview.Image?.Dispose();
        base.Dispose(disposing);
    }
}
