using InventoryManager.ConsoleIO.Wrappers;
using InventoryManager.Data;
using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;
using Microsoft.Extensions.Logging;
using Serilog;

namespace InventoryManager
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            var logger = LoggerCreator.CreateLogger();
            try
            {
                logger.LogInformation("InventoryManager started");
                var databaseController = new DatabaseController(logger, new InventoryContext());
                var console = new ConsoleWrapper();

                var IOManager = new ConsoleIO.IOManagers.MainMenuIOManager(logger, console, databaseController);
                IOManager.ExecuteIO();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unhandled exception");
            }
            finally
            {
                logger.LogInformation("Closing InventoryManager");
                Log.CloseAndFlush();
            }
        }
    }
}