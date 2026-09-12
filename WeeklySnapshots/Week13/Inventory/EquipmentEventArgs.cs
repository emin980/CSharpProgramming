using System;
namespace CSharpProgramming.Inventory
{
    internal class EquipmentEventArgs : EventArgs
    {
        public EquipmentEventArgs(Equipment equipment, EquipmentStatus newStatus) { this.Equipment = equipment; this.NewStatus = newStatus; }
        public Equipment Equipment { get; private set; }
        public EquipmentStatus NewStatus { get; private set; }
    }
}
