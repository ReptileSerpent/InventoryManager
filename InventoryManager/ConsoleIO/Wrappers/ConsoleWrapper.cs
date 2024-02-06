using InventoryManager.ConsoleIO.Interfaces;

namespace InventoryManager.ConsoleIO.Wrappers
{
    internal class ConsoleWrapper : IConsole
    {
        public string? ReadLine() => Console.ReadLine();

        public void Write(string value) => Console.Write(value);
        public void WriteLine(string value) => Console.WriteLine(value);
    }
}
