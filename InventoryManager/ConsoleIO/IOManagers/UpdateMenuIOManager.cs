using InventoryManager.Data.Entities;
using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Requesters;
using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class UpdateMenuIOManager : MainMenuIOManager
    {
        public UpdateMenuIOManager(IConsole console, IDatabaseController databaseController) : base(console, databaseController) { }

        public override string CommandsInfo => " === Update entity menu === \nEntities updatable by code: product, category, warehouse, location\nEntities updatable by id: inventory_entry\nCommands: exit";

        public override Result ProcessInput(string[] input)
        {
            Result result;
            var lowercaseCommand = input[0].ToLower();
            switch (lowercaseCommand)
            {
                case "product":
                    Product product;
                    result = new UpdateCommandRequester(Console, DatabaseController).RequestEntityByCode<Product>(out product);
                    return result.IsSuccess ? DatabaseController.TryUpdateEntity(product) : result;
                case "category":
                    Category category;
                    result = new UpdateCommandRequester(Console, DatabaseController).RequestEntityByCode<Category>(out category);
                    return result.IsSuccess ? DatabaseController.TryUpdateEntity(category) : result;
                case "warehouse":
                    Warehouse warehouse;
                    result = new UpdateCommandRequester(Console, DatabaseController).RequestEntityByCode<Warehouse>(out warehouse);
                    return result.IsSuccess ? DatabaseController.TryUpdateEntity(warehouse) : result;
                case "location":
                    Location location;
                    result = new UpdateCommandRequester(Console, DatabaseController).RequestEntityByCode<Location>(out location);
                    return result.IsSuccess ? DatabaseController.TryUpdateEntity(location) : result;
                case "inventory_entry":
                    InventoryEntry inventoryEntry;
                    result = new UpdateCommandRequester(Console, DatabaseController).RequestEntityById<InventoryEntry>(out inventoryEntry);
                    return result.IsSuccess ? DatabaseController.TryUpdateEntity(inventoryEntry) : result;
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