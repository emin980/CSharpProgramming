using System;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string applicationName = "HangarDesk";
            string hangarPlace = "Lab / Hangar";

            int equipmentCount = 0;
            int staffOnDuty = 1;
            double sampleBatteryVoltage = 22.2;
            char aisleCode = 'A';
            bool deskIsReady = true;

            object deskMemo = "Zimmet listesi henuz bos";

            int copiedCount = equipmentCount;
            copiedCount = 1;

            Console.WriteLine("=== " + applicationName + " ===");
            Console.WriteLine("Yer: " + hangarPlace);
            Console.WriteLine("Kayitli ekipman adedi: " + equipmentCount);
            Console.WriteLine("Gorevdeki personel: " + staffOnDuty);
            Console.WriteLine("Ornek batarya voltaji: " + sampleBatteryVoltage);
            Console.WriteLine("Koridor kodu: " + aisleCode);
            Console.WriteLine("Masa hazir mi: " + deskIsReady);
            Console.WriteLine("Not: " + deskMemo);
            Console.WriteLine("Kopyalanan adet: " + copiedCount);
            Console.WriteLine("Orijinal adet (deger turu kopyalandi): " + equipmentCount);
        }
    }
}
