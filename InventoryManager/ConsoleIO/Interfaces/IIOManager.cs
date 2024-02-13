using InventoryManager.Helpers;

namespace InventoryManager.ConsoleIO.Interfaces
{
    internal interface IIOManager
    {
        /// <summary>
        /// Gets information about the current menu and available commands. Displayed to the user in the ExecuteIO method.
        /// </summary>
        public string CommandsInfo { get; }

        /// <summary>
        /// Requests user input in a loop until an exit command is issued, repeats requests on invalid input, displays error and success messages.
        /// </summary>
        public Result ExecuteIO();
        public Result ProcessInput(string[] input);
    }
}
