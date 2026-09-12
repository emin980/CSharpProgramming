using System;

namespace CSharpProgramming.Inventory
{
    internal class EquipmentCatalog
    {
        public const int MaximumCapacity = 12;
        private Equipment[] items;

        public EquipmentCatalog(int capacity)
        {
            this.CatalogId = Guid.NewGuid().ToString();
            this.items = new Equipment[capacity];
        }

        public readonly string CatalogId;
        public int Count { get; private set; }

        public Equipment this[int index]
        {
            get { return this.items[index]; }
        }

        public bool Add(Equipment equipment)
        {
            if (this.Count >= this.items.Length)
            {
                return false;
            }

            this.items[this.Count] = equipment;
            this.Count++;
            return true;
        }
    }
}
