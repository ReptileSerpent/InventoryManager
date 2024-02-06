using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.Helpers;

namespace InventoryManager.ConsoleIO.Requesters
{
    internal class IdentificationRequester
    {
        internal Result RequestCode<T>(DatabaseController databaseController, out string code)
        {
            code = "";
            Console.Write($"Code? ");
            var input = Console.ReadLine();
            if (input == null)
                return new Result() { IsSuccess = false, ErrorDescription = "Unexpected end of input" };
            code = input;
            return new Result() { IsSuccess = true };
        }

        internal Result RequestId<T>(DatabaseController databaseController, out uint inventoryEntryId)
        {
            inventoryEntryId = 0;
            var shouldKeepAskingForId = true;
            while (shouldKeepAskingForId)
            {
                Console.Write($"Id? ");
                var input = Console.ReadLine();
                if (input == null)
                    return new Result() { IsSuccess = false, ErrorDescription = "Unexpected end of input" };

                object convertedValue;
                var conversionResult = TypeConverter.TryConvertStringToType(input, typeof(uint), databaseController, out convertedValue);
                if (conversionResult.IsSuccess)
                {
                    inventoryEntryId = (uint)convertedValue;
                    return new Result() { IsSuccess = true };
                }

                Console.WriteLine($"Error: {conversionResult.ErrorDescription}. Please try again.");
            }

            return new Result();
        }
    }
}