using CSharpProgramming.Inventory;
namespace CSharpProgramming.Extensions
{
    internal static class EquipmentExtensions
    {
        public static string GetDisplayName(this Equipment equipment)
        { return equipment.Name + " (" + equipment.GetType().Name + ")"; }
    }
}
