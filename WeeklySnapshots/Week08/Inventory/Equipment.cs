using System;

namespace CSharpProgramming.Inventory
{
    internal class Equipment
    {
        public const int MaximumNameLength = 60;
        public readonly string InventoryCode;

        public Equipment(
            string name,
            string serialNumber,
            EquipmentType type,
            EquipmentStatus status,
            Location location)
        {
            this.InventoryCode = Guid.NewGuid().ToString();
            this.Name = name;
            this.SerialNumber = serialNumber;
            this.Type = type;
            this.Status = status;
            this.Location = location;
        }

        public string Name { get; private set; }
        public string SerialNumber { get; private set; }
        public EquipmentType Type { get; private set; }
        public EquipmentStatus Status { get; set; }
        public Location Location { get; set; }

        public string GetSummary()
        {
            return this.Name + " | " +
                   this.SerialNumber + " | " +
                   this.Type + " | " +
                   this.Status + " | " +
                   this.Location.GetDescription();
        }
    }
}
