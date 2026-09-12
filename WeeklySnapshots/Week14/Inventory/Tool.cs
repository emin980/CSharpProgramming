namespace CSharpProgramming.Inventory
{
    internal sealed class Tool : Equipment
    {
        public int CalibrationMonths { get; set; }
        public override string GetOperationalDetails() { return "Kalibrasyon aralığı: " + this.CalibrationMonths + " ay"; }
    }
}
