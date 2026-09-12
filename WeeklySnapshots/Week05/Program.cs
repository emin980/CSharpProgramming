using System;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] equipmentNames = { "LiPo Batarya", "GPS Modülü", "", "", "" };
            string[] serialNumbers = { "BAT-001", "GPS-001", "", "", "" };
            string[] equipmentStatuses = { "Müsait", "Bakımda", "", "", "" };
            int equipmentCount = 2;
            bool applicationIsRunning = true;

            while (applicationIsRunning)
            {
                DisplayMenu();
                string menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        ListEquipment(equipmentNames, serialNumbers, equipmentStatuses, equipmentCount);
                        break;
                    case "2":
                        AddEquipment(
                            equipmentNames,
                            serialNumbers,
                            equipmentStatuses,
                            ref equipmentCount);
                        break;
                    case "3":
                        SearchEquipment(serialNumbers, equipmentNames, equipmentCount);
                        break;
                    case "4":
                        DisplaySortedNames(equipmentNames, equipmentCount);
                        break;
                    case "0":
                        applicationIsRunning = false;
                        break;
                    default:
                        Console.WriteLine("Geçersiz menü seçimi.");
                        break;
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== HangarDesk ===");
            Console.WriteLine("1 - Ekipmanları listele");
            Console.WriteLine("2 - Yeni ekipman ekle");
            Console.WriteLine("3 - Seri numarasına göre ara");
            Console.WriteLine("4 - Adları sıralı görüntüle");
            Console.WriteLine("0 - Çıkış");
            Console.Write("Seçiminiz: ");
        }

        static void ListEquipment(
            string[] names,
            string[] serials,
            string[] statuses,
            int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Kayıtlı ekipman bulunmamaktadır.");
                return;
            }

            for (int index = 0; index < count; index++)
            {
                Console.WriteLine(
                    (index + 1) + " - " +
                    names[index] + " | " +
                    serials[index] + " | " +
                    statuses[index]);
            }
        }

        static void AddEquipment(
            string[] names,
            string[] serials,
            string[] statuses,
            ref int count)
        {
            if (count >= names.Length)
            {
                Console.WriteLine("Ekipman kapasitesi dolmuştur.");
                return;
            }

            Console.Write("Ekipman adı: ");
            names[count] = Console.ReadLine();
            Console.Write("Seri numarası: ");
            serials[count] = Console.ReadLine();
            statuses[count] = "Müsait";
            count++;
            Console.WriteLine("Ekipman kaydı oluşturuldu.");
        }

        static void SearchEquipment(
            string[] serials,
            string[] names,
            int count)
        {
            Console.Write("Aranacak seri numarası: ");
            string targetSerial = Console.ReadLine();
            int foundIndex;

            if (TryFindEquipment(serials, count, targetSerial, out foundIndex))
            {
                Console.WriteLine("Bulunan ekipman: " + names[foundIndex]);
            }
            else
            {
                Console.WriteLine("Ekipman bulunamadı.");
            }
        }

        static bool TryFindEquipment(
            string[] serials,
            int count,
            string targetSerial,
            out int foundIndex)
        {
            foundIndex = Array.IndexOf(serials, targetSerial, 0, count);
            return foundIndex >= 0;
        }

        static void DisplaySortedNames(string[] names, int count)
        {
            string[] copiedNames = new string[count];
            Array.Copy(names, copiedNames, count);
            Array.Sort(copiedNames);

            foreach (string name in copiedNames)
            {
                Console.WriteLine("- " + name);
            }
        }
    }
}
