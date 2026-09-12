using System;
using CSharpProgramming.Exceptions;
using CSharpProgramming.Inventory;
using CSharpProgramming.Services;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("HangarDesk hata ayıklama yapılandırmasıyla çalışıyor.");
#endif
            InventoryService service = new InventoryService();
            service.EquipmentStatusChanged += DisplayStatusNotification;

            EquipmentNotificationHandler notification = DisplayConsoleNotification;
            notification += RecordNotification;
            notification("Bildirim sistemi etkinleştirildi.");

            try
            {
                service.Add(new Equipment("Eğitim Dronu", "DRN-001"));
                service.ChangeStatus("DRN-001", EquipmentStatus.InMaintenance);
                service.ChangeStatus("UNKNOWN", EquipmentStatus.Lost);
            }
            catch (EquipmentNotFoundException exception)
            {
                Console.WriteLine("Kayıt hatası: " + exception.Message);
            }
            catch (InvalidEquipmentOperationException exception)
            {
                Console.WriteLine("İşlem hatası: " + exception.Message);
            }
            finally
            {
                Console.WriteLine("İşlem oturumu tamamlandı.");
            }
        }

        static void DisplayStatusNotification(object sender, EquipmentEventArgs eventArgs)
        { Console.WriteLine("Olay: " + eventArgs.Equipment.SerialNumber + " -> " + eventArgs.NewStatus); }
        static void DisplayConsoleNotification(string message) { Console.WriteLine("Konsol: " + message); }
        static void RecordNotification(string message) { Console.WriteLine("Kayıt: " + message); }
    }
}
