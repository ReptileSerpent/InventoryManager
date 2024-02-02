using InventoryManager.Data.Entities;
using InventoryManager.DatabaseAccess.Interfaces;

namespace InventoryManager.Helpers
{
    internal class TypeConverter
    {
        internal static Result ConvertStringToType(string input, Type type, IDatabaseController databaseController, out object convertedValue)
        {
            convertedValue = new object();
            switch (type)
            {
                case Type _ when type == typeof(string):
                    convertedValue = input;
                    return new Result() { IsSuccess = true };
                case Type _ when type == typeof(uint):
                    uint uintValue;
                    if (!uint.TryParse(input, out uintValue))
                        return new Result() { IsSuccess = false, ErrorDescription = $"Invalid uint value: {input}" };

                    convertedValue = uintValue;
                    return new Result() { IsSuccess = true };
                case Type _ when type == typeof(decimal):
                    decimal decimalValue;
                    if (!decimal.TryParse(input, out decimalValue))
                        return new Result() { IsSuccess = false, ErrorDescription = $"Invalid decimal value: {input}" };

                    convertedValue = decimalValue;
                    return new Result() { IsSuccess = true };
                case Type _ when type == typeof(Product):
                    var productReadResult = databaseController.TryReadEntityByCode<Product>(input, out Product product);
                    if (!productReadResult.IsSuccess)
                        return new Result() { IsSuccess = false, ErrorDescription = $"Сode not found: {input}" };

                    convertedValue = product;
                    return new Result() { IsSuccess = true };
                case Type _ when type == typeof(Category):
                    var categoryReadResult = databaseController.TryReadEntityByCode<Category>(input, out Category category);
                    if (!categoryReadResult.IsSuccess)
                        return new Result() { IsSuccess = false, ErrorDescription = $"Сode not found: {input}" };

                    convertedValue = category;
                    return new Result() { IsSuccess = true };
                case Type _ when type == typeof(Warehouse):
                    var warehouseReadResult = databaseController.TryReadEntityByCode<Warehouse>(input, out Warehouse warehouse);
                    if (!warehouseReadResult.IsSuccess)
                        return new Result() { IsSuccess = false, ErrorDescription = $"Сode not found: {input}" };

                    convertedValue = warehouse;
                    return new Result() { IsSuccess = true };
                case Type _ when type == typeof(Location):
                    var locationReadResult = databaseController.TryReadEntityByCode<Location>(input, out Location location);
                    if (!locationReadResult.IsSuccess)
                        return new Result() { IsSuccess = false, ErrorDescription = $"Сode not found: {input}" };

                    convertedValue = location;
                    return new Result() { IsSuccess = true };
                case Type _ when type == typeof(InventoryEntry):
                    uint inventoryId;
                    if (!uint.TryParse(input, out inventoryId))
                        return new Result() { IsSuccess = false, ErrorDescription = $"Invalid uint value: {input}" };

                    var inventoryEntryReadResult = databaseController.TryReadEntityById<InventoryEntry>(inventoryId, out InventoryEntry inventoryEntry);
                    if (!inventoryEntryReadResult.IsSuccess)
                        return new Result() { IsSuccess = false, ErrorDescription = $"Id not found: {inventoryId}" };

                    convertedValue = inventoryEntry;
                    return new Result() { IsSuccess = true };
            }
            return new Result() { IsSuccess = false, ErrorDescription = "Invalid input: " + input };
        }
    }
}
