using System;
namespace CSharpProgramming.Inventory
{
    internal class Equipment
    {
        public Equipment(string name, string serialNumber, DateTime registeredAt)
        { this.Name = name; this.SerialNumber = serialNumber; this.RegisteredAt = registeredAt; }
        public string Name { get; private set; }
        public string SerialNumber { get; private set; }
        public DateTime RegisteredAt { get; private set; }
        public string GetSummary()
        { return string.Format("{0} | {1} | {2:dd.MM.yyyy HH:mm}", this.SerialNumber, this.Name.Trim(), this.RegisteredAt); }
    }
}
