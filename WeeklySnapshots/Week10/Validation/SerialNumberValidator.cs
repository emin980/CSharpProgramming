using System.Text.RegularExpressions;
namespace CSharpProgramming.Validation
{
    internal class SerialNumberValidator
    {
        public static bool IsValid(string serialNumber) { return Regex.IsMatch(serialNumber, "^[A-Z]{3}-[0-9]{3}$"); }
    }
}
