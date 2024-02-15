using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;
using Microsoft.Extensions.Logging;

namespace InventoryManager.ConsoleIO.Requesters
{
    internal class CreateCommandRequester
    {
        public CreateCommandRequester(ILogger logger, IConsole console, IDatabaseController databaseController)
        {
            Logger = logger;
            Console = console;
            DatabaseController = databaseController;
        }

        private ILogger Logger { get; }
        private IConsole Console { get; }
        private IDatabaseController DatabaseController { get; }
        

        /// <summary>
        /// Requests property values from Console, except for Id.
        /// </summary>
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
                    var result = TypeConverter.TryConvertStringToType(input, property.PropertyType, DatabaseController, out object convertedValue);
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
