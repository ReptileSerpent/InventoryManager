using InventoryManager.Data.Entities;
using InventoryManager.Helpers;
using InventoryManager.ConsoleIO.Requesters;
using System.Text;
using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;
using Microsoft.Extensions.Logging;
using InventoryManager.Data.Interfaces;

namespace InventoryManager.ConsoleIO.IOManagers
{
    internal class ReadMenuIOManager : MainMenuIOManager
    {
        public ReadMenuIOManager(ILogger logger, IConsole console, IDatabaseController databaseController) : base(logger, console, databaseController) { }

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
                    requestResult = new IdentificationRequester(Logger, Console, DatabaseController).RequestCode<Product>(out productCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityByCode(productCode, out Product product);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateFormattedPropertyValuesString(product));
                    break;
                case "category":
                    string categoryCode;
                    requestResult = new IdentificationRequester(Logger, Console, DatabaseController).RequestCode<Category>(out categoryCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityByCode(categoryCode, out Category category);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateFormattedPropertyValuesString(category));
                    break;
                case "warehouse":
                    string warehouseCode;
                    requestResult = new IdentificationRequester(Logger, Console, DatabaseController).RequestCode<Warehouse>(out warehouseCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityByCode(warehouseCode, out Warehouse warehouse);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateFormattedPropertyValuesString(warehouse));
                    break;
                case "location":
                    string locationCode;
                    requestResult = new IdentificationRequester(Logger, Console, DatabaseController).RequestCode<Location>(out locationCode);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityByCode(locationCode, out Location location);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateFormattedPropertyValuesString(location));
                    break;
                case "inventory_entry":
                    uint inventoryEntryId;
                    requestResult = new IdentificationRequester(Logger, Console, DatabaseController).RequestId<InventoryEntry>(out inventoryEntryId);
                    if (!requestResult.IsSuccess)
                        return requestResult;
                    readResult = DatabaseController.TryReadEntityById(inventoryEntryId, out InventoryEntry inventoryEntry);
                    if (readResult.IsSuccess)
                        Console.WriteLine(GenerateFormattedPropertyValuesString(inventoryEntry));
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
                        ErrorDescription = $"Invalid command: {input[0]}"
                    };
                    break;
            }

            return readResult;
        }

        private string GenerateFormattedPropertyValuesString<T>(T entity)
        {
            var stringBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsAssignableTo(typeof(IEntityWithCode)))
                {
                    var linkedEntity = property.GetValue(entity);
                    var code = ((IEntityWithCode)linkedEntity!).Code;
                    stringBuilder.Append($"{property.Name} - {code}\n");
                }
                else
                    stringBuilder.Append($"{property.Name} - {property.GetValue(entity)}\n");
            }

            return stringBuilder.ToString();
        }
    }
}