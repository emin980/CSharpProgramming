using System;
using CSharpProgramming.Inventory;
using CSharpProgramming.Services;

namespace CSharpProgramming.Presentation
{
    internal class ConsoleMenu
    {
        private InventoryService service;
        public ConsoleMenu(InventoryService service) { this.service = service; }
        public void Run()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n1-Listele 2-Ekle 3-Durum değiştir 0-Çıkış");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": this.List(); break;
                    case "2": this.Add(); break;
                    case "3": this.ChangeStatus(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Geçersiz seçim."); break;
                }
            }
        }
        private void List()
        {
            for (int index = 0; index < this.service.Count; index++)
                Console.WriteLine(this.service.Get(index).GetSummary());
        }
        private void Add()
        {
            Console.Write("Ad: "); string name = Console.ReadLine();
            Console.Write("Seri numarası: "); string serial = Console.ReadLine();
            bool added = this.service.Add(name, serial, EquipmentType.Tool);
            Console.WriteLine(added ? "Kayıt oluşturuldu." : "Kayıt oluşturulamadı.");
        }
        private void ChangeStatus()
        {
            Console.Write("Seri numarası: "); string serial = Console.ReadLine();
            bool changed = this.service.ChangeStatus(serial, EquipmentStatus.InMaintenance);
            Console.WriteLine(changed ? "Durum güncellendi." : "Kayıt bulunamadı.");
        }
    }
}
