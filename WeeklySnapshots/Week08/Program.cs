using System;
using CSharpProgramming.Inventory;
using Text = System.String;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EquipmentCatalog catalog = new EquipmentCatalog(EquipmentCatalog.MaximumCapacity);

            Location hangarLocation = new Location("Ana Hangar", 'A', 3);
            Equipment battery = new Equipment(
                "LiPo Batarya",
                "BAT-001",
                EquipmentType.Battery,
                EquipmentStatus.Available,
                hangarLocation);

            Equipment drone = new Equipment(
                "Eğitim Dronu",
                "DRN-001",
                EquipmentType.Drone,
                EquipmentStatus.Assigned,
                new Location("Uçuş Sahası", 'B', 1));

            catalog.Add(battery);
            catalog.Add(drone);

            Text[] statusNames = Enum.GetNames(typeof(EquipmentStatus));
            Console.WriteLine("Tanımlı durumlar: " + string.Join(", ", statusNames));
            Console.WriteLine("Katalog oluşturma kodu: " + catalog.CatalogId);
            Console.WriteLine();

            for (int index = 0; index < catalog.Count; index++)
            {
                Console.WriteLine(catalog[index].GetSummary());
            }
        }
    }
}
