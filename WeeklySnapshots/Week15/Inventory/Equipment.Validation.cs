namespace CSharpProgramming.Inventory
{
    internal abstract partial class Equipment
    {
        public bool HasValidIdentity()
        { return !string.IsNullOrWhiteSpace(this.Name) && !string.IsNullOrWhiteSpace(this.SerialNumber); }
    }
}
