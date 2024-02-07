using InventoryManager.Data.Entities;
using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Requesters;
using System.Text;
using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class ReadMenuIOManager : MainMenuIOManager
    {
        public ReadMenuIOManager(IConsole console, IDatabaseController databaseController) : base(console, databaseController) { }

        public override string CommandsInfo => " === Read entity menu === \nEntities readable by code: product, category, warehouse, location\nEntities readable by id: inventory_entry\nCommands: exit";

        public override Result ProcessInput(string[] input)
        {
            Result readResult;
            Result requestResult;
            var lowercaseCommand = input[0].ToLower();
            switch (lowercaseCommand)
            {
                case "product":
                    string productCode;
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestCode<Product>(out productCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityByCode(productCode, out Product product);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateDisplayablePropertyValues(product));
                    break;
                case "category":
                    string categoryCode;
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestCode<Category>(out categoryCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityByCode(categoryCode, out Category category);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateDisplayablePropertyValues(category));
                    break;
                case "warehouse":
                    string warehouseCode;
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestCode<Product>(out warehouseCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityByCode(warehouseCode, out Warehouse warehouse);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateDisplayablePropertyValues(warehouse));
                    break;
                case "location":
                    string locationCode;
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestCode<Product>(out locationCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityByCode(locationCode, out Location location);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateDisplayablePropertyValues(location));
                    break;
                case "inventory_entry":
                    uint inventoryEntryId;
                    requestResult = new IdentificationRequester(Console, DatabaseController).RequestId<Product>(out inventoryEntryId);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityById(inventoryEntryId, out InventoryEntry inventoryEntry);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateDisplayablePropertyValues(inventoryEntry));
                    break;
                case "exit":
                    readResult = new Result()
                    {
                        IsSuccess = true,
                        ReceivedExitCommand = true
                    };
                    break;
                default:
                    readResult = new Result()
                    {
                        IsSuccess = false,
                        ErrorDescription = "Invalid command: " + input[0]
                    };
                    break;
            }

            return readResult;
        }

        private string GenerateDisplayablePropertyValues<T>(T entity)
        {
            var stringBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
                stringBuilder.Append($"{property.Name} - {property.GetValue(entity)}\n");
            return stringBuilder.ToString();
        }
    }
}