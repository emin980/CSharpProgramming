namespace CSharpProgramming.Inventory
{
    internal class SensorKit : Equipment
    {
        public SensorKit(string name, string serial, string measurementType) : base(name, serial) { this.MeasurementType = measurementType; }
        public string MeasurementType { get; private set; }
        public override string GetOperationalDetails() { return "Sensör | Ölçüm: " + this.MeasurementType; }
    }
}
