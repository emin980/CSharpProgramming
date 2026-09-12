using System.Data;
using HangarDesk.Final.Domain;
using Microsoft.Data.SqlClient;
namespace HangarDesk.Final.Infrastructure;
internal sealed class HangarDeskRepository
{
    private readonly string connectionString;
    public HangarDeskRepository(string connectionString) { this.connectionString = connectionString; }
    public DataTable Table(string procedure, params SqlParameter[] parameters)
    {
        using SqlConnection connection = new(this.connectionString);
        using SqlCommand command = new(procedure, connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.AddRange(parameters);
        using SqlDataAdapter adapter = new(command);
        DataTable table = new();
        adapter.Fill(table);
        return table;
    }
    public int Scalar(string procedure, params SqlParameter[] parameters)
    {
        using SqlConnection connection = new(this.connectionString);
        using SqlCommand command = new(procedure, connection) { CommandType = CommandType.StoredProcedure };
        command.Parameters.AddRange(parameters);
        connection.Open();
        return Convert.ToInt32(command.ExecuteScalar());
    }
    public UserCredential GetCredential(string username)
    {
        DataTable table = this.Table("dbo.usp_User_GetByUsername", P("@Username", username));
        if (table.Rows.Count == 0) return null;
        DataRow row = table.Rows[0];
        return new UserCredential
        {
            Id = (int)row["Id"], Username = (string)row["Username"], DisplayName = (string)row["DisplayName"],
            PasswordHash = (byte[])row["PasswordHash"], PasswordSalt = (byte[])row["PasswordSalt"],
            Role = (string)row["RoleName"], IsActive = (bool)row["IsActive"]
        };
    }
    public void Audit(int? userId, string action, string entity, int? entityId, string detail)
    {
        this.Scalar("dbo.usp_Audit_Create", P("@UserId", userId), P("@Action", action), P("@EntityName", entity), P("@EntityId", entityId), P("@Detail", detail));
    }
    public static SqlParameter P(string name, object value) => new(name, value ?? DBNull.Value);
}
