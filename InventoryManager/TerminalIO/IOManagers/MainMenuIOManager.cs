using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;
using InventoryManager.TerminalIO.Interfaces;

namespace InventoryManager.TerminalIO.IOManagers
{
    internal class MainMenuIOManager : IIOManager
    {
        public MainMenuIOManager(DatabaseController databaseController)
        {
            this.databaseController = databaseController;
        }

        protected DatabaseController databaseController { get; }
        public virtual string CommandsInfo => "Inventory Manager\nCommands: create, read, update, delete, exit";

        public Result ExecuteIO()
        {
            var shouldExit = false;
            while (!shouldExit)
            {
                Console.WriteLine(CommandsInfo);

                Console.Write("> ");
                var input = Console.ReadLine()?.Split(" ");
                if (input == null)
                {
                    shouldExit = true;
                    continue;
                }
                var result = ProcessInput(input);
                if (result.IsSuccess && !result.ReceivedExitCommand)
                    Console.WriteLine("Success");
                if (!result.IsSuccess)
                    Console.WriteLine("Error: " + result.ErrorDescription);
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
                    var createCommandIOManager = new CreateMenuIOManager(databaseController);
                    return createCommandIOManager.ExecuteIO();
                case "read":
                    var readCommandIOManager = new ReadMenuIOManager(databaseController);
                    return readCommandIOManager.ExecuteIO();
                case "update":
                    var updateCommandIOManager = new UpdateMenuIOManager(databaseController);
                    return updateCommandIOManager.ExecuteIO();
                case "delete":
                    var deleteCommandIOManager = new DeleteMenuIOManager(databaseController);
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