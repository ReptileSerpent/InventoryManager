using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class MainMenuIOManager : IIOManager
    {
        public MainMenuIOManager(IConsole console, IDatabaseController databaseController)
        {
            this.databaseController = databaseController;
            this.console = console;
        }

        protected IDatabaseController databaseController { get; }
        protected IConsole console { get; }
        public virtual string CommandsInfo => "Inventory Manager\nCommands: create, read, update, delete, exit";

        public Result ExecuteIO()
        {
            var shouldExit = false;
            while (!shouldExit)
            {
                console.WriteLine(CommandsInfo);

                console.Write("> ");
                var input = console.ReadLine()?.Split(" ");
                if (input == null)
                {
                    shouldExit = true;
                    continue;
                }
                var result = ProcessInput(input);
                if (result.IsSuccess && !result.ReceivedExitCommand)
                    console.WriteLine("Success");
                if (!result.IsSuccess)
                    console.WriteLine("Error: " + result.ErrorDescription);
                if (result.ReceivedExitCommand)
                    shouldExit = true;
            }
            return new Result() { IsSuccess = true };
        }

        public virtual Result ProcessInput(string[] input)
        {
            var lowercaseCommand = input[0].ToLower();
            switch (lowercaseCommand)
            {
                case "create":
                    var createCommandIOManager = new CreateMenuIOManager(console, databaseController);
                    return createCommandIOManager.ExecuteIO();
                case "read":
                    var readCommandIOManager = new ReadMenuIOManager(console, databaseController);
                    return readCommandIOManager.ExecuteIO();
                case "update":
                    var updateCommandIOManager = new UpdateMenuIOManager(console, databaseController);
                    return updateCommandIOManager.ExecuteIO();
                case "delete":
                    var deleteCommandIOManager = new DeleteMenuIOManager(console, databaseController);
                    return deleteCommandIOManager.ExecuteIO();
                case "exit":
                    return new Result()
                    {
                        IsSuccess = true,
                        ReceivedExitCommand = true
                    };
                default:
                    return new Result()
                    {
                        IsSuccess = false,
                        ErrorDescription = $"Invalid command: {input[0]}"
                    };
            }
        }
    }
}