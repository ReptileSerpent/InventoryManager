using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;

namespace InventoryManager.ConsoleIO.Requesters
{
    internal class CreateCommandRequester
    {
        public CreateCommandRequester(IConsole console, IDatabaseController databaseController)
        {
            Console = console;
            DatabaseController = databaseController;
        }

        private IDatabaseController DatabaseController { get; }
        private IConsole Console { get; }

        internal Result RequestPropertyValues<T>(out T entity) where T : Data.Interfaces.IEntity, new()
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
                    var result = TypeConverter.TryConvertStringToType(input, property.PropertyType, DatabaseController, out convertedValue);
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
