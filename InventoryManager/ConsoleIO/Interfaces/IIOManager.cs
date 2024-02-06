using InventoryManager.Helpers;

namespace InventoryManager.ConsoleIO.Interfaces
{
    internal interface IIOManager
    {
        public string CommandsInfo { get; }

        public Result ExecuteIO();
        public Result ProcessInput(string[] input);
    }
}
