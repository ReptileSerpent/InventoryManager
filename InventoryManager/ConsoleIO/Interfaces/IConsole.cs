namespace InventoryManager.ConsoleIO.Interfaces
{
    internal interface IConsole
    {
        public string? ReadLine();

        public void WriteLine(string value);
        public void Write(string value);
    }
}
