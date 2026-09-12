namespace CSharpProgramming.Inventory
{
    internal class Equipment
    {
        private string name;
        private string serialNumber;
        private string status;

        public static int CreatedObjectCount { get; private set; }
        public static string InventoryPrefix { get; private set; }

        static Equipment()
        {
            InventoryPrefix = "HD";
            CreatedObjectCount = 0;
        }

        public Equipment()
            : this("Tanımsız Ekipman", "Tanımsız", "Müsait")
        {
        }

        public Equipment(string name, string serialNumber)
            : this(name, serialNumber, "Müsait")
        {
        }

        public Equipment(string name, string serialNumber, string status)
        {
            this.Name = name;
            this.SerialNumber = serialNumber;
            this.Status = status;
            CreatedObjectCount++;
        }

        ~Equipment()
        {
            CreatedObjectCount--;
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                if (value != "")
                {
                    this.name = value;
                }
            }
        }

        public string SerialNumber
        {
            get { return this.serialNumber; }
            private set { this.serialNumber = value; }
        }

        public string Status
        {
            get { return this.status; }
            set
            {
                if (value != "")
                {
                    this.status = value;
                }
            }
        }

        public string GetSummary()
        {
            return InventoryPrefix + " | " + this.Name + " | " +
                   this.SerialNumber + " | " + this.Status;
        }
    }
}
