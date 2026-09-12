using System;
namespace CSharpProgramming.Inventory
{
    internal class SensorKit : Equipment
    {
        public SensorKit(string name, string serial, string measurementType) : base(name, serial)
        { this.MeasurementType = measurementType; this.categoryNote = "Sensör seti"; }
        public string MeasurementType { get; private set; }
        public void DisplayMeasurementType() { Console.WriteLine("Ölçüm türü: " + this.MeasurementType); }
    }
}
