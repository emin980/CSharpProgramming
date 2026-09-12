namespace CSharpProgramming.Inventory
{
    internal sealed class SensorKit : Equipment
    {
        public string MeasurementType { get; set; }
        public override string GetOperationalDetails() { return "Ölçüm türü: " + this.MeasurementType; }
    }
}
