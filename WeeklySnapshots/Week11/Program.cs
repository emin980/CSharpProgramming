using System;
using CSharpProgramming.Inventory;
namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Equipment[] items =
            {
                new Drone("Eğitim Dronu", "DRN-001", 4, 32),
                new SensorKit("Çevre Sensör Seti", "SNS-001", "Sıcaklık"),
                new Tool("Tork Anahtarı", "TOL-001", 20)
            };
            foreach (Equipment item in items) Console.WriteLine(item.GetBaseSummary());
            ((Drone)items[0]).DisplayFlightLimits();
            ((SensorKit)items[1]).DisplayMeasurementType();
            ((Tool)items[2]).DisplayCalibrationInterval();
        }
    }
}
