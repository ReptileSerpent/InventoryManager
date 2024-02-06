using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;

namespace InventoryManager.ConsoleIO.Requesters
{
    internal class CreateCommandRequester
    {
        internal Result RequestPropertyValues<T>(IConsole console, DatabaseController databaseController, out T entity) where T : Data.Interfaces.IEntity, new()
        {
            var properties = typeof(T).GetProperties();
            entity = new T();
            foreach (var property in properties)
            {
                if (property.Name == "Id")
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
