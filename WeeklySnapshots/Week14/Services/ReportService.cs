using System;
using System.Collections.Generic;
namespace CSharpProgramming.Services
{
    internal class ReportService
    {
        public void Display<T>(IEnumerable<T> items, Func<T, string> formatter)
        { foreach (T item in items) Console.WriteLine(formatter(item)); }
    }
}
