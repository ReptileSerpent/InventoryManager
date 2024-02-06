using InventoryManager.DatabaseAccess.Controllers;

namespace InventoryManager
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var databaseController = new DatabaseController();

            var IOManager = new ConsoleIO.IOManagers.MainMenuIOManager(databaseController);
            IOManager.ExecuteIO();
        }
    }
}