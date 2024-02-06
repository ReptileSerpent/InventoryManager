using InventoryManager.Data.Entities;
using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Requesters;
using InventoryManager.ConsoleIO.Interfaces;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class DeleteMenuIOManager : MainMenuIOManager
    {
        public DeleteMenuIOManager(IConsole console, DatabaseController databaseController) : base(console, databaseController) { }

        public override string CommandsInfo => " === Delete entity menu === \nEntities deletable by code: product, category, warehouse, location\nEntities deletable by id: inventory_entry\nCommands: exit";

        public override Result ProcessInput(string[] input)
        {
            Result deletionResult;
            Result requestResult;
            var lowercaseCommand = input[0].ToLower();
            switch (lowercaseCommand)
            {
                case "product":
                    string productCode;
                    requestResult = new IdentificationRequester().RequestCode<Product>(console, databaseController, out productCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = databaseController.TryDeleteEntityByCode<Product>(productCode);
                    break;
                case "category":
                    string categoryCode;
                    requestResult = new IdentificationRequester().RequestCode<Category>(console, databaseController, out categoryCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = databaseController.TryDeleteEntityByCode<Category>(categoryCode);
                    break;
                case "warehouse":
                    string warehouseCode;
                    requestResult = new IdentificationRequester().RequestCode<Product>(console, databaseController, out warehouseCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = databaseController.TryDeleteEntityByCode<Warehouse>(warehouseCode);
                    break;
                case "location":
                    string locationCode;
                    requestResult = new IdentificationRequester().RequestCode<Product>(console, databaseController, out locationCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = databaseController.TryDeleteEntityByCode<Location>(locationCode);
                    break;
                case "inventory_entry":
                    uint inventoryEntryId;
                    requestResult = new IdentificationRequester().RequestId<Product>(console, databaseController, out inventoryEntryId);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = databaseController.TryDeleteEntityById<InventoryEntry>(inventoryEntryId);
                    break;
                case "exit":
                    deletionResult = new Result()
                    {
                        IsSuccess = true,
                        ReceivedExitCommand = true
                    };
                    break;
                default:
                    deletionResult = new Result()
                    {
                        IsSuccess = false,
                        ErrorDescription = "Invalid command: " + input[0]
                    };
                    break;
            }

            return deletionResult;
        }
    }
}