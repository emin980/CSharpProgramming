using System;
using CSharpProgramming.Inventory;
namespace CSharpProgramming.Services
{
    internal class EquipmentService
    {
        private Equipment[] items;
        public EquipmentService(Equipment[] items) { this.items = items; }
        public void DisplayAll() { foreach (Equipment item in this.items) Console.WriteLine(item.GetSummary()); }
        public Equipment Find(string serial)
        { foreach (Equipment item in this.items) if (item.SerialNumber == serial) return item; return null; }
        public Equipment Find(Type equipmentType)
        { foreach (Equipment item in this.items) if (item.GetType() == equipmentType) return item; return null; }
    }
}
