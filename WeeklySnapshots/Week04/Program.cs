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
                Console.WriteLine();
                Console.WriteLine("=== HangarDesk ===");
                Console.WriteLine("1 - Ekipmanları listele");
                Console.WriteLine("2 - Yeni ekipman ekle");
                Console.WriteLine("3 - Ölçüm tablolarını görüntüle");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçiminiz: ");
                string menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        if (equipmentCount == 0)
                        {
                            Console.WriteLine("Kayıtlı ekipman bulunmamaktadır.");
                        }
                        else
                        {
                            for (int index = 0; index < equipmentCount; index++)
                            {
                                Console.WriteLine(
                                    (index + 1) + " - " +
                                    equipmentNames[index] + " | " +
                                    serialNumbers[index] + " | " +
                                    equipmentStatuses[index]);
                            }
                        }
                        break;

                    case "2":
                        if (equipmentCount < equipmentNames.Length)
                        {
                            Console.Write("Ekipman adı: ");
                            equipmentNames[equipmentCount] = Console.ReadLine();
                            Console.Write("Seri numarası: ");
                            serialNumbers[equipmentCount] = Console.ReadLine();
                            equipmentStatuses[equipmentCount] = "Müsait";
                            equipmentCount++;
                            Console.WriteLine("Ekipman kaydı oluşturuldu.");
                        }
                        else
                        {
                            Console.WriteLine("Ekipman kapasitesi dolmuştur.");
                        }
                        break;

                    case "3":
                        double[,] batteryMeasurements =
                        {
                            { 22.4, 22.1, 21.9 },
                            { 24.8, 24.5, 24.1 }
                        };

                        for (int batteryIndex = 0;
                             batteryIndex < batteryMeasurements.GetLength(0);
                             batteryIndex++)
                        {
                            Console.Write("Batarya " + (batteryIndex + 1) + ": ");
                            for (int measurementIndex = 0;
                                 measurementIndex < batteryMeasurements.GetLength(1);
                                 measurementIndex++)
                            {
                                Console.Write(batteryMeasurements[batteryIndex, measurementIndex] + " ");
                            }
                            Console.WriteLine();
                        }

                        string[][] maintenanceSteps =
                        {
                            new string[] { "Görsel kontrol", "Gerilim ölçümü" },
                            new string[] { "Bağlantı kontrolü", "Yazılım kontrolü", "Kalibrasyon" }
                        };

                        foreach (string[] equipmentSteps in maintenanceSteps)
                        {
                            foreach (string step in equipmentSteps)
                            {
                                Console.WriteLine("- " + step);
                            }
                        }
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
    }
}
