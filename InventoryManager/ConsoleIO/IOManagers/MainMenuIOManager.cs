﻿using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class MainMenuIOManager : IIOManager
    {
        public MainMenuIOManager(IConsole console, IDatabaseController databaseController)
        {
            Console = console;
            DatabaseController = databaseController;
        }

        protected IConsole Console { get; }
        protected IDatabaseController DatabaseController { get; }

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
                    var createCommandIOManager = new CreateMenuIOManager(Console, DatabaseController);
                    return createCommandIOManager.ExecuteIO();
                case "read":
                    var readCommandIOManager = new ReadMenuIOManager(Console, DatabaseController);
                    return readCommandIOManager.ExecuteIO();
                case "update":
                    var updateCommandIOManager = new UpdateMenuIOManager(Console, DatabaseController);
                    return updateCommandIOManager.ExecuteIO();
                case "delete":
                    var deleteCommandIOManager = new DeleteMenuIOManager(Console, DatabaseController);
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