using System;
using System.IO;
using CSharpProgramming.Data;
using CSharpProgramming.Inventory;
using CSharpProgramming.Validation;

namespace CSharpProgramming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dataDirectory = "DataFiles";
            Directory.CreateDirectory(dataDirectory);
            Equipment[] items =
            {
                new Equipment("LiPo Batarya", "BAT-001", DateTime.Now),
                new Equipment("GPS Modülü", "GPS-002", DateTime.Now.AddDays(-2))
            };
            foreach (Equipment item in items)
                Console.WriteLine(item.GetSummary() + " | Geçerli seri: " + SerialNumberValidator.IsValid(item.SerialNumber));

            InventoryFileStore.SaveText(Path.Combine(dataDirectory, "inventory.txt"), items);
            InventoryFileStore.SaveBinary(Path.Combine(dataDirectory, "inventory.bin"), items);
            Console.WriteLine("Metin kayıtları: " + string.Join(", ", InventoryFileStore.ReadTextLines(Path.Combine(dataDirectory, "inventory.txt"))));
            Console.WriteLine("İkili kayıt sayısı: " + InventoryFileStore.ReadBinaryCount(Path.Combine(dataDirectory, "inventory.bin")));

            byte[] countBytes = BitConverter.GetBytes(items.Length);
            byte[] buffer = new byte[8];
            Buffer.BlockCopy(countBytes, 0, buffer, 0, countBytes.Length);
            Console.WriteLine(string.Format("Tampon içindeki kayıt sayısı: {0}", BitConverter.ToInt32(buffer, 0)));
            Console.WriteLine("Ayrılmış bellek: " + GC.GetTotalMemory(false));
        }
    }
}
