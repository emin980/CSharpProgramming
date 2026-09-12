using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CSharpProgramming.Data
{
    internal class EquipmentRepository
    {
        private readonly string connectionString;

        public EquipmentRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Create(EquipmentRecord item)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = this.CreateStoredProcedure(connection, "dbo.usp_Equipment_Create"))
            {
                this.AddCreateParameters(command, item);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void CreateBatch(IEnumerable<EquipmentRecord> items)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (EquipmentRecord item in items)
                        {
                            using (SqlCommand command = this.CreateStoredProcedure(connection, "dbo.usp_Equipment_Create"))
                            {
                                command.Transaction = transaction;
                                this.AddCreateParameters(command, item);
                                command.ExecuteScalar();
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<EquipmentRecord> GetAll()
        {
            List<EquipmentRecord> items = new List<EquipmentRecord>();
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = this.CreateStoredProcedure(connection, "dbo.usp_Equipment_GetAll"))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(this.Map(reader));
                    }
                }
            }
            return items;
        }

        public bool UpdateStatus(int id, string status, string notes)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = this.CreateStoredProcedure(connection, "dbo.usp_Equipment_UpdateStatus"))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, 30).Value = status;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value =
                    notes == null ? DBNull.Value : notes;
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) == 1;
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = this.CreateStoredProcedure(connection, "dbo.usp_Equipment_Delete"))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) == 1;
            }
        }

        private SqlCommand CreateStoredProcedure(SqlConnection connection, string name)
        {
            return new SqlCommand(name, connection) { CommandType = CommandType.StoredProcedure };
        }

        private void AddCreateParameters(SqlCommand command, EquipmentRecord item)
        {
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = item.Name;
            command.Parameters.Add("@SerialNumber", SqlDbType.NVarChar, 30).Value = item.SerialNumber;
            command.Parameters.Add("@EquipmentType", SqlDbType.NVarChar, 30).Value = item.EquipmentType;
            command.Parameters.Add("@Status", SqlDbType.NVarChar, 30).Value = item.Status;
            command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value =
                item.Notes == null ? DBNull.Value : item.Notes;
        }

        private EquipmentRecord Map(SqlDataReader reader)
        {
            return new EquipmentRecord
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                SerialNumber = reader.GetString(2),
                EquipmentType = reader.GetString(3),
                Status = reader.GetString(4),
                RegisteredAt = reader.GetDateTime(5),
                Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }
    }
}
