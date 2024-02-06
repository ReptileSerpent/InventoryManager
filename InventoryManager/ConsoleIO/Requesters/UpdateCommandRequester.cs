using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;

namespace InventoryManager.ConsoleIO.Requesters
{
    internal class UpdateCommandRequester
    {
        internal Result RequestEntityById<T>(IConsole console, IDatabaseController databaseController, out T entity) where T : class, Data.Interfaces.IEntity, new()
        {
            entity = new T();
            uint id;
            T readEntity;
            var shouldKeepAskingForId = true;
            while (shouldKeepAskingForId)
            {
                console.Write($"Id? ");
                var input = console.ReadLine();
                if (input == null)
                {
                    shouldKeepAskingForId = false;
                    continue;
                }

                object convertedValue;
                var conversionResult = TypeConverter.TryConvertStringToType(input, typeof(uint), databaseController, out convertedValue);
                if (conversionResult.IsSuccess)
                {
                    id = (uint)convertedValue;
                    var readResult = databaseController.TryReadEntityById(id, out readEntity);
                    if (!readResult.IsSuccess)
                        return readResult;

                    var result = RequestPropertyValues<T>(console, databaseController, readEntity);
                    if (result.IsSuccess)
                    {
                        entity = readEntity;
                        return result;
                    }

                    return result;
                }
                else
                    console.WriteLine($"Error: {conversionResult.ErrorDescription}. Please try again.");
            }

            return new Result();
        }

        internal Result RequestEntityByCode<T>(IConsole console, IDatabaseController databaseController, out T entity) where T : class, Data.Interfaces.IEntityWithCode, new()
        {
            entity = new T();
            T readEntity;

            console.Write($"Code? ");
            var input = console.ReadLine();
            if (input == null)
                return new Result() { IsSuccess = false, ErrorDescription = "Unexpected end of input" };
            var readResult = databaseController.TryReadEntityByCode(input, out readEntity);
            if (!readResult.IsSuccess)
                return readResult;

            var result = RequestPropertyValues<T>(console, databaseController, readEntity);
            if (result.IsSuccess)
            {
                entity = readEntity;
                return result;
            }

            return result;
        }

        internal Result RequestPropertyValues<T>(IConsole console, IDatabaseController databaseController, T entity) where T : Data.Interfaces.IEntity, new()
        {
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "Id" || property.Name == "Code")
                    continue;
                var shouldKeepAsking = true;
                while (shouldKeepAsking)
                {
                    console.Write($"{property.Name}? ");
                    var input = console.ReadLine();
                    if (input == null)
                    {
                        shouldKeepAsking = false;
                        continue;
                    }
                    object convertedValue;
                    var result = TypeConverter.TryConvertStringToType(input, property.PropertyType, databaseController, out convertedValue);
                    if (result.IsSuccess)
                    {
                        property.SetValue(entity, convertedValue);
                        shouldKeepAsking = false;
                    }
                    else
                        console.WriteLine($"Error: {result.ErrorDescription}. Please try again.");
                }
            }

            return new Result() { IsSuccess = true };
        }
    }
}