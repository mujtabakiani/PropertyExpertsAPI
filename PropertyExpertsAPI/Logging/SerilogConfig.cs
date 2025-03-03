using Serilog;

namespace PropertyExperts.API.Logging
{
    public static class SerilogConfig
    {
        public static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logging/logs/app.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
