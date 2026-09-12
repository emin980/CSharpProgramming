namespace CSharpProgramming.Inventory
{
    internal struct Location
    {
        public Location(string areaName, char aisleCode, int shelfNumber)
        {
            this.AreaName = areaName;
            this.AisleCode = aisleCode;
            this.ShelfNumber = shelfNumber;
        }

        public string AreaName { get; private set; }
        public char AisleCode { get; private set; }
        public int ShelfNumber { get; private set; }

        public string GetDescription()
        {
            return this.AreaName + " / " + this.AisleCode + "-" + this.ShelfNumber;
        }
    }
}
