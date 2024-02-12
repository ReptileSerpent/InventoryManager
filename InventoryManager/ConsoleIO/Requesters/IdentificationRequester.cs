using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;

namespace InventoryManager.ConsoleIO.Requesters
{
    internal class IdentificationRequester
    {
        public IdentificationRequester(IConsole console, IDatabaseController databaseController)
        {
            Console = console;
            DatabaseController = databaseController;
        }

        private IDatabaseController DatabaseController { get; }
        private IConsole Console { get; }

        internal Result RequestCode<T>(out string code)
        {
            code = "";
            Console.Write($"Code? ");
            var input = Console.ReadLine();
            if (input == null)
                return new Result() { IsSuccess = false, ErrorDescription = "Unexpected end of input" };
            code = input;
            return new Result() { IsSuccess = true };
        }

        internal Result RequestId<T>(out uint id)
        {
            id = 0;
            var shouldKeepAsking = true;
            while (shouldKeepAsking)
            {
                Console.Write($"Id? ");
                var input = Console.ReadLine();
                if (input == null)
                    return new Result() { IsSuccess = false, ErrorDescription = "Unexpected end of input" };

                object convertedValue;
                var conversionResult = TypeConverter.TryConvertStringToType(input, typeof(uint), DatabaseController, out convertedValue);
                if (conversionResult.IsSuccess)
                {
                    id = (uint)convertedValue;
                    return new Result() { IsSuccess = true };
                }

                Console.WriteLine($"Error: {conversionResult.ErrorDescription}. Please try again.");
            }

            return new Result();
        }
    }
}