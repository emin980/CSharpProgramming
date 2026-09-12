namespace CSharpProgramming.Inventory
{
    internal class Equipment
    {
        public Equipment(string name, string serial) { this.Name = name; this.SerialNumber = serial; this.Status = EquipmentStatus.Available; }
        public string Name { get; private set; }
        public string SerialNumber { get; private set; }
        public EquipmentStatus Status { get; set; }
    }
}
