namespace CSharpProgramming.Inventory
{
    internal class Equipment
    {
        private string serialNumber;
        protected string categoryNote;
        public Equipment(string name, string serialNumber)
        { this.Name = name; this.serialNumber = serialNumber; this.categoryNote = "Genel ekipman"; }
        public string Name { get; protected set; }
        public string SerialNumber { get { return this.serialNumber; } }
        public string GetBaseSummary() { return this.SerialNumber + " | " + this.Name + " | " + this.categoryNote; }
    }
}
