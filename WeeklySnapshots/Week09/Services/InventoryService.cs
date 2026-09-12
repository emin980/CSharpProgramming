using CSharpProgramming.Inventory;

namespace CSharpProgramming.Services
{
    internal class InventoryService
    {
        private Equipment[] items;
        public InventoryService(int capacity) { this.items = new Equipment[capacity]; }
        public int Count { get; private set; }
        public Equipment Get(int index) { return this.items[index]; }
        public bool Add(string name, string serialNumber, EquipmentType type)
        {
            if (this.Count >= this.items.Length || this.Find(serialNumber) >= 0) return false;
            this.items[this.Count] = new Equipment(name, serialNumber, type);
            this.Count++; return true;
        }
        public int Find(string serialNumber)
        {
            for (int index = 0; index < this.Count; index++)
                if (this.items[index].SerialNumber == serialNumber) return index;
            return -1;
        }
        public bool ChangeStatus(string serialNumber, EquipmentStatus status)
        {
            int index = this.Find(serialNumber);
            if (index < 0) return false;
            this.items[index].Status = status; return true;
        }
    }
}
