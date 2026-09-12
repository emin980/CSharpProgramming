namespace CSharpProgramming.Inventory
{
    internal class Tool : Equipment, IMaintainable
    {
        public Tool(string name, string serial, int calibrationMonths) : base(name, serial) { this.CalibrationMonths = calibrationMonths; }
        public int CalibrationMonths { get; private set; }
        public override string GetOperationalDetails() { return "Araç | Kalibrasyon: " + this.CalibrationMonths + " ay"; }
        public new string GetIdentity() { return "TOOL-" + base.GetIdentity(); }
        public void PerformMaintenance() { this.CalibrationMonths = 12; }
    }
}
