using InventoryManager.ConsoleIO.Interfaces;
using InventoryManager.DatabaseAccess.Controllers;
using InventoryManager.DatabaseAccess.Interfaces;
using InventoryManager.Helpers;

namespace InventoryManager.ConsoleIO.Requesters
{
    internal class IdentificationRequester
    {
        internal Result RequestCode<T>(IConsole console, IDatabaseController databaseController, out string code)
        {
            code = "";
            console.Write($"Code? ");
            var input = console.ReadLine();
            if (input == null)
                return new Result() { IsSuccess = false, ErrorDescription = "Unexpected end of input" };
            code = input;
            return new Result() { IsSuccess = true };
        }

        internal Result RequestId<T>(IConsole console, IDatabaseController databaseController, out uint inventoryEntryId)
        {
            inventoryEntryId = 0;
            var shouldKeepAskingForId = true;
            while (shouldKeepAskingForId)
            {
                console.Write($"Id? ");
                var input = console.ReadLine();
                if (input == null)
                    return new Result() { IsSuccess = false, ErrorDescription = "Unexpected end of input" };

                object convertedValue;
                var conversionResult = TypeConverter.TryConvertStringToType(input, typeof(uint), databaseController, out convertedValue);
                if (conversionResult.IsSuccess)
                {
                    inventoryEntryId = (uint)convertedValue;
                    return new Result() { IsSuccess = true };
                }

                console.WriteLine($"Error: {conversionResult.ErrorDescription}. Please try again.");
            }

            return new Result();
        }
    }
}