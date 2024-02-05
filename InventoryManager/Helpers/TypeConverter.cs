using InventoryManager.Data.Entities;
using InventoryManager.Data.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;

namespace InventoryManager.Helpers
{
    internal class TypeConverter
    {
        internal static Result TryConvertStringToType(string input, Type type, IDatabaseController databaseController, out object convertedValue)
        {
            convertedValue = new object();
            switch (type)
            {
                case Type _ when type == typeof(string):
                    convertedValue = input;
                    return new Result() { IsSuccess = true };
                case Type _ when type == typeof(uint):
                {
                    uint uintValue;
                    var result = TryConvertToUint(input, out uintValue);
                    convertedValue = uintValue;
                    return result;
                }
                case Type _ when type == typeof(decimal):
                {
                    decimal decimalValue;
                    var result = TryConvertToDecimal(input, out decimalValue);
                    convertedValue = decimalValue;
                    return result;
                }
                case Type _ when type == typeof(Product):
                {
                    Product product;
                    var result = TryConvertToEntityWithCodeByCode<Product>(input, databaseController, out product);
                    convertedValue = product;
                    return result;
                }
                case Type _ when type == typeof(Category):
                {
                    Category category;
                    var result = TryConvertToEntityWithCodeByCode<Category>(input, databaseController, out category);
                    convertedValue = category;
                    return result;
                }
                case Type _ when type == typeof(Warehouse):
                {
                    Warehouse warehouse;
                    var result = TryConvertToEntityWithCodeByCode<Warehouse>(input, databaseController, out warehouse);
                    convertedValue = warehouse;
                    return result;
                }
                case Type _ when type == typeof(Location):
                {
                    Location location;
                    var result = TryConvertToEntityWithCodeByCode<Location>(input, databaseController, out location);
                    convertedValue = location;
                    return result;
                }
                case Type _ when type == typeof(InventoryEntry):
                {
                    InventoryEntry inventoryEntry;
                    var result = TryConvertToEntityById<InventoryEntry>(input, databaseController, out inventoryEntry);
                    convertedValue = inventoryEntry;
                    return result;
                }
            }

            return new Result() { IsSuccess = false, ErrorDescription = "Invalid input: " + input };
        }

        private static Result TryConvertToUint(string input, out uint convertedValue)
        {
            if (!uint.TryParse(input, out convertedValue))
                return new Result() { IsSuccess = false, ErrorDescription = $"Invalid uint value: {input}" };

            return new Result() { IsSuccess = true };
        }

        private static Result TryConvertToDecimal(string input, out decimal convertedValue)
        {
            if (!decimal.TryParse(input, out convertedValue))
                return new Result() { IsSuccess = false, ErrorDescription = $"Invalid decimal value: {input}" };

            return new Result() { IsSuccess = true };
        }

        private static Result TryConvertToEntityWithCodeByCode<T>(string input, IDatabaseController databaseController, out T convertedValue) where T : class, IEntityWithCode, new()
        {
            var productReadResult = databaseController.TryReadEntityByCode<T>(input, out convertedValue);
            if (!productReadResult.IsSuccess)
                return new Result() { IsSuccess = false, ErrorDescription = $"Сode not found: {input}" };

            return new Result() { IsSuccess = true };
        }

        private static Result TryConvertToEntityById<T>(string input, IDatabaseController databaseController, out T convertedValue) where T : class, IEntity, new()
        {
            convertedValue = new T();
            uint id;
            var uintConversionResult = TryConvertToUint(input, out id);
            if (!uintConversionResult.IsSuccess)
                return uintConversionResult;

            var inventoryEntryReadResult = databaseController.TryReadEntityById<T>(id, out convertedValue);
            if (!inventoryEntryReadResult.IsSuccess)
                return new Result() { IsSuccess = false, ErrorDescription = $"Id not found: {id}" };

            return new Result() { IsSuccess = true };
        }
    }
}
