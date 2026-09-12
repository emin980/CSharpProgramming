using System;
namespace CSharpProgramming.Inventory
{
    internal class Drone : Equipment
    {
        public Drone(string name, string serial, int motorCount, int maximumFlightMinutes) : base(name, serial)
        { this.MotorCount = motorCount; this.MaximumFlightMinutes = maximumFlightMinutes; this.categoryNote = "İHA"; }
        public int MotorCount { get; private set; }
        public int MaximumFlightMinutes { get; private set; }
        public void DisplayFlightLimits() { Console.WriteLine("Motor: " + this.MotorCount + " | Azami uçuş: " + this.MaximumFlightMinutes + " dk"); }
    }
}
