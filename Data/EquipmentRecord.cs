using System;

namespace CSharpProgramming.Data
{
    internal class EquipmentRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string EquipmentType { get; set; }
        public string Status { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Notes { get; set; }
    }
}
