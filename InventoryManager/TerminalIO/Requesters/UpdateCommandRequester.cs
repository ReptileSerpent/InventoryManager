using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;

namespace InventoryManager.TerminalIO.Requesters
{
    internal class UpdateCommandRequester
    {
        internal Result RequestEntityById<T>(DatabaseController databaseController, out T entity) where T : class, Data.Interfaces.IEntity, new()
        {
            entity = new T();
            uint id;
            T readEntity;
            var shouldKeepAskingForId = true;
            while (shouldKeepAskingForId)
            {
                Console.Write($"Id? ");
                var input = Console.ReadLine();
                if (input == null)
                {
                    shouldKeepAskingForId = false;
                    continue;
                }

                object convertedValue;
                var conversionResult = TypeConverter.ConvertStringToType(input, typeof(uint), databaseController, out convertedValue);
                if (conversionResult.IsSuccess)
                {
                    id = (uint)convertedValue;
                    var readResult = databaseController.TryReadEntityById(id, out readEntity);
                    if (!readResult.IsSuccess)
                        return readResult;

                    var result = RequestPropertyValues<T>(databaseController, readEntity);
                    if (result.IsSuccess)
                    {
                        entity = readEntity;
                        return result;
                    }

                    return result;
                }
                else
                    Console.WriteLine($"Error: {conversionResult.ErrorDescription}. Please try again.");
            }

            return new Result();
        }

        internal Result RequestEntityByCode<T>(DatabaseController databaseController, out T entity) where T : class, Data.Interfaces.IEntityWithCode, new()
        {
            entity = new T();
            T readEntity;

            Console.Write($"Code? ");
            var input = Console.ReadLine();
            if (input == null)
                return new Result() { IsSuccess = false, ErrorDescription = "Unexpected end of input" };
            var readResult = databaseController.TryReadEntityByCode(input, out readEntity);
            if (!readResult.IsSuccess)
                return readResult;

            var result = RequestPropertyValues<T>(databaseController, readEntity);
            if (result.IsSuccess)
            {
                entity = readEntity;
                return result;
            }

            return result;
        }

        internal Result RequestPropertyValues<T>(DatabaseController databaseController, T entity) where T : Data.Interfaces.IEntity, new()
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "Id" || property.Name == "Code")
                    continue;
                var shouldKeepAsking = true;
                while (shouldKeepAsking)
                {
                    Console.Write($"{property.Name}? ");
                    var input = Console.ReadLine();
                    if (input == null)
                    {
                        shouldKeepAsking = false;
                        continue;
                    }
                    object convertedValue;
                    var result = TypeConverter.ConvertStringToType(input, property.PropertyType, databaseController, out convertedValue);
                    if (result.IsSuccess)
                    {
                        property.SetValue(entity, convertedValue);
                        shouldKeepAsking = false;
                    }
                    else
                        Console.WriteLine($"Error: {result.ErrorDescription}. Please try again.");
                }
            }

            return new Result() { IsSuccess = true };
        }
    }
}