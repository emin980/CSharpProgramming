using System.Text.Json;
namespace HangarDesk.Final.Configuration;
internal sealed class AppSettings
{
    public DatabaseOptions Database { get; set; }
    public static AppSettings Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        AppSettings settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path))
            ?? throw new InvalidOperationException("Uygulama yapılandırması okunamadı.");
        string overrideConnection = Environment.GetEnvironmentVariable("HANGARDESK_CONNECTION_STRING");
        if (!string.IsNullOrWhiteSpace(overrideConnection))
            settings.Database.ConnectionString = overrideConnection;
        return settings;
    }
}
internal sealed class DatabaseOptions { public string ConnectionString { get; set; } }
