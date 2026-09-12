namespace CSharpProgramming.Inventory
{
    internal class EquipmentCatalog
    {
        private Equipment[] items;

        public EquipmentCatalog(int capacity)
        {
            this.items = new Equipment[capacity];
            this.Count = 0;
        }

        public int Count { get; private set; }

        public Equipment this[int index]
        {
            get { return this.items[index]; }
            set { this.items[index] = value; }
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
