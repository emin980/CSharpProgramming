using System;
namespace CSharpProgramming.Inventory
{
    internal sealed class Drone : Equipment, IMaintainable
    {
        public Drone(string name, string serial, int flightMinutes) : base(name, serial) { this.FlightMinutes = flightMinutes; }
        public int FlightMinutes { get; private set; }
        public override string GetOperationalDetails() { return "İHA | Uçuş süresi: " + this.FlightMinutes + " dk"; }
        public override string GetSummary() { return "[DRONE] " + base.GetSummary(); }
        public void PerformMaintenance() { Console.WriteLine(this.Name + " pervane ve motor bakımı tamamlandı."); }
    }
}
