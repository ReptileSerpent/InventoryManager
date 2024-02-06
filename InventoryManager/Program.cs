using InventoryManager.ConsoleIO.Wrappers;
using InventoryManager.DatabaseAccess.Controllers;
using System;

namespace InventoryManager
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var databaseController = new DatabaseController();
            var console = new ConsoleWrapper();

            var IOManager = new ConsoleIO.IOManagers.MainMenuIOManager(console, databaseController);
            IOManager.ExecuteIO();
        }
    }
}