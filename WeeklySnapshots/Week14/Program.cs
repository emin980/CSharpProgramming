using System;
using System.Collections.Generic;
using System.Linq;
using CSharpProgramming.Extensions;
using CSharpProgramming.Inventory;
using CSharpProgramming.Repositories;
using CSharpProgramming.Services;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Repository<Equipment> repository = new Repository<Equipment>();
            repository.ItemAdded += delegate(Equipment item)
            {
                Console.WriteLine("Eklendi: " + item.GetDisplayName());
            };

            repository.Add(new Drone
            {
                Name = "Eğitim Dronu", SerialNumber = "DRN-001",
                Status = EquipmentStatus.Available, MaximumFlightMinutes = 32
            });
            repository.Add(new Tool
            {
                Name = "Tork Anahtarı", SerialNumber = "TOL-001",
                Status = EquipmentStatus.InMaintenance, CalibrationMonths = 12
            });
            repository.Add(new SensorKit
            {
                Name = "Çevre Sensör Seti", SerialNumber = "SNS-001",
                Status = EquipmentStatus.Available, MeasurementType = "Sıcaklık"
            });

            var availableEquipment = repository.Items
                .Where(item => item.Status == EquipmentStatus.Available)
                .OrderBy(item => item.Name)
                .ToList();

            var summary = availableEquipment.Select(item => new
            {
                item.SerialNumber,
                DisplayName = item.GetDisplayName(),
                Detail = item.GetOperationalDetails()
            });

            foreach (var item in summary)
                Console.WriteLine(item.SerialNumber + " | " + item.DisplayName + " | " + item.Detail);

            IEnumerable<Equipment> query =
                from item in repository.Items
                where item.Name.Contains("Eğitim") || item.Name.Contains("Sensör")
                select item;

            Console.WriteLine("Sorgu sonucu: " + query.Count());
            foreach (Equipment item in repository.Iterate())
                Console.WriteLine("Iterator: " + item.SerialNumber);

            Func<Equipment, bool> requiresMaintenance =
                item => item.Status == EquipmentStatus.InMaintenance;
            Console.WriteLine("Bakım gereken kayıt: " + repository.Items.Count(requiresMaintenance));

            dynamic runtimeReport = new { Title = "HangarDesk", EquipmentCount = repository.Items.Count };
            Console.WriteLine(runtimeReport.Title + " | Toplam: " + runtimeReport.EquipmentCount);
        }
    }
}
