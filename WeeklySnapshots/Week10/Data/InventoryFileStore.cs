using System.IO;
using CSharpProgramming.Inventory;
namespace CSharpProgramming.Data
{
    internal class InventoryFileStore
    {
        public static void SaveText(string path, Equipment[] items)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
                foreach (Equipment item in items) writer.WriteLine(item.SerialNumber + "|" + item.Name + "|" + item.RegisteredAt.ToString("O"));
        }
        public static string[] ReadTextLines(string path) { return File.ReadAllLines(path); }
        public static void SaveBinary(string path, Equipment[] items)
        {
            using (Stream stream = new FileStream(path, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(items.Length);
                foreach (Equipment item in items) { writer.Write(item.SerialNumber); writer.Write(item.Name); }
            }
        }
        public static int ReadBinaryCount(string path)
        {
            using (Stream stream = new FileStream(path, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream)) return reader.ReadInt32();
        }
    }
}
