using InventoryManager.Data.Entities;
using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Requesters;
using InventoryManager.ConsoleIO.Interfaces;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class UpdateMenuIOManager : MainMenuIOManager
    {
        public UpdateMenuIOManager(IConsole console, DatabaseController databaseController) : base(console, databaseController) { }

        public override string CommandsInfo => " === Update entity menu === \nEntities updatable by code: product, category, warehouse, location\nEntities updatable by id: inventory_entry\nCommands: exit";

        public override Result ProcessInput(string[] input)
        {
            Result result;
            var lowercaseCommand = input[0].ToLower();
            switch (lowercaseCommand)
            {
                case "product":
                    Product product;
                    result = new UpdateCommandRequester().RequestEntityByCode<Product>(console, databaseController, out product);
                    return result.IsSuccess ? databaseController.TryUpdateEntity(product) : result;
                case "category":
                    Category category;
                    result = new UpdateCommandRequester().RequestEntityByCode<Category>(console, databaseController, out category);
                    return result.IsSuccess ? databaseController.TryUpdateEntity(category) : result;
                case "warehouse":
                    Warehouse warehouse;
                    result = new UpdateCommandRequester().RequestEntityByCode<Warehouse>(console, databaseController, out warehouse);
                    return result.IsSuccess ? databaseController.TryUpdateEntity(warehouse) : result;
                case "location":
                    Location location;
                    result = new UpdateCommandRequester().RequestEntityByCode<Location>(console, databaseController, out location);
                    return result.IsSuccess ? databaseController.TryUpdateEntity(location) : result;
                case "inventory_entry":
                    InventoryEntry inventoryEntry;
                    result = new UpdateCommandRequester().RequestEntityById<InventoryEntry>(console, databaseController, out inventoryEntry);
                    return result.IsSuccess ? databaseController.TryUpdateEntity(inventoryEntry) : result;
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