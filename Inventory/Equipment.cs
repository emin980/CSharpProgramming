namespace CSharpProgramming.Inventory
{
    internal abstract partial class Equipment
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public EquipmentStatus Status { get; set; }
        public abstract string GetOperationalDetails();
    }
}
