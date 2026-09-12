using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CSharpProgramming.Configuration;
using CSharpProgramming.Data;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseSettings settings = DatabaseSettings.Load();
            string connectionString = settings.Database.ConnectionString;

            DatabaseInitializer initializer = new DatabaseInitializer(connectionString);
            initializer.Initialize();

            EquipmentRepository repository = new EquipmentRepository(connectionString);
            List<EquipmentRecord> currentItems = repository.GetAll();

            if (currentItems.Count == 0)
            {
                repository.CreateBatch(new EquipmentRecord[]
                {
                    new EquipmentRecord
                    {
                        Name = "Eğitim Dronu", SerialNumber = "DRN-001",
                        EquipmentType = "Drone", Status = "Available",
                        Notes = "Uçuş eğitimi için ayrılmıştır."
                    },
                    new EquipmentRecord
                    {
                        Name = "Tork Anahtarı", SerialNumber = "TOL-001",
                        EquipmentType = "Tool", Status = "InMaintenance",
                        Notes = null
                    }
                });
            }

            List<EquipmentRecord> items = repository.GetAll();
            foreach (EquipmentRecord item in items)
            {
                Console.WriteLine(
                    item.Id + " | " + item.SerialNumber + " | " +
                    item.Name + " | " + item.Status + " | " +
                    (item.Notes ?? "Açıklama bulunmamaktadır."));
            }

            DataSet disconnectedData = new EquipmentDataSetService(connectionString)
                .LoadDisconnectedData();
            IEnumerable<DataRow> maintenanceRows =
                disconnectedData.Tables["Equipment"].AsEnumerable()
                    .Where(row => row.Field<string>("Status") == "InMaintenance");

            Console.WriteLine("Bağlantısız bakım kaydı sayısı: " + maintenanceRows.Count());
            Console.WriteLine("Veritabanı ve Stored Procedure kurulumu tamamlandı.");
        }
    }
}
