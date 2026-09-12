using System;
using System.IO;
using System.Text.Json;

namespace CSharpProgramming.Configuration
{
    internal class DatabaseSettings
    {
        public DatabaseSection Database { get; set; }

        public static DatabaseSettings Load()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<DatabaseSettings>(json);
        }
    }

    internal class DatabaseSection
    {
        public string ConnectionString { get; set; }
    }
}
