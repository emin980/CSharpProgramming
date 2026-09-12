using Microsoft.Data.SqlClient;
namespace HangarDesk.Final.Infrastructure;
internal sealed class DatabaseInitializer
{
    private readonly string connectionString;
    public DatabaseInitializer(string connectionString) { this.connectionString = connectionString; }
    public void Initialize()
    {
        SqlConnectionStringBuilder builder = new(this.connectionString);
        string database = builder.InitialCatalog;
        builder.InitialCatalog = "master";
        using (SqlConnection connection = new(builder.ConnectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            connection.Open();
            command.CommandText = "IF DB_ID(@name) IS NULL EXEC('CREATE DATABASE [' + @name + ']')";
            command.Parameters.AddWithValue("@name", database);
            command.ExecuteNonQuery();
        }
        using SqlConnection appConnection = new(this.connectionString);
        appConnection.Open();
        using (SqlCommand versionTable = new("IF OBJECT_ID('dbo.SchemaVersions','U') IS NULL CREATE TABLE dbo.SchemaVersions(ScriptName NVARCHAR(200) NOT NULL PRIMARY KEY, AppliedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME())", appConnection))
            versionTable.ExecuteNonQuery();
        foreach (string file in Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "Database", "Scripts"), "*.sql").Order())
        {
            string scriptName = Path.GetFileName(file);
            using (SqlCommand exists = new("SELECT COUNT(*) FROM dbo.SchemaVersions WHERE ScriptName=@name", appConnection))
            {
                exists.Parameters.AddWithValue("@name", scriptName);
                if (Convert.ToInt32(exists.ExecuteScalar()) > 0) continue;
            }
            string script = File.ReadAllText(file);
            using SqlTransaction transaction = appConnection.BeginTransaction();
            try
            {
                foreach (string batch in System.Text.RegularExpressions.Regex.Split(script, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(batch)) continue;
                    using SqlCommand command = new(batch, appConnection, transaction);
                    command.ExecuteNonQuery();
                }
                using SqlCommand record = new("INSERT dbo.SchemaVersions(ScriptName) VALUES(@name)", appConnection, transaction);
                record.Parameters.AddWithValue("@name", scriptName);
                record.ExecuteNonQuery();
                transaction.Commit();
            }
            catch { transaction.Rollback(); throw; }
        }
    }
}
