using InventoryManager.Data.Entities;
using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Requesters;
using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class CreateMenuIOManager : MainMenuIOManager
    {
        public CreateMenuIOManager(ILogger logger, IConsole console, IDatabaseController databaseController) : base(logger, console, databaseController) { }

        public override string CommandsInfo => " === Create entity menu === \nOptions: product, category, warehouse, location, inventory_entry\nCommands: exit";

        public override Result ProcessInput(string[] input)
        {
            Result result;
            var lowercaseCommand = input[0].ToLower();
            switch (lowercaseCommand)
            {
                case "product":
                    Product product;
                    result = new CreateCommandRequester(Logger, Console, DatabaseController).RequestPropertyValues<Product>(out product);
                    return result.IsSuccess ? DatabaseController.TryCreateEntity(product) : result;
                case "category":
                    Category category;
                    result = new CreateCommandRequester(Logger, Console, DatabaseController).RequestPropertyValues<Category>(out category);
                    return result.IsSuccess ? DatabaseController.TryCreateEntity(category) : result;
                case "warehouse":
                    Warehouse warehouse;
                    result = new CreateCommandRequester(Logger, Console, DatabaseController).RequestPropertyValues<Warehouse>(out warehouse);
                    return result.IsSuccess ? DatabaseController.TryCreateEntity(warehouse) : result;
                case "location":
                    Location location;
                    result = new CreateCommandRequester(Logger, Console, DatabaseController).RequestPropertyValues<Location>(out location);
                    return result.IsSuccess ? DatabaseController.TryCreateEntity(location) : result;
                case "inventory_entry":
                    InventoryEntry inventoryEntry;
                    result = new CreateCommandRequester(Logger, Console, DatabaseController).RequestPropertyValues<InventoryEntry>(out inventoryEntry);
                    return result.IsSuccess ? DatabaseController.TryCreateEntity(inventoryEntry) : result;
                case "exit":
                    result = new Result()
                    {
                        IsSuccess = true,
                        ReceivedExitCommand = true
                    };
                    break;
                default:
                    result = new Result()
                    {
                        IsSuccess = false,
                        ErrorDescription = $"Invalid command: {input[0]}"
                    };
                    break;
            }
            return result;
        }
    }
}