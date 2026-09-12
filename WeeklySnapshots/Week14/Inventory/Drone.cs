namespace CSharpProgramming.Inventory
{
    internal sealed class Drone : Equipment
    {
        public int MaximumFlightMinutes { get; set; }
        public override string GetOperationalDetails() { return "Azami uçuş: " + this.MaximumFlightMinutes + " dakika"; }
    }
}
