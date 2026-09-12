using System.Data;
using Microsoft.Data.SqlClient;

namespace CSharpProgramming.Data
{
    internal class EquipmentDataSetService
    {
        private readonly string connectionString;

        public EquipmentDataSetService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DataSet LoadDisconnectedData()
        {
            DataSet dataSet = new DataSet("HangarDesk");
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            using (SqlCommand command = new SqlCommand("dbo.usp_Equipment_GetAll", connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;
                adapter.Fill(dataSet, "Equipment");
            }
            return dataSet;
        }
    }
}
