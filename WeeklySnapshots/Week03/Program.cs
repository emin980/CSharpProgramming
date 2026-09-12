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

            Console.WriteLine("=== " + applicationName + " ===");
            Console.WriteLine("Yer: " + hangarPlace);
            Console.WriteLine("Kayitli ekipman adedi: " + equipmentCount);
            Console.WriteLine("Gorevdeki personel: " + staffOnDuty);
            Console.WriteLine("Ornek batarya voltaji: " + sampleBatteryVoltage);
            Console.WriteLine("Koridor kodu: " + aisleCode);
            Console.WriteLine("Masa hazir mi: " + deskIsReady);
            Console.WriteLine("Not: " + deskMemo);

            Console.WriteLine();
            Console.Write("Eklenecek ekipman adedini giriniz (0-12): ");
            string requestedCountText = Console.ReadLine();
            int requestedCount = Convert.ToInt32(requestedCountText);

            int updatedEquipmentCount = equipmentCount + requestedCount;
            int remainingCapacity = 12 - updatedEquipmentCount;
            int pairCount = updatedEquipmentCount / 2;
            int unpairedEquipmentCount = updatedEquipmentCount % 2;

            double equipmentCountAsDouble = updatedEquipmentCount;
            double occupancyRate = (double)updatedEquipmentCount / 12 * 100;
            int wholeVoltageValue = (int)sampleBatteryVoltage;

            object boxedEquipmentCount = updatedEquipmentCount;
            int unboxedEquipmentCount = (int)boxedEquipmentCount;
            string equipmentCountText = updatedEquipmentCount.ToString();

            int cameraFeature = 1;
            int gpsFeature = 2;
            int activeFeatures = cameraFeature | gpsFeature;
            bool gpsIsActive = (activeFeatures & gpsFeature) == gpsFeature;

            bool capacityExceeded = updatedEquipmentCount > 12;
            bool registrationCanContinue = deskIsReady && !capacityExceeded;

            staffOnDuty++;
            sampleBatteryVoltage += 0.3;

            int nextRecordNumber = checked(updatedEquipmentCount + 1);
            int uncheckedOverflowExample = unchecked(int.MaxValue + 1);

            Console.WriteLine();
            Console.WriteLine("--- Donusum ve Operator Sonuclari ---");
            Console.WriteLine("Guncel ekipman adedi: " + equipmentCountText);
            Console.WriteLine("Kalan kapasite: " + remainingCapacity);
            Console.WriteLine("Ikili ekipman grubu: " + pairCount);
            Console.WriteLine("Grup disinda kalan ekipman: " + unpairedEquipmentCount);
            Console.WriteLine("Ondalik turde ekipman adedi: " + equipmentCountAsDouble);
            Console.WriteLine("Kapasite kullanim orani: %" + occupancyRate);
            Console.WriteLine("Voltajin tam sayi kismi: " + wholeVoltageValue);
            Console.WriteLine("Boxing ve unboxing sonrasi adet: " + unboxedEquipmentCount);
            Console.WriteLine("GPS ozelligi etkin mi: " + gpsIsActive);
            Console.WriteLine("Kayit devam edebilir mi: " + registrationCanContinue);
            Console.WriteLine("Gorevdeki personel (artirim sonrasi): " + staffOnDuty);
            Console.WriteLine("Guncel ornek voltaj: " + sampleBatteryVoltage);
            Console.WriteLine("Sonraki kayit numarasi: " + nextRecordNumber);
            Console.WriteLine("Unchecked tasma ornegi: " + uncheckedOverflowExample);
            Console.WriteLine("int turunun bellekteki boyutu: " + sizeof(int) + " bayt");
            Console.WriteLine("Tur bilgisi: " + typeof(int).Name);
        }
    }
}
