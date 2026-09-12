using System;
namespace CSharpProgramming.Exceptions
{
    internal class InvalidEquipmentOperationException : Exception
    { public InvalidEquipmentOperationException(string message) : base(message) { } }
}
