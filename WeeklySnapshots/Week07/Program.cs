using System;
using CSharpProgramming.Inventory;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EquipmentCatalog catalog = new EquipmentCatalog(5);
            catalog.Add(new Equipment("LiPo Batarya", "BAT-001", "Müsait"));
            catalog.Add(new Equipment("GPS Modülü", "GPS-001"));

            bool applicationIsRunning = true;
            while (applicationIsRunning)
            {
                Console.WriteLine();
                Console.WriteLine("=== HangarDesk ===");
                Console.WriteLine("Toplam oluşturulan nesne: " + Equipment.CreatedObjectCount);
                Console.WriteLine("1 - Listele");
                Console.WriteLine("2 - Durumu güncelle");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        for (int index = 0; index < catalog.Count; index++)
                        {
                            Console.WriteLine(catalog[index].GetSummary());
                        }
                        break;
                    case "2":
                        Console.Write("Kayıt numarası: ");
                        int selectedIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                        if (selectedIndex >= 0 && selectedIndex < catalog.Count)
                        {
                            Console.Write("Yeni durum: ");
                            catalog[selectedIndex].Status = Console.ReadLine();
                        }
                        break;
                    case "0":
                        applicationIsRunning = false;
                        break;
                }
            }
        }
    }
}
