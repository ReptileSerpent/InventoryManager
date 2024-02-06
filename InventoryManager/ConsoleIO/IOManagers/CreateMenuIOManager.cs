using InventoryManager.Data.Entities;
using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Requesters;
using InventoryManager.ConsoleIO.Interfaces;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class CreateMenuIOManager : MainMenuIOManager
    {
        public CreateMenuIOManager(IConsole console, DatabaseController databaseController) : base(console, databaseController) { }

        public override string CommandsInfo => " === Create entity menu === \nOptions: product, category, warehouse, location, inventory_entry\nCommands: exit";

        public override Result ProcessInput(string[] input)
        {
            Result result;
            var lowercaseCommand = input[0].ToLower();
            switch (lowercaseCommand)
            {
                case "product":
                    Product product;
                    result = new CreateCommandRequester().RequestPropertyValues<Product>(console, databaseController, out product);
                    return result.IsSuccess ? databaseController.TryCreateEntity(product) : result;
                case "category":
                    Category category;
                    result = new CreateCommandRequester().RequestPropertyValues<Category>(console, databaseController, out category);
                    return result.IsSuccess ? databaseController.TryCreateEntity(category) : result;
                case "warehouse":
                    Warehouse warehouse;
                    result = new CreateCommandRequester().RequestPropertyValues<Warehouse>(console, databaseController, out warehouse);
                    return result.IsSuccess ? databaseController.TryCreateEntity(warehouse) : result;
                case "location":
                    Location location;
                    result = new CreateCommandRequester().RequestPropertyValues<Location>(console, databaseController, out location);
                    return result.IsSuccess ? databaseController.TryCreateEntity(location) : result;
                case "inventory_entry":
                    InventoryEntry inventoryEntry;
                    result = new CreateCommandRequester().RequestPropertyValues<InventoryEntry>(console, databaseController, out inventoryEntry);
                    return result.IsSuccess ? databaseController.TryCreateEntity(inventoryEntry) : result;
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
                        ErrorDescription = "Invalid command: " + input[0]
                    };
                    break;
            }
            return result;
        }
    }
}