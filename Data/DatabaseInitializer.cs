using Microsoft.Data.SqlClient;

namespace CSharpProgramming.Data
{
    internal class DatabaseInitializer
    {
        private readonly string connectionString;

        public DatabaseInitializer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Initialize()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(this.connectionString);
            string databaseName = builder.InitialCatalog;
            builder.InitialCatalog = "master";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText =
                    "IF DB_ID(@DatabaseName) IS NULL " +
                    "BEGIN EXEC('CREATE DATABASE [' + @DatabaseName + ']') END";
                command.Parameters.AddWithValue("@DatabaseName", databaseName);
                command.ExecuteNonQuery();
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                this.Execute(connection, TableScript);
                this.Execute(connection, CreateProcedureScript);
                this.Execute(connection, GetAllProcedureScript);
                this.Execute(connection, GetByIdProcedureScript);
                this.Execute(connection, UpdateProcedureScript);
                this.Execute(connection, DeleteProcedureScript);
            }
        }

        private void Execute(SqlConnection connection, string sql)
        {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private const string TableScript = @"
IF OBJECT_ID('dbo.Equipment', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Equipment
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        SerialNumber NVARCHAR(30) NOT NULL UNIQUE,
        EquipmentType NVARCHAR(30) NOT NULL,
        Status NVARCHAR(30) NOT NULL,
        RegisteredAt DATETIME2 NOT NULL CONSTRAINT DF_Equipment_RegisteredAt DEFAULT SYSUTCDATETIME(),
        Notes NVARCHAR(500) NULL
    );
END;";

        private const string CreateProcedureScript = @"
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Create
    @Name NVARCHAR(100), @SerialNumber NVARCHAR(30),
    @EquipmentType NVARCHAR(30), @Status NVARCHAR(30),
    @Notes NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.Equipment(Name, SerialNumber, EquipmentType, Status, Notes)
    VALUES(@Name, @SerialNumber, @EquipmentType, @Status, @Notes);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;";

        private const string GetAllProcedureScript = @"
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_GetAll AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, SerialNumber, EquipmentType, Status, RegisteredAt, Notes
    FROM dbo.Equipment ORDER BY Name;
END;";

        private const string GetByIdProcedureScript = @"
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_GetById @Id INT AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, SerialNumber, EquipmentType, Status, RegisteredAt, Notes
    FROM dbo.Equipment WHERE Id = @Id;
END;";

        private const string UpdateProcedureScript = @"
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_UpdateStatus
    @Id INT, @Status NVARCHAR(30), @Notes NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Equipment SET Status = @Status, Notes = @Notes WHERE Id = @Id;
    SELECT @@ROWCOUNT;
END;";

        private const string DeleteProcedureScript = @"
CREATE OR ALTER PROCEDURE dbo.usp_Equipment_Delete @Id INT AS
BEGIN
    SET NOCOUNT ON;
    DELETE dbo.Equipment WHERE Id = @Id;
    SELECT @@ROWCOUNT;
END;";
    }
}
