using Serilog.Extensions.Logging;
using Serilog;

namespace InventoryManager.Helpers
{
    internal static class LoggerCreator
    {
        internal static Microsoft.Extensions.Logging.ILogger CreateLogger()
        {
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.File("log.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            return new SerilogLoggerFactory(serilogLogger)
                .CreateLogger("Logger");
        }
    }
}
