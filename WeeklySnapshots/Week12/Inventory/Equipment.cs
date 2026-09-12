namespace CSharpProgramming.Inventory
{
    internal abstract class Equipment : ITrackable
    {
        protected Equipment(string name, string serial) { this.Name = name; this.SerialNumber = serial; }
        public string Name { get; protected set; }
        public string SerialNumber { get; private set; }
        public virtual string GetSummary() { return this.GetIdentity() + " | " + this.GetOperationalDetails(); }
        public string GetIdentity() { return this.SerialNumber + " | " + this.Name; }
        public string GetTrackingCode() { return "HD-" + this.SerialNumber; }
        public abstract string GetOperationalDetails();
    }
}
