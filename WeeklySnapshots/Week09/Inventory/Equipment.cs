namespace CSharpProgramming.Inventory
{
    internal class Equipment
    {
        public Equipment(string name, string serialNumber, EquipmentType type)
        {
            this.Name = name; this.SerialNumber = serialNumber; this.Type = type;
            this.Status = EquipmentStatus.Available;
        }
        public string Name { get; private set; }
        public string SerialNumber { get; private set; }
        public EquipmentType Type { get; private set; }
        public EquipmentStatus Status { get; set; }
        public string GetSummary() { return this.SerialNumber + " | " + this.Name + " | " + this.Type + " | " + this.Status; }
    }
}
