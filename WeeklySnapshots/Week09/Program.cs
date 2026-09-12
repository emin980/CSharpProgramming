using CSharpProgramming.Inventory;
using CSharpProgramming.Presentation;
using CSharpProgramming.Services;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InventoryService inventoryService = new InventoryService(12);
            inventoryService.Add("LiPo Batarya", "BAT-001", EquipmentType.Battery);
            inventoryService.Add("Eğitim Dronu", "DRN-001", EquipmentType.Drone);
            ConsoleMenu consoleMenu = new ConsoleMenu(inventoryService);
            consoleMenu.Run();
        }
    }
}
