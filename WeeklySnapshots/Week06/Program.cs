using System;
using CSharpProgramming.Inventory;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Equipment[] equipmentItems = new Equipment[5];
            int equipmentCount = 0;

            Equipment battery = new Equipment();
            battery.SetInformation("LiPo Batarya", "BAT-001", "Müsait");
            battery.AddTags("enerji", "uçuş");
            equipmentItems[equipmentCount] = battery;
            equipmentCount++;

            Equipment gpsModule = new Equipment();
            gpsModule.SetInformation("GPS Modülü", "GPS-001");
            equipmentItems[equipmentCount] = gpsModule;
            equipmentCount++;

            bool applicationIsRunning = true;
            while (applicationIsRunning)
            {
                Console.WriteLine();
                Console.WriteLine("1 - Listele");
                Console.WriteLine("2 - Ekipman ekle");
                Console.WriteLine("3 - Denetim adımlarını görüntüle");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        for (int index = 0; index < equipmentCount; index++)
                        {
                            Console.WriteLine(equipmentItems[index].GetSummary());
                        }
                        break;
                    case "2":
                        if (equipmentCount < equipmentItems.Length)
                        {
                            Equipment equipment = new Equipment();
                            Console.Write("Ekipman adı: ");
                            string name = Console.ReadLine();
                            Console.Write("Seri numarası: ");
                            string serialNumber = Console.ReadLine();
                            equipment.SetInformation(name, serialNumber);
                            equipmentItems[equipmentCount] = equipment;
                            equipmentCount++;
                        }
                        break;
                    case "3":
                        battery.DisplayInspectionSteps(3);
                        break;
                    case "0":
                        applicationIsRunning = false;
                        break;
                }
            }
        }
    }
}
