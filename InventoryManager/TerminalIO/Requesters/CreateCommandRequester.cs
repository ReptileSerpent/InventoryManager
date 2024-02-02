using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;

namespace InventoryManager.TerminalIO.Requesters
{
    internal class CreateCommandRequester
    {
        internal Result RequestPropertyValues<T>(DatabaseController databaseController, out T entity) where T : Data.Interfaces.IEntity, new()
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
