using InventoryManager.Data.Entities;
using InventoryManager.Data.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;

namespace InventoryManager.Helpers
{
    internal class TypeConverter
    {
        /// <summary>
        /// Converts the input to the specified type. Accepted types: string, uint, decimal, Product, Category, Warehouse, Location, InventoryEntry.
        /// </summary>
        // Method cannot be made generic due to the need to convert to arbitrary types in Requester classes
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
                    var result = TryConvertToUint(input, out uint uintValue);
                    convertedValue = uintValue;
                    return result;
                }
                case Type _ when type == typeof(decimal):
                {
                    var result = TryConvertToDecimal(input, out decimal decimalValue);
                    convertedValue = decimalValue;
                    return result;
                }
                case Type _ when type == typeof(Product):
                {
                    var result = TryConvertToEntityWithCodeByCode(input, databaseController, out Product product);
                    convertedValue = product;
                    return result;
                }
                case Type _ when type == typeof(Category):
                {
                    var result = TryConvertToEntityWithCodeByCode(input, databaseController, out Category category);
                    convertedValue = category;
                    return result;
                }
                case Type _ when type == typeof(Warehouse):
                {
                    var result = TryConvertToEntityWithCodeByCode(input, databaseController, out Warehouse warehouse);
                    convertedValue = warehouse;
                    return result;
                }
                case Type _ when type == typeof(Location):
                {
                    var result = TryConvertToEntityWithCodeByCode(input, databaseController, out Location location);
                    convertedValue = location;
                    return result;
                }
                case Type _ when type == typeof(InventoryEntry):
                {
                    var result = TryConvertToEntityById(input, databaseController, out InventoryEntry inventoryEntry);
                    convertedValue = inventoryEntry;
                    return result;
                }
            }

            return new Result() { IsSuccess = false, ErrorDescription = $"Invalid input: {input}" };
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
            var readResult = databaseController.TryReadEntityByCode<T>(input, out convertedValue);
            if (!readResult.IsSuccess)
                return new Result() { IsSuccess = false, ErrorDescription = $"Code not found: {input}" };

            return new Result() { IsSuccess = true };
        }

        private static Result TryConvertToEntityById<T>(string input, IDatabaseController databaseController, out T convertedValue) where T : class, IEntity, new()
        {
            convertedValue = new T();
            var uintConversionResult = TryConvertToUint(input, out uint id);
            if (!uintConversionResult.IsSuccess)
                return uintConversionResult;

            var inventoryEntryReadResult = databaseController.TryReadEntityById<T>(id, out convertedValue);
            if (!inventoryEntryReadResult.IsSuccess)
                return new Result() { IsSuccess = false, ErrorDescription = $"Id not found: {id}" };

            return new Result() { IsSuccess = true };
        }
    }
}
