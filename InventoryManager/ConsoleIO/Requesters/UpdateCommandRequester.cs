using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;
using Microsoft.Extensions.Logging;

namespace InventoryManager.ConsoleIO.Requesters
{
    internal class UpdateCommandRequester
    {
        public UpdateCommandRequester(ILogger logger, IConsole console, IDatabaseController databaseController)
        {
            Logger = logger;
            Console = console;
            DatabaseController = databaseController;
        }

        private ILogger Logger { get; }
        private IConsole Console { get; }
        private IDatabaseController DatabaseController { get; }

        /// <summary>
        /// Requests id from Console, checks whether id is a valid uint, checks whether entity with given id exists, and calls RequestPropertyValues.
        /// </summary>
        internal Result RequestEntityById<T>(out T entity) where T : class, Data.Interfaces.IEntity, new()
        {
            entity = new T();
            uint id;
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

                var conversionResult = TypeConverter.TryConvertStringToType(input, typeof(uint), DatabaseController, out object convertedValue);
                if (conversionResult.IsSuccess)
                {
                    id = (uint)convertedValue;
                    var readResult = DatabaseController.TryReadEntityById(id, out T readEntity);
                    if (!readResult.IsSuccess)
                        return readResult;

                    var result = RequestPropertyValues<T>(readEntity);
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

        /// <summary>
        /// Requests code from Console, checks whether entity with given code exists, and calls RequestPropertyValues.
        /// </summary>
        internal Result RequestEntityByCode<T>(out T entity) where T : class, Data.Interfaces.IEntityWithCode, new()
        {
            entity = new T();

            Console.Write($"Code? ");
            var input = Console.ReadLine();
            if (input == null)
                return new Result() { IsSuccess = false, ErrorDescription = "Unexpected end of input" };
            var readResult = DatabaseController.TryReadEntityByCode(input, out T readEntity);
            if (!readResult.IsSuccess)
                return readResult;

            var result = RequestPropertyValues<T>(readEntity);
            if (result.IsSuccess)
            {
                entity = readEntity;
                return result;
            }

            return result;
        }

        /// <summary>
        /// Requests property values from Console, except for Id and Code.
        /// </summary>
        internal Result RequestPropertyValues<T>(T entity) where T : Data.Interfaces.IEntity, new()
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