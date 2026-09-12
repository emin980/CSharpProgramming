using System;

namespace CSharpProgramming.Inventory
{
    internal class Equipment
    {
        public string Name;
        public string SerialNumber;
        public string Status;
        public string Tags;

        public void SetInformation(string name, string serialNumber)
        {
            this.SetInformation(name, serialNumber, "Müsait");
        }

        public void SetInformation(string name, string serialNumber, string status)
        {
            this.Name = name;
            this.SerialNumber = serialNumber;
            this.Status = status;
        }

        public void AddTags(params string[] tags)
        {
            this.Tags = string.Join(", ", tags);
        }

        public string GetSummary()
        {
            return this.Name + " | " + this.SerialNumber + " | " + this.Status;
        }

        public void DisplayInspectionSteps(int stepNumber)
        {
            if (stepNumber <= 0)
            {
                Console.WriteLine("Denetim tamamlandı.");
                return;
            }

            Console.WriteLine("Denetim adımı: " + stepNumber);
            this.DisplayInspectionSteps(stepNumber - 1);
        }
    }
}
