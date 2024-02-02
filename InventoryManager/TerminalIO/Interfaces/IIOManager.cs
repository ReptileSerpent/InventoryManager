using InventoryManager.Helpers;

namespace InventoryManager.TerminalIO.Interfaces
{
    internal interface IIOManager
    {
        public string CommandsInfo { get; }

        public Result ExecuteIO();
        public Result ProcessInput(string[] input);
    }
}
