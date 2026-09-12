using System;
namespace CSharpProgramming.Exceptions
{
    internal class EquipmentNotFoundException : Exception
    { public EquipmentNotFoundException(string serial) : base("Ekipman bulunamadı: " + serial) { } }
}
