using System;
namespace CSharpProgramming.Inventory
{
    internal class Tool : Equipment
    {
        public Tool(string name, string serial, int calibrationIntervalMonths) : base(name, serial)
        { this.CalibrationIntervalMonths = calibrationIntervalMonths; this.categoryNote = "Teknik araç"; }
        public int CalibrationIntervalMonths { get; private set; }
        public void DisplayCalibrationInterval() { Console.WriteLine("Kalibrasyon aralığı: " + this.CalibrationIntervalMonths + " ay"); }
    }
}
