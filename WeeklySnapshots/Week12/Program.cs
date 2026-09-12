using System;
using CSharpProgramming.Inventory;
using CSharpProgramming.Services;
namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Equipment[] items =
            {
                new Drone("Eğitim Dronu", "DRN-001", 32),
                new SensorKit("Çevre Sensör Seti", "SNS-001", "Sıcaklık"),
                new Tool("Tork Anahtarı", "TOL-001", 20)
            };
            EquipmentService service = new EquipmentService(items);
            service.DisplayAll();
            Console.WriteLine("Seri araması: " + service.Find("DRN-001").GetSummary());
            Console.WriteLine("Tür araması: " + service.Find(typeof(Tool)).GetSummary());
            IMaintainable maintainable = (IMaintainable)items[0];
            maintainable.PerformMaintenance();
            Console.WriteLine("Arayüz kimliği: " + ((ITrackable)items[1]).GetTrackingCode());
        }
    }
}
