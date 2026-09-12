using System;
using CSharpProgramming.Exceptions;
using CSharpProgramming.Inventory;
namespace CSharpProgramming.Services
{
    internal delegate void EquipmentNotificationHandler(string message);
    internal delegate void EquipmentStatusEventHandler(object sender, EquipmentEventArgs eventArgs);
    internal class InventoryService
    {
        private Equipment[] items = new Equipment[12];
        private int count;
        public event EquipmentStatusEventHandler EquipmentStatusChanged;
        public void Add(Equipment equipment)
        {
            if (this.count >= this.items.Length) throw new InvalidEquipmentOperationException("Envanter kapasitesi dolmuştur.");
            this.items[this.count++] = equipment;
        }
        public void ChangeStatus(string serial, EquipmentStatus status)
        {
            Equipment equipment = this.Find(serial);
            equipment.Status = status;
            if (this.EquipmentStatusChanged != null)
                this.EquipmentStatusChanged(this, new EquipmentEventArgs(equipment, status));
        }
        private Equipment Find(string serial)
        {
            for (int index = 0; index < this.count; index++)
                if (this.items[index].SerialNumber == serial) return this.items[index];
            throw new EquipmentNotFoundException(serial);
        }
    }
}
