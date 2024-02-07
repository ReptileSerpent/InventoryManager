using InventoryManager.Data.Entities;
using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Requesters;
using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class DeleteMenuIOManager : MainMenuIOManager
    {
        public DeleteMenuIOManager(IConsole console, IDatabaseController databaseController) : base(console, databaseController) { }

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
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestCode<Product>(out productCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = DatabaseController.TryDeleteEntityByCode<Product>(productCode);
                    break;
                case "category":
                    string categoryCode;
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestCode<Category>(out categoryCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = DatabaseController.TryDeleteEntityByCode<Category>(categoryCode);
                    break;
                case "warehouse":
                    string warehouseCode;
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestCode<Warehouse>(out warehouseCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = DatabaseController.TryDeleteEntityByCode<Warehouse>(warehouseCode);
                    break;
                case "location":
                    string locationCode;
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestCode<Location>(out locationCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = DatabaseController.TryDeleteEntityByCode<Location>(locationCode);
                    break;
                case "inventory_entry":
                    uint inventoryEntryId;
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestId<InventoryEntry>(out inventoryEntryId);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    deletionResult = DatabaseController.TryDeleteEntityById<InventoryEntry>(inventoryEntryId);
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